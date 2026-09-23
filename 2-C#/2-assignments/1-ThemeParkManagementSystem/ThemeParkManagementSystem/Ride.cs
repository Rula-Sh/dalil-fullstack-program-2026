namespace ThemeParkManagementSystem
{
    public enum RideType
    {
        thrill,
        family,
        water
    }
    public enum RideStatus
    {
        open,
        closed,
        underMaintenance
    }
    public class Ride
    {
        public string Name { get; set; }
        public int MinAge { get; set; }
        public int MinHeight { get; set; }
        public int MinimumAccompanyingAdult { get; set; }
        public int Capacity { get; set; }
        public int CurrentCapacity { get; set; }
        public RideType Type { get; set; }
        public RideStatus Status { get; set; }
        public Tier Tier { get; set; }
        public int AssignedEmployeeId { get; set; }

        internal static int Count { get; set; }

        public Ride(string name, int minAge, int minHeight, int minimumAccompanyingAdult, int capacity, RideType type, RideStatus status, Tier tier, int assignedEmplyeeId)
        {
            Name = name;
            MinAge = minAge;
            MinHeight = minHeight;
            MinimumAccompanyingAdult = minimumAccompanyingAdult;
            Capacity = capacity;
            Type = type;
            Status = status;
            Tier = tier;
            AssignedEmployeeId = assignedEmplyeeId;
            CurrentCapacity = 0;
            ParkSystem.Add(this);
            Count++;
        }
    }
}
