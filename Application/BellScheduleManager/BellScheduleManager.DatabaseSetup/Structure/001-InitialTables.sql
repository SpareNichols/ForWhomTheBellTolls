CREATE TABLE [dbo].[Schedule](
	[ScheduleId] [uniqueidentifier] NOT NULL,
	[OwningUser] [nvarchar](50) NOT NULL,
	[ScheduleName] [nvarchar](50) NOT NULL,
	[GoogleCalendarId] [nvarchar](200) NULL,
 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED 
(
	[ScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Schedule] ADD  CONSTRAINT [DF_Schedule_ScheduleId]  DEFAULT (newid()) FOR [ScheduleId]
GO



CREATE TABLE [dbo].[ScheduleRule](
	[ScheduleRuleId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](500) NULL,
	[ScheduleRuleType] [nvarchar](50) NOT NULL,
	[DaysOfWeek] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[ScheduleId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ScheduleRule] PRIMARY KEY CLUSTERED 
(
	[ScheduleRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScheduleRule] ADD  CONSTRAINT [DF_ScheduleRule_ScheduleRuleId]  DEFAULT (newid()) FOR [ScheduleRuleId]
GO

ALTER TABLE [dbo].[ScheduleRule]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleRule_Schedule] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[Schedule] ([ScheduleId]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ScheduleRule] CHECK CONSTRAINT [FK_ScheduleRule_Schedule]
GO
