using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BellScheduleManager.Common.Options;
using BellScheduleManager.Resources.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BellScheduleManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDataStore _dataStore;
        private readonly GoogleAuthOptions _googleAuthOptions;
        private GoogleAuthorizationCodeFlow flow => new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleAuthOptions.ClientId,
                ClientSecret = _googleAuthOptions.ClientSecret
            },
            Scopes = new List<string>
            {
                "openid",
                "profile",
                "email",
                CalendarService.Scope.Calendar // + ".app.created" is a more restricted scope, but the Calendar APIs do not seem to support it yet, even though it is included in the Google API console
            },
            DataStore = _dataStore
        });

        public AuthController(IDataStore dataStore, IOptions<GoogleAuthOptions> googleAuthOptions)
        {
            _dataStore = dataStore;
            _googleAuthOptions = googleAuthOptions.Value;
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var model = new UserModel()
            {
                DisplayName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? User.Identity.Name,
                Email = User.Identity.Name
            };

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet("google-login")]
        public IActionResult AuthForGoogle()
        {
            var authRequest = flow.CreateAuthorizationCodeRequest(_googleAuthOptions.RedirectUrl).Build();
            return Ok(new { redirectUrl = authRequest.ToString() });
        }

        [AllowAnonymous]
        [HttpGet("google-login-callback")]
        public async Task<IActionResult> GoogleCallback(string code)
        {
            // Use empty user ID initially -- need token before knowing email address to use
            var token = await flow.ExchangeCodeForTokenAsync("", code, _googleAuthOptions.RedirectUrl, CancellationToken.None).ConfigureAwait(false);

            var oidcOptions = new OpenIdConnectOptions()
            {
                MetadataAddress = "https://accounts.google.com/.well-known/openid-configuration",
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidAudience = _googleAuthOptions.ClientId
                }
            };
            new OpenIdConnectPostConfigureOptions(new EphemeralDataProtectionProvider()).PostConfigure("google", oidcOptions);
            var config = await oidcOptions.ConfigurationManager.GetConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
            
            var validationParameters = oidcOptions.TokenValidationParameters.Clone();
            var issuer = new[] { config.Issuer };
            validationParameters.ValidIssuers = validationParameters.ValidIssuers?.Concat(issuer) ?? issuer;
            validationParameters.IssuerSigningKeys = validationParameters.IssuerSigningKeys?.Concat(config.SigningKeys)
                ?? config.SigningKeys;
            var parsedClaimsPrincipal = oidcOptions.SecurityTokenValidator.ValidateToken(token.IdToken, validationParameters, out _);
            var claims = parsedClaimsPrincipal.Claims;
            
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, parsedClaimsPrincipal.Identity.AuthenticationType, ClaimTypes.Email, ""));

            await _dataStore.StoreAsync(principal.Identity.Name, token).ConfigureAwait(false);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            return Ok();
        }
    }
}
