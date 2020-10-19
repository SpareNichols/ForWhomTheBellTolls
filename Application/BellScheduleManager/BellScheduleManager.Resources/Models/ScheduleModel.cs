using BellScheduleManager.Resources.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BellScheduleManager.Resources.Models
{
    public class ScheduleModel
    {
        public Guid ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public string GoogleCalendarId { get; set; }
        public List<ScheduleRuleModel> ScheduleRules { get; set; }
        public List<ScheduleEventModel> TodaysEvents { get; set; }
    }

    public class ScheduleEventModel
    {
        public DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public string Name { get; set; }
    }
}
