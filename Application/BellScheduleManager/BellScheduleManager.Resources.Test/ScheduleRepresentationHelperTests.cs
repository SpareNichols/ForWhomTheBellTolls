using BellScheduleManager.Common.Enumerations;
using BellScheduleManager.Resources.Helpers;
using BellScheduleManager.Resources.Services;
using Google.Apis.Calendar.v3.Data;
using System;
using Xunit;

namespace BellScheduleManager.Resources.Test
{
    public class ScheduleRepresentationHelperTests
    {
        [Theory]
        [InlineData(DaysOfWeek.Sunday | DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday | DaysOfWeek.Saturday, "FREQ=WEEKLY;BYDAY=SU,MO,TU,WE,TH,FR,SA")]
        [InlineData(DaysOfWeek.Sunday | DaysOfWeek.Monday, "FREQ=WEEKLY;BYDAY=SU,MO")]
        public void ConvertToRRULE_WhenRecurrenceTypeByDayOfWeek_ProducesValidRRULE(DaysOfWeek daysOfWeek, string expectedResult)
        {
            // Arrange
            var model = new ScheduleRuleModel()
            {
                ScheduleRuleType = ScheduleRuleType.ByDayOfWeek,
                DaysOfWeek = daysOfWeek
            };

            // Act
            var rrule = ScheduleRepresentationHelper.GenerateRrule(ScheduleRuleType.ByDayOfWeek,);

            // Assert
            Assert.Equal(expectedResult, rrule);
        }

        [Theory]
        [InlineData(DaysOfWeek.Sunday | DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday | DaysOfWeek.Friday | DaysOfWeek.Saturday, "FREQ=WEEKLY;INTERVAL=2;BYDAY=SU,MO,TU,WE,TH,FR,SA")]
        [InlineData(DaysOfWeek.Sunday | DaysOfWeek.Monday, "FREQ=WEEKLY;INTERVAL=2;BYDAY=SU,MO")]
        public void ConvertToRRULE_WhenRecurrenceTypeByDayOfWeekEveryOtherWeek_ProducesValidRRULE(DaysOfWeek daysOfWeek, string expectedResult)
        {
            // Arrange
            var model = new ScheduleRuleModel()
            {
                ScheduleRuleType = ScheduleRuleType.ByDayOfWeekEveryOtherWeek,
                DaysOfWeek = daysOfWeek
            };

            // Act
            var rrule = ScheduleRepresentationHelper.ConvertToRRULE(model);

            // Assert
            Assert.Equal(expectedResult, rrule);
        }

        [Theory]
        [InlineData("1:00-2:00", 1, 0, 2, 0)]
        [InlineData("1:20 PM - 2:10 PM", 13, 20, 14, 10)]
        [InlineData("11AM-1PM", 11, 0, 13, 0)]
        public void ConvertTimeStringToStartEndTimes_WhenValidTimeString_ShouldReturnValidTimeSpans(string input, int expectedStartHour, int expectedStartMinute, int expectedEndHour, int expectedEndMinute)
        {
            var timeSpans = ScheduleRepresentationHelper.ConvertTimeStringToStartEndTimes(input);

            Assert.Equal(expectedStartHour, timeSpans.Item1.Hours);
            Assert.Equal(expectedStartMinute, timeSpans.Item1.Minutes);
            Assert.Equal(expectedEndHour, timeSpans.Item2.Hours);
            Assert.Equal(expectedEndMinute, timeSpans.Item2.Minutes);
        }
    }
}
