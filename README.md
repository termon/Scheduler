
DotNet 6 FullCalendar Scheduler
=====================================

This .NET project is based on the DotNetTemplate [https://github.com/termon/DotNetTemplate] and demonstrates use of FullCalendar [https://fullcalendar.io] in an MVC application. Management of calendar events is handled by the MVC application and full calendar is simply used to render the calendar events.

The ```_Calendar.cshtml``` partial view is responsible for rendering the calendar component. The ```Add.cshtml``` and ```Edit.cshtml``` views display a calendar with a bootstrap 5 modal dialog to manage the event data.

A remote validator is used to verify events do not overlap and provide in modal event validation feedback.
