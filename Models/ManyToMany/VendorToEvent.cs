namespace Infinterest.Models
{
    public class VendorToEvent
    {
        public int VendorToEventId {get;set;}
        public bool Confirmed {get;set;}
        public int VendorId {get;set;}
        public Vendor Vendor {get;set;}
        public int EventId {get;set;}
        public Event Event {get;set;}

        public VendorToEvent ()
        {
            Confirmed = false;
        }
        public VendorToEvent (Vendor userInput, Event eventInput)
        {
            Confirmed = false;
            Vendor = userInput;
            VendorId = userInput.UserId;
            Event = eventInput;
            EventId = eventInput.EventId;
        }
    }
}