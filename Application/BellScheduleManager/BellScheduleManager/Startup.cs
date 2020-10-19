using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellScheduleManager.Common.Options;
using BellScheduleManager.Data;
using BellScheduleManager.Resources.Helpers;
using BellScheduleManager.Resources.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BellScheduleManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            ConfigureOptions(services);

            // Services
            services.AddScoped<IDataStore, InMemoryDataStore>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddHttpContextAccessor();

            // DbContext
            var dbOptions = new DatabaseOptions();
            Configuration.Bind("DatabaseOptions", dbOptions);
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(dbOptions.ConnectionString);
            });

            // ASP.NET Core Infrastructure
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddControllers();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<GoogleAuthOptions>(Configuration.GetSection("GoogleAuthOptions"));
            services.Configure<DatabaseOptions>(Configuration.GetSection("DatabaseOptions"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
