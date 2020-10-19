using BellScheduleManager.Common.Enumerations;
using BellScheduleManager.Resources.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BellScheduleManager.Resources.Helpers
{
    public static class ScheduleRepresentationHelper
    {
        public static string GenerateRrule(ScheduleRuleType scheduleRuleType, DaysOfWeek daysOfWeek, DateTime until)
        {
            var rrule = new StringBuilder();

            rrule.Append("RRULE:");

            if (scheduleRuleType == ScheduleRuleType.ByDayOfWeek
                || scheduleRuleType == ScheduleRuleType.ByDayOfWeekEveryOtherWeek)
            {
                rrule.Append("FREQ=WEEKLY;");
            }
            else
            {
                throw new Exception($"Unable to parse ScheduleRuleType {scheduleRuleType} to RRULE");
            }

            if (scheduleRuleType == ScheduleRuleType.ByDayOfWeekEveryOtherWeek)
            {
                rrule.Append("INTERVAL=2;");
            }

            var byDayString = ConvertDaysOfWeekToRRULEString(daysOfWeek);

            rrule.Append($"BYDAY={byDayString};");

            rrule.Append($"UNTIL={until.ToString("yyyyMMdd")}");

            return rrule.ToString();
        }

        public static TimeSpan ConvertTimeStringToTimeSpan(string timeString)
        {
            var time = DateTime.Parse(timeString);
            return time.TimeOfDay;
        }

        public static DaysOfWeek ConvertStringToDaysOfWeekFlags(string s)
        {
            var daysOfWeek = new DaysOfWeek();

            if (s.Contains("Sunday", StringComparison.InvariantCultureIgnoreCase))
                daysOfWeek = daysOfWeek | DaysOfWeek.Sunday;
            if (s.Contains("Monday", StringComparison.InvariantCultureIgnoreCase))
                daysOfWeek = daysOfWeek | DaysOfWeek.Monday;
            if (s.Contains("Tuesday", StringComparison.InvariantCultureIgnoreCase))
                daysOfWeek = daysOfWeek | DaysOfWeek.Tuesday;
            if (s.Contains("Wednesday", StringComparison.InvariantCultureIgnoreCase))
                daysOfWeek = daysOfWeek | DaysOfWeek.Wednesday;
            if (s.Contains("Thursday", StringComparison.InvariantCultureIgnoreCase))
                daysOfWeek = daysOfWeek | DaysOfWeek.Thursday;
            if (s.Contains("Friday", StringComparison.InvariantCultureIgnoreCase))
                daysOfWeek = daysOfWeek | DaysOfWeek.Friday;
            if (s.Contains("Saturday", StringComparison.InvariantCultureIgnoreCase))
                daysOfWeek = daysOfWeek | DaysOfWeek.Saturday;

            return daysOfWeek;
        }

        public static string ConvertDaysOfWeekToRRULEString(DaysOfWeek daysOfWeek)
        {
            var listOfDays = GetListOfDayOfWeekFromFlags(daysOfWeek);
            var listOfRRULEFormatted = listOfDays.Select(d =>
            {
                switch (d)
                {
                    case DayOfWeek.Sunday:
                        return "SU";
                    case DayOfWeek.Monday:
                        return "MO";
                    case DayOfWeek.Tuesday:
                        return "TU";
                    case DayOfWeek.Wednesday:
                        return "WE";
                    case DayOfWeek.Thursday:
                        return "TH";
                    case DayOfWeek.Friday:
                        return "FR";
                    case DayOfWeek.Saturday:
                        return "SA";
                    default:
                        return "UNKNOWN";
                }
            });

            return string.Join(",", listOfRRULEFormatted);
        }

        public static IEnumerable<DayOfWeek> GetListOfDayOfWeekFromFlags(DaysOfWeek daysOfWeek)
        {
            if (daysOfWeek.HasFlag(DaysOfWeek.Sunday)) yield return DayOfWeek.Sunday;
            if (daysOfWeek.HasFlag(DaysOfWeek.Monday)) yield return DayOfWeek.Monday;
            if (daysOfWeek.HasFlag(DaysOfWeek.Tuesday)) yield return DayOfWeek.Tuesday;
            if (daysOfWeek.HasFlag(DaysOfWeek.Wednesday)) yield return DayOfWeek.Wednesday;
            if (daysOfWeek.HasFlag(DaysOfWeek.Thursday)) yield return DayOfWeek.Thursday;
            if (daysOfWeek.HasFlag(DaysOfWeek.Friday)) yield return DayOfWeek.Friday;
            if (daysOfWeek.HasFlag(DaysOfWeek.Saturday)) yield return DayOfWeek.Saturday;
        }
    }
}
