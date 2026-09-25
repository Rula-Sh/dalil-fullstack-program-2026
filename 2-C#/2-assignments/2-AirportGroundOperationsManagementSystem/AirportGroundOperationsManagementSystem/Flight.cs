namespace AirportGroundOperationsManagementSystem
{
    public enum FlightStatus
    {
        Idle,
        boarding,
        delayed,
        departed,
        cancelled
    }

    public class Flight
    {
        public int Id { get; set; }
        public string DepartureFrom { get; set; }
        public string DestinationTo { get; set; }
        public FlightStatus Status { get; set; }
        public bool IsInternational { get; set; }
        public int SeatCapacity { get; set; }
        public int GroundStaffCount { get; set; }
        public double MaxBaggageWeightPerPassenger { get; set; }
        public DateOnly DepartureDate { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateOnly LandingDate { get; set; }
        public TimeOnly LandingTime { get; set; }
        public Gate Gate { get; set; }
        public StandbyList? StandbyList { get; set; }
        public List<Passenger>? Passengers { get; set; }
        public List<GroundStaff>? GroundStaffs { get; set; }

        public Flight(int id, string departureFrom, string destinationTo, TimeSpan Duration, bool isInternational, int seatCapacity, int groundStaffCount, double maximumBaggageWeightPerPassenger, DateOnly departureDate, TimeOnly departureTime, TimeOnly landingTime, Gate gate)
        {
            Id = id;
            DepartureFrom = departureFrom;
            DestinationTo = destinationTo;
            IsInternational = isInternational;
            SeatCapacity = seatCapacity;
            GroundStaffCount = groundStaffCount;
            MaxBaggageWeightPerPassenger = maximumBaggageWeightPerPassenger;
            DepartureDate = departureDate;
            DepartureTime = departureTime;
            LandingTime = landingTime;

            DateTime landingSchedule = new DateTime(DepartureDate, DepartureTime) + Duration;
            LandingDate = DateOnly.FromDateTime(landingSchedule);
            LandingTime = TimeOnly.FromDateTime(landingSchedule);

            Status = FlightStatus.Idle;
            Gate = gate;
            StandbyList = new StandbyList(this);
            Passengers = new List<Passenger>();
            GroundStaffs = new List<GroundStaff>();
            AirportSystem.Add(this);
        }
    }
}
