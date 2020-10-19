INSERT INTO dbo.Schedule
	(ScheduleName, OwningUser)
VALUES
	('Michael''s schedule', 'senseofnickels@gmail.com')

DECLARE @ScheduleId uniqueidentifier = (SELECT TOP 1 ScheduleId FROM dbo.Schedule WHERE ScheduleName = 'Michael''s schedule')

INSERT INTO dbo.ScheduleRule
	([Name], [Url], ScheduleRuleType, DaysOfWeek, StartDate, EndDate, StartTime, EndTime, ScheduleId)
VALUES
	('FirstPeriod - Music', NULL, 'ByDayOfWeek', 42, '2020-09-01', '2020-12-12', '8:00', '9:20', @ScheduleId) -- MWF
	,('SecondPeriod - Social Studies', 'https://www.google.com', 'ByDayOfWeek', 42, '2020-09-01', '2020-12-12', '9:40', '11:20', @ScheduleId) -- MWF
	,('ThirdPeriod - Language Arts', NULL, 'ByDayOfWeek', 42, '2020-09-01', '2020-12-12', '11:40', '14:10', @ScheduleId) -- MWF
	,('FirstPeriod - Math', NULL, 'ByDayOfWeek', 20, '2020-09-01', '2020-12-12', '8:00', '9:20', @ScheduleId) -- MWF
	,('SecondPeriod - Science', 'https://www.google.com', 'ByDayOfWeek', 20, '2020-09-01', '2020-12-12', '9:40', '11:20', @ScheduleId) -- MWF
	,('ThirdPeriod - Snoozing', NULL, 'ByDayOfWeek', 20, '2020-09-01', '2020-12-12', '11:40', '14:10', @ScheduleId) -- MWF
