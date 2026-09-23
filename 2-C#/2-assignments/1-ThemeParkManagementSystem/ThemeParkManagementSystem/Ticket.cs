namespace ThemeParkManagementSystem
{
    public enum TicketStatus
    {
        valid,
        invalid,
        expired,
        cancelled
    }
    public class Ticket
    {
        public int Id { get; set; }
        public string RideName { get; set; }
        public DateOnly ExpireDate { get; set; }
        public Tier Tier { get; set; }
        public int Price { get; set; }
        public TicketStatus Status { get; set; }

        internal static int Count { get; set; }

        public Ticket(int id, string rideName, DateOnly expireDate, Tier tier, int price, TicketStatus status)
        {
            Id = id;
            RideName = rideName;
            ExpireDate = expireDate;
            Tier = tier;
            Price = price;
            Status = status;
            ParkSystem.Add(this);
            Count++;
        }
    }
}
