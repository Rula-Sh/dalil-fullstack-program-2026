namespace ThemeParkManagementSystem
{
    public class Reservation  
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public int TicketId { get; set; }
        public string RideName { get; set; }
        public DateOnly Date{ get; set; }
        public TimeOnly TimeSlot { get; set; }

        internal static int Count { get; set; }

        public Reservation(int visitorId, int ticketId, string rideName, DateOnly date, TimeOnly timeSlot)
        {
            Id = int.Parse($"{visitorId}{ticketId}");
            VisitorId = visitorId;
            TicketId = ticketId;
            RideName = rideName;
            Date = date;
            TimeSlot = timeSlot;
            ParkSystem.Add(this);
            Count++;
        }
    }
}
