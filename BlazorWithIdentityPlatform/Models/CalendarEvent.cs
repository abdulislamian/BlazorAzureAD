using BlazorCalendar.Models;
using static BlazorWithIdentityPlatform.Components.Pages.ListEvents;

namespace BlazorWithIdentityPlatform.Models
{
    public class CalendarEvent
    {
        public string Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public string ChangeKey { get; set; }
        public string OriginalStartTimeZone { get; set; }
        public string OriginalEndTimeZone { get; set; }
        public string Subject { get; set; }
        public bool IsAllDay { get; set; }
        public object OccurrenceId { get; set; }
        public DateTimeTimeZone Start { get; set; }
        public DateTimeTimeZone End { get; set; }
        public Location Location { get; set; }
        public List<object> Locations { get; set; }
        //public Recurrence Recurrence { get; set; }
    }
    public class Start
    {
        public DateTime DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class End
    {
        public DateTime DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    //public class Location
    //{
    //    public string DisplayName { get; set; }
    //}

    public class Organizer
    {
        public EmailAddress EmailAddress { get; set; }
    }

    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
