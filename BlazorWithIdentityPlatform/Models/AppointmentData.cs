//namespace BlazorWithIdentityPlatform.Models
//{
//    public class AppointmentData
//    {
//        //public string EventID { get; set; }
//        //public string EventSubject { get; set; }
//        //public DateTime EventStart { get; set; }
//        //public DateTime EventEnd { get; set; }
//        public string Id { get; set; }
//        public string Subject { get; set; }
//        public string Location { get; set; }
//        public DateTime StartTime { get; set; }
//        public DateTime EndTime { get; set; }
//        public string Description { get; set; }
//        public bool IsAllDay { get; set; }
//        public string RecurrenceRule { get; set; }
//        public string RecurrenceException { get; set; }
//        public Nullable<int> RecurrenceID { get; set; }
//    }
//}
using BlazorCalendar.Models;
using static BlazorWithIdentityPlatform.Components.Pages.ListEvents;

namespace BlazorWithIdentityPlatform.Models
{
    public class AppointmentData
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public Nullable<int> RecurrenceID { get; set; }
    }
    public class Location
    {
        //public string DisplayName { get; set; }
        //public string LocationType { get; set; }
        //public string UniqueId { get; set; }
        //public string UniqueIdType { get; set; }
        public string DisplayName { get; set; }
        public string LocationType { get; set; }
        public string UniqueId { get; set; }
        public string UniqueIdType { get; set; }
        public Address Address { get; set; }
        public Coordinates Coordinates { get; set; }
    }

}

