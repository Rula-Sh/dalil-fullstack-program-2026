namespace AirportGroundOperationsManagementSystem
{
    public class Baggage
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public Baggage(int id, double weight)
        {
            Id = id;
            Weight = weight;
            AirportSystem.Add(this);
        }
    }
}
