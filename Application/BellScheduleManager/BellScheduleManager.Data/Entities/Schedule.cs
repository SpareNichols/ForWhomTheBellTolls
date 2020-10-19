using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BellScheduleManager.Data.Entities
{
    [Table("Schedule", Schema = "dbo")]
    public class Schedule
    {
        [Key]
        public Guid ScheduleId { get; set; }
        public string OwningUser { get; set; }
        public string ScheduleName { get; set; }
        public string GoogleCalendarId { get; set; }
        public virtual List<ScheduleRule> ScheduleRules { get; set; }
    }
}
