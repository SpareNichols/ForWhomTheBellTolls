using BellScheduleManager.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellScheduleManager.Data.Entities
{
    [Table("ScheduleRule", Schema = "dbo")]
    public class ScheduleRule
    {
        [Key]
        public Guid ScheduleRuleId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public ScheduleRuleType ScheduleRuleType { get; set; }
        public DaysOfWeek DaysOfWeek { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Guid ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }
    }
}