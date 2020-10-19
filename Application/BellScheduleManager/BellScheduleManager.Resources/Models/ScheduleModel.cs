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
    }
}
