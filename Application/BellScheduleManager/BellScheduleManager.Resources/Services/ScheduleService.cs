using BellScheduleManager.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BellScheduleManager.Common.Enumerations;
using BellScheduleManager.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using BellScheduleManager.Common.Options;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Microsoft.Extensions.Options;
using BellScheduleManager.Resources.Helpers;
using TimeZoneConverter;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.IO;

namespace BellScheduleManager.Resources.Services
{
    public interface IScheduleService
    {
        Task<List<ScheduleModel>> GetSchedulesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ScheduleOccurrenceInstance>> GetScheduledInstancesAsync(DateTime date, CancellationToken cancellationToken = default(CancellationToken));
        Task CreateCalendarForScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default(CancellationToken));
        Task CreateScheduleFromFileAsync(string name, Stream fileStream);
        Task DeleteScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class ScheduleService : IScheduleService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
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

        public ScheduleService(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IDataStore dataStore, IOptions<GoogleAuthOptions> googleAuthOptions)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _dataStore = dataStore;
            _googleAuthOptions = googleAuthOptions.Value;
        }

        public async Task<List<ScheduleModel>> GetSchedulesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            var models = await _appDbContext.Schedules.Where(s => s.OwningUser == _httpContextAccessor.HttpContext.User.Identity.Name)
                .Select(s => new ScheduleModel()
                {
                    ScheduleId = s.ScheduleId,
                    ScheduleName = s.ScheduleName,
                    GoogleCalendarId = s.GoogleCalendarId,
                    ScheduleRules = s.ScheduleRules.Select(sr => new ScheduleRuleModel
                    {
                        Name = sr.Name,
                        ScheduleRuleType = sr.ScheduleRuleType,
                        DaysOfWeek = sr.DaysOfWeek,
                        StartDate = sr.StartDate,
                        EndDate = sr.EndDate,
                        StartTime = sr.StartTime,
                        EndTime = sr.EndTime
                    }).ToList()
                })
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return models;
        }

        public async Task<List<ScheduleOccurrenceInstance>> GetScheduledInstancesAsync(DateTime date, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new List<ScheduleOccurrenceInstance>
            {

            };
        }

        public async Task CreateCalendarForScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var schedule = await _appDbContext.Schedules
                .Include(s => s.ScheduleRules)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId, cancellationToken).ConfigureAwait(false);
            if (schedule == null)
            {
                throw new Exception("Unable to find schedule with given ID");
            }

            var token = await flow.LoadTokenAsync(_httpContextAccessor.HttpContext.User.Identity.Name, cancellationToken).ConfigureAwait(false);
            var userCredentials = new UserCredential(flow, _httpContextAccessor.HttpContext.User.Identity.Name, token);

            var service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = userCredentials
            });

            var calendars = await service.CalendarList.List().ExecuteAsync(cancellationToken).ConfigureAwait(false);
            string userCalendarId;

            if (calendars.Items.Count == 0 || !calendars.Items.Any(i => i.Description == schedule.ScheduleId.ToString()))
            {
                var cal = await service.Calendars.Insert(new Calendar
                {
                    Summary = $"FWTBT - {schedule.ScheduleName}",
                    Description = schedule.ScheduleId.ToString()
                }).ExecuteAsync().ConfigureAwait(false);
                userCalendarId = cal.Id;
            }
            else
            {
                userCalendarId = calendars.Items.First(i => i.Description == schedule.ScheduleId.ToString()).Id;
            }

            schedule.GoogleCalendarId = userCalendarId;

            foreach (var rule in schedule.ScheduleRules)
            {
                var rrule = ScheduleRepresentationHelper.GenerateRrule(rule.ScheduleRuleType, rule.DaysOfWeek, rule.EndDate.Date + rule.EndTime);

                service.Events.Insert(new Event
                {
                    Summary = rule.Name,
                    Location = rule.Url,
                    Start = new EventDateTime { 
                        DateTime = rule.StartDate.Date + rule.StartTime,
                        TimeZone = TZConvert.WindowsToIana(TimeZoneInfo.Local.StandardName)
                    },
                    End = new EventDateTime
                    {
                        DateTime = rule.StartDate.Date + rule.EndTime,
                        TimeZone = TZConvert.WindowsToIana(TimeZoneInfo.Local.StandardName)
                    },
                    Recurrence = new List<string>
                    {
                        rrule
                    }
                }, userCalendarId).Execute();

            }

            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default)
        {
            var scheduleToRemove = await _appDbContext.Schedules
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId && s.OwningUser == _httpContextAccessor.HttpContext.User.Identity.Name, cancellationToken).ConfigureAwait(false);

            if (scheduleToRemove != null)
            {
                _appDbContext.Remove(scheduleToRemove);
            }

            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class ScheduleRuleModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public ScheduleRuleType ScheduleRuleType { get; set; }
        public DaysOfWeek DaysOfWeek { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string ReadableDescription
        {
            get
            {
                var str = new StringBuilder();
                if (ScheduleRuleType == ScheduleRuleType.ByDayOfWeek)
                {
                    str.Append("Every week");
                }
                else if (ScheduleRuleType == ScheduleRuleType.ByDayOfWeekEveryOtherWeek)
                {
                    str.Append("Every other week");
                }
                else
                {
                    str.Append("Unknown schedule type");
                }

                str.Append(" on ");

                str.Append(string.Join(",", ScheduleRepresentationHelper.GetListOfDayOfWeekFromFlags(DaysOfWeek).Select(e => e.ToString())));

                str.Append(" from ");
                str.Append(new DateTime(1, 1, 1, StartTime.Hours, StartTime.Minutes, StartTime.Seconds).ToShortTimeString());

                str.Append(" to ");
                str.Append(new DateTime(1, 1, 1, EndTime.Hours, EndTime.Minutes, EndTime.Seconds).ToShortTimeString());

                return str.ToString();
            }
        }
    }

    public class ScheduleOccurrenceInstance
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class ScheduleFileRow
    {
        public string Name { get; set; }
        public string Time { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DaysOfWeek { get; set; }
        public string Url { get; set; }
    }
}
