using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirportGroundOperationsManagementSystem
{
    public class AirportSystem
    {
        private static List<GroundStaff> GroundStaffs { get; set; }
        private static List<Flight> Flights { get; set; }
        private static List<Gate> Gates { get; set; }
        private static List<Passenger> Passengers { get; set; }
        private static List<Baggage> Baggages { get; set; }

        static AirportSystem()
        {
            GroundStaffs = new List<GroundStaff>();
            Flights = new List<Flight>();
            Gates = new List<Gate>();
            Passengers = new List<Passenger>();
            Baggages = new List<Baggage>();
        }

        // -------------------------------- Create & Get Objects -------------------------------- //
        internal static void Add<T>(T entity)
        {
            string type = entity.GetType().Name;
            bool ignoreCreationMessage = false;

            if (entity is GroundStaff e)
            {
                GroundStaffs.Add(e);
                ignoreCreationMessage = e.Position == Position.admin;
            }

            else if (entity is Flight f)
                Flights.Add(f);
            else if (entity is Gate g)
                Gates.Add(g);
            else if (entity is Passenger p)
            {
                Passengers.Add(p);
                ignoreCreationMessage = true;
            }
            else if (entity is Baggage b)
                Baggages.Add(b);
            else
                throw new Exception($"\nError: Could not add {type}");

            if (!ignoreCreationMessage)
                Console.Write($"\nSuccess: {type} has been added to the system.\n");
        }

        internal static Flight GetFlightById(int id) => Flights.FirstOrDefault(f => f.Id == id);
        internal static Gate GetGateById(int id) => Gates.FirstOrDefault(g => g.Id == id);
        internal static Passenger GetPassengerById(int id) => Passengers.FirstOrDefault(p => p.Id == id);
        internal static GroundStaff GetStaffById(int id) => GroundStaffs.FirstOrDefault(s => s.Id == id);

        // -----------------------------------  ----------------------------------- //
        internal static void CheckIfTimeIsValid(TimeOnly time)
        {
            if (time.Minute % 15 != 0)
                throw new Exception($"\nFailed: Flight times in minutes should only be set in quarters (:00, :15, :30, :45)");
        }
        internal static void ValidateTimeDifference(DateOnly date, TimeOnly departureTime, TimeOnly arrivalTime, TimeSpan duration)
        {

            if (arrivalTime < departureTime)
                throw new Exception("\nFailed, Cannot set the arrival time before the departure time.");
            if (arrivalTime - departureTime != duration)
                throw new Exception("\nFailed, the time differences between the departure time and the arrival time does not match the duration");
        }
        internal static void IsThereAGate()
        {
            if (Gates.Count == 0)
                throw new Exception($"\nFailed, no gates have been registered yet in the system to assign the flight to it.");
        }
        internal static string ListAvailableGateBasedOnSchedule(DateOnly scheduledDate, TimeOnly scheduledTime)
        {
            string availableGates = "";
            bool isThereAnAvailableGate = false;
            foreach (var gate in Gates)
            {
                if (IsGateAvailable(gate.BookedTimes, scheduledDate, scheduledTime))
                {
                    availableGates += $"- {gate.Id} ({(gate.SupportsInternationalFlights ? "Supports" : "Does not Support")} International Flights)\n";
                    isThereAnAvailableGate = true;
                }
            }

            if (!isThereAnAvailableGate)
                throw new Exception($"\nThere are no available gates for this flight on {scheduledDate} at {scheduledTime}, you will need to change the flight schedule or register a new gate.");

            return availableGates;
        }
        internal static bool IsGateAvailable(List<DateTime> timesScheduled, DateOnly date, TimeOnly time)
        {
            if (timesScheduled == null || timesScheduled.Count == 0)
                return true;

            return !timesScheduled.Any(schedule => date == DateOnly.FromDateTime(schedule) && time > TimeOnly.FromDateTime(schedule).AddMinutes(-45) && time < TimeOnly.FromDateTime(schedule).AddMinutes(45)); // 45m so that the gave have time to register the prev or next flight
        }

        internal static void IsStaffAvailable(GroundStaff staff, DateOnly scheduledDate, TimeOnly scheduledTime)
        {
            TimeOnly newStart, newEnd;
            if (staff.Position == Position.baggageStaff)
            {
                newStart = scheduledTime.AddMinutes(-90);
                newEnd = scheduledTime;
            }
            else
            {
                newStart = scheduledTime.AddMinutes(-45);
                newEnd = scheduledTime.AddMinutes(45);
            }


            WorkingHoursPerDay staffWorkingDay = staff.WorkingHoursPerDays.FirstOrDefault(w => w.Date == scheduledDate);
            if (staffWorkingDay != null)
            {
                TimeSpan totalWorkingHoursIfAssigned = staffWorkingDay.WorkedDutyHours + TimeSpan.Parse("01:30");
                if (staffWorkingDay.Date == scheduledDate && totalWorkingHoursIfAssigned > TimeSpan.Parse("9:00"))
                    throw new Exception("\nFailed! Cant assign staff, as it will exceed his 9 hours shift.");
            }

            foreach (var flight in staff.Flights)
            {
                if (flight.DepartureDate == scheduledDate)
                {
                    TimeOnly existingStart, existingEnd;
                    if (staff.Position == Position.baggageStaff)
                    {
                        existingStart = flight.DepartureTime.AddMinutes(-90);
                        existingEnd = flight.DepartureTime;
                    }
                    else
                    {
                        existingStart = flight.DepartureTime.AddMinutes(-45);
                        existingEnd = flight.DepartureTime.AddMinutes(45);
                    }

                    if (newStart < existingEnd && newEnd > existingStart)
                    {
                        throw new Exception("\nFailed! The staff is already assigned to another flight at that time.");
                    }
                }
            }
        }

        // ------------------------------------- View Data ------------------------------------- //
        internal static void ListGates()
        {
            bool hasItems = Gates.Any(item => item != null);
            if (hasItems)
            {
                Console.WriteLine("Gate ID    Supports Intl.  Assigned Flights  Booked Times");
                Gates.ForEach(g => Console.WriteLine($"  {g.Id}            {g.SupportsInternationalFlights}               {g.AssignedFlights.Count}              {g.BookedTimes.Count}"));
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total: {Gates.Count}");
            }
            else
                Console.WriteLine("\nNo gates have been registered yet.");
        }
        internal static void ListGateReservedDurations(Gate gate)
        {
            bool hasItems = gate.AssignedFlights.Any(item => item != null);
            if (hasItems)
            {
                Console.WriteLine("Flight ID       Date        Booked From     Booked to ");
                for (int i = 0; i < gate.AssignedFlights.Count; i++)
                    Console.WriteLine($"  {gate.AssignedFlights[i].Id}         {gate.AssignedFlights[i].DepartureDate}        {TimeOnly.FromDateTime(gate.BookedTimes[i].AddMinutes(-30))}           {TimeOnly.FromDateTime(gate.BookedTimes[i].AddMinutes(15))}");
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total: {Gates.Count}");
            }
            else
                Console.WriteLine("\nThe gate has no reserved flights yet.");
        }
        internal static void ListFlights()
        {
            bool hasItems = Flights.Any(item => item != null);
            if (hasItems)
            {
                Flights.ForEach(f =>
                {
                    if (TimeOnly.FromDateTime(DateTime.Now) >= f.DepartureTime)
                        f.Status = FlightStatus.departed;
                    else if (TimeOnly.FromDateTime(DateTime.Now) > f.DepartureTime.AddMinutes(-30))
                        f.Status = FlightStatus.boarding;
                });

                Console.WriteLine("Flight ID      From       To        Gate      Status          Departure Date        Landing Date       Departure Time      Landing Time       No.Passengers   No.Standby List");
                Flights.ForEach(f => Console.WriteLine($"  {f.Id}           {f.DepartureFrom}           {f.DestinationTo}           {f.Gate.Id}        {f.Status}     {f.DepartureDate}     {f.LandingDate}     {f.DepartureTime}   {f.LandingTime}        {f.Passengers.Count}             {f.StandbyList.Passengers.Count}"));
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total: {Flights.Count}");
            }
            else
                Console.WriteLine("\nNo flights have been registered yet.");
        }
        internal static void ListPassengersPerFlight(List<Passenger> passengers)
        {
            bool hasItems = passengers.Any(item => item != null);
            if (hasItems)
            {
                Console.WriteLine("Passenger ID    Name        Type       Total BaggagesWeights       Total BaggagesWeights Weight ");
                passengers.ForEach(p => Console.WriteLine($"    {p.Id}           {p.Name}        {p.Type}            {p.BaggagesWeights.Count}                         {p.BaggagesWeights.Sum()}")); //{p.BaggagesWeights.Select(b => b.Weight).ToList().Sum()}
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total: {passengers.Count}");
            }
            else
                Console.WriteLine("\nNo passengers have been added to the standby list yet.");
        }
        internal static void ListPassengerFlights(Passenger passenger)
        {
            bool hasItems = passenger.Flights.Any(item => item != null);
            if (hasItems)
            {
                Console.WriteLine("Flight ID     From       To        Status     Departure Date   Gate");
                passenger.Flights.ForEach(f => Console.WriteLine($"  {f.Id}           {f.DepartureFrom}        {f.DestinationTo}      {f.Status}        {f.DepartureDate}            {f.Gate.Id}"));
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total: {passenger.Flights.Count}");
            }
            else
                Console.WriteLine("\nNo flights have been registered yet.");
        }
        internal static void ListPassengers()
        {
            bool hasItems = Passengers.Any(item => item != null);
            if (hasItems)
            {
                Console.WriteLine("Passenger ID    Name      Type         Total BaggagesWeights         Total BaggagesWeights Weight         Booked Flights");
                Passengers.ForEach(p => Console.WriteLine($"    {p.Id}           {p.Name}         {p.Type}              {p.BaggagesWeights.Count}                         {p.BaggagesWeights.Sum()}                         {p.Flights.Count} ")); //{p.BaggagesWeights.Select(b => b.Weight).ToList().Sum()}
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total: {Passengers.Count}");
            }
            else
                Console.WriteLine("\nNo passengers have been registered yet.");
        }
        internal static void ListAFlightStandbyList(List<Passenger> passengers)
        {
            bool hasItems = passengers.Any(item => item != null);
            if (hasItems)
            {
                Console.WriteLine("Passenger ID   Name       Type         Total BaggagesWeights");
                passengers.ForEach(p => Console.WriteLine($"  {p.Id}             {p.Name}       {p.Type}              {p.BaggagesWeights.Count}"));
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total standby passengers: {passengers.Count}");
            }
            else
                Console.WriteLine("\nThe standby list is empty.");
        }
        internal static void ListStaffWorkingHours(GroundStaff staff)
        {
            bool hasItems = staff.WorkingHoursPerDays.Any(item => item != null);
            if (hasItems)
            {
                Console.WriteLine("  Date         Total Work Hours ");
                staff.WorkingHoursPerDays.ForEach(wh => Console.WriteLine($"{wh.Date}              {wh.WorkedDutyHours.TotalHours}h"));
                Console.WriteLine($"--------------------------------------------------------");
            }
            else
                Console.WriteLine("\nThe staff did not get any assignment yet.");
        }
        internal static void ListGroundStaff()
        {
            bool hasItems = GroundStaffs.Any(item => item != null && item.Id != 0);
            if (hasItems)
            {
                Console.WriteLine("Staff ID       Position          Flights Assigned   Total Hours Worked");
                GroundStaffs.ForEach(s =>
                {
                    if (s.Id != 0)
                        Console.WriteLine($"  {s.Id}        {s.Position}                 {s.Flights.Count}                 {s.WorkingHoursPerDays.Sum(w => (w.WorkedDutyHours).TotalHours)}h");
                }
                );
                Console.WriteLine($"--------------------------------------------------------");
                Console.WriteLine($"Total: {GroundStaffs.Count}");
            }
            else
                Console.WriteLine("\nNo staff has been hired yet.");
        }
    }
}
