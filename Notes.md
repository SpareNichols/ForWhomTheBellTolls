
# Feature Ideas

- Synchronous/Asynchronous
- Link To Virtual Classroom
- Push notifications to Android/iOS/Windows10
- Google Calendar Add-in
- Google Assistant announcement bell

# Models

ScheduleUnit
    Synchronous (class-participation) or Asynchronous (individually-driven)
    TimeSpan
    Date
    Name
    Description
    URL

# Google Assistant

- Issues
  -  Apparently no API for configuring Family Bell
  -  Push notifications to smart speakers not supported outside of select applications (e.g. smart doorbells)
  -  Assistant notifications has limitations (max 10/user/day)

# Google Calendar

- Limit of 1,000,000 API requests - limit of 60 calendars
- Decide whether to create on someone's calendar or own the calendar
- Recurrence using RFC5545 iCal spec - .NET library https://github.com/rianjs/ical.net
- Reminders on calendar event
  - email or popup

# Required Configuration (processes that should be automated eventually)
1. Set up project in Google API Console
   1. (mine is ForWhomTheBellTolls-Dev)
2. Enable Google Calendar API in API Library
3. Configure OAuth Consent Screen
   1. User Type: External
   2. App Name: For Whom The Bell Tolls - Dev
   3. User support email: acct owner
   4. Add .../auth/calendar.app.created scope (Make secondary Google calendars, and see, create, change, and delete events on them)
4. Create OAuth client ID credentials
   1. App type - web application
   2. Name - ForWhomTheBellTolls-DotNetApi-Dev
   3. Authorized origins and redirect URIs - https://localhost:23023
      1. CLIENT ID - 966590036234-59pf9s1ms5ncecran0h43ns75q2r79di.apps.googleusercontent.com
      2. CLIENT SECRET - h1IctSXkJaso5yyzt9Gu7bcz