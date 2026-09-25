namespace AirportGroundOperationsManagementSystem
{
    public enum PassengerType
    {
        standard,
        VIP,
        reducedMobility
    }

    public class Passenger
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<double> BaggagesWeights { get; set; }
        public List<Flight> Flights { get; set; }
        public PassengerType Type { get; set; }
        public bool? AllDocumentsValid { get; set; }

        public Passenger(int id, string name, PassengerType type)
        {
            Id = id;
            Name = name;
            Type = type;
            BaggagesWeights = new List<double>();
            Flights = new List<Flight>();
            AirportSystem.Add(this);
        }
    }
}
