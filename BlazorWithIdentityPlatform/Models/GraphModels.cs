using BlazorWithIdentityPlatform.Components.Pages;
using BlazorWithIdentityPlatform.Models;
using System; 

namespace BlazorCalendar.Models
{
    public class GraphEventsResponse
    {
        public MicrosoftGraphEvent[] Value { get; set; }
    }
    public class MicrosoftGraphEvent
    {
        public string Subject { get; set; }
        public DateTimeTimeZone Start { get; set; }
        public DateTimeTimeZone End { get; set; }
    }

    public class DateTimeTimeZone 
    {
        public string DateTime {get; set;}
        public string TimeZone {get; set;}

        public DateTime ConvertToLocalDateTime()
        {
            var dateTime = System.DateTime.Parse(DateTime);
            var time = TimeZoneInfo.Local.Id;
            TimeZoneInfo timeZone = null; 
            if(TimeZone == "UTC")
                timeZone = TimeZoneInfo.Utc; 
            else
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
            var timeZoneDate = new DateTimeOffset(dateTime, timeZone.BaseUtcOffset).LocalDateTime;
            return timeZoneDate;
        }
    }
}