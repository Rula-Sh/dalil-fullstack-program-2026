using Microsoft.VisualBasic;

namespace AirportGroundOperationsManagementSystem
{
    public class Gate
    {

        public int Id { get; set; }
        public bool SupportsInternationalFlights { get; set; }
        public List<Flight>? AssignedFlights { get; set; }
        public List<DateTime>? BookedTimes { get; set; }

        public Gate(int id, bool supportsInternationalFlights)
        {
            Id = id;
            SupportsInternationalFlights = supportsInternationalFlights;
            AssignedFlights = new List<Flight>();
            BookedTimes = new List<DateTime>();
            AirportSystem.Add(this);
        }
    }
}
