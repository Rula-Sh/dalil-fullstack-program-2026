namespace AirportGroundOperationsManagementSystem
{
    public class StandbyList
    {
        public Flight Flight { get; set; }

        public List<Passenger> Passengers { get; set; }

        public StandbyList(Flight flight)
        {
            Flight = flight;
            Passengers = new List<Passenger>();
        }
    }
}
