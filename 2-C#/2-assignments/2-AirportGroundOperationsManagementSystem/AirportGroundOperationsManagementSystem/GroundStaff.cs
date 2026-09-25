using System.Diagnostics.CodeAnalysis;
using System.Timers;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AirportGroundOperationsManagementSystem
{
    public enum Position
    {
        gateAgent,
        baggageStaff,
        admin
    }
    public class GroundStaff
    {

        public int Id { get; set; }
        public Position Position { get; set; }
        public List<Flight>? Flights { get; set; }
        public List<WorkingHoursPerDay> WorkingHoursPerDays { get; set; }

        public GroundStaff(int id, Position position)
        {
            Id = id;
            Position = position;
            Flights = new List<Flight>();
            WorkingHoursPerDays = new List<WorkingHoursPerDay>();
            AirportSystem.Add(this);
        }

        // -------------------------------------- Flights -------------------------------------- //
        public void RegisterGate()
        {
            try
            {
                Console.Write("Enter gate id: ");
                int id = int.Parse(Console.ReadLine());
                isInputAboveZero(id);
                if (AirportSystem.GetGateById(id) != null)
                    throw new Exception("\nFailed! A gate with te same ID already exist in the system");

                Console.Write("Does it support international flights? Type \"y\" to confirm. ");
                bool supports = Console.ReadLine().ToLower() == "y" ? true : false;

                new Gate(id, supports);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public void RegisterFlight()
        {
            try
            {
                AirportSystem.IsThereAGate();

                Console.Write("Enter Flight id: ");
                int id = int.Parse(Console.ReadLine());
                isInputAboveZero(id);

                if (AirportSystem.GetFlightById(id) != null)
                    throw new Exception("\nFailed! A flight with te same ID already exist in the system");
                else
                {
                    Console.Write("Enter departure location: ");
                    string departureFrom = Console.ReadLine();
                    isstringEmpty(departureFrom);

                    Console.Write("Enter destination location: ");
                    string destinationTo = Console.ReadLine();
                    isstringEmpty(destinationTo);

                    if (departureFrom == "" || destinationTo == "")
                        throw new Exception("\nFailed! You cant insert an empty location.");
                    if (departureFrom == destinationTo)
                        throw new Exception("\nFailed! departure and destination location cannot be the same");

                    Console.Write("Enter flight duration (hh:mm): ");
                    TimeOnly input = TimeOnly.Parse(Console.ReadLine());
                    AirportSystem.CheckIfTimeIsValid(input);
                    TimeSpan duration = new TimeSpan(input.Hour, input.Minute, 0);
                    if (duration.TotalMinutes < 30)
                        throw new Exception("\nFailed! No flight duration can be less than 30 minutes");

                    Console.Write("Is this flight international? Type \"y\" to confirm. ");
                    bool isInternational = Console.ReadLine().ToLower() == "y" ? true : false;

                    Console.Write("Enter seats capacity: ");
                    int capacity = int.Parse(Console.ReadLine());
                    isInputAboveZero(capacity);

                    Console.Write("Enter staff members needed for this flight (both baggage staffs and gate agents): ");
                    int groundStaffCount = int.Parse(Console.ReadLine());
                    isInputAboveZero(groundStaffCount);
                    if (groundStaffCount < 2)
                        throw new Exception($"\nFailed! at least 2 staff members are needed for a single flight.");

                    Console.Write("Enter the maximum baggage weight (in kg) per passenger: ");
                    double MaxBaggageWeight = double.Parse(Console.ReadLine());
                    isInputAboveZero((int)MaxBaggageWeight);

                    Console.Write("Enter departure date (yyyy/mm/dd): ");
                    DateOnly departureDate = DateOnly.Parse(Console.ReadLine());
                    if (departureDate <= DateOnly.FromDateTime(DateTime.Now))
                        throw new Exception("\nFailed! Cannot enter a date that already passed.");

                    Console.Write("Enter departure time in 24h format (hh:mm): ");
                    TimeOnly departureTime = TimeOnly.Parse(Console.ReadLine());
                    AirportSystem.CheckIfTimeIsValid(departureTime);

                    Gate gate = GetAvailableGateBySchedule(departureDate, departureTime);

                    if (!gate.SupportsInternationalFlights && isInternational)
                        throw new Exception($"\nFailed! The gate you selected does not support international flights");

                    Flight flight = new Flight(id, departureFrom, destinationTo, duration, isInternational, capacity, groundStaffCount, MaxBaggageWeight, departureDate, departureTime, departureTime, gate);


                    Console.Write($"\nThe flight has been scheduled to departure on {flight.DepartureDate} at {flight.DepartureTime} from \"{flight.DepartureFrom}\" and land on \"{flight.DestinationTo}\"{(flight.DepartureDate != flight.LandingDate ? $" on {flight.LandingDate}" : "")} at {flight.LandingTime}. \nAlso, ");

                    AssignFlightToGate(flight, gate);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void getFlightToAssignToGate()
        {
            try
            {
                Console.Write("Enter Flight id: ");
                int id = int.Parse(Console.ReadLine());
                isInputAboveZero(id);

                Flight flight = AirportSystem.GetFlightById(id);
                if (flight == null)
                    throw new Exception($"\nFailed! could not find a flight with ID {id}.");

                Gate gate = GetAvailableGateBySchedule(flight.DepartureDate, flight.DepartureTime);

                if (!gate.SupportsInternationalFlights && flight.IsInternational)
                    throw new Exception($"\nFailed! The gate you selected does not support international flights.");

                if (flight.Gate == gate)
                    throw new Exception($"\nFailed! The flight is already assigned to that gate.");

                Gate oldGate = AirportSystem.GetGateById(flight.Gate.Id);
                int flightIndex = oldGate.AssignedFlights.IndexOf(flight);
                oldGate.AssignedFlights.RemoveAt(flightIndex);
                oldGate.BookedTimes.RemoveAt(flightIndex);

                Console.WriteLine();

                AssignFlightToGate(flight, gate);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void updateFlightStatus()
        {
            try
            {
                Console.Write("Enter Flight id: ");
                int id = int.Parse(Console.ReadLine());
                isInputAboveZero(id);

                Flight flight = AirportSystem.GetFlightById(id);
                if (flight == null)
                    throw new Exception($"\nFailed! could not find a flight with ID {id}");

                DateTime today = DateTime.Now;
                if (DateOnly.FromDateTime(today) == flight.DepartureDate)
                {
                    if (TimeOnly.FromDateTime(today) >= flight.DepartureTime)
                        flight.Status = FlightStatus.departed;
                    else if (TimeOnly.FromDateTime(today) > flight.DepartureTime.AddMinutes(-30))
                        flight.Status = FlightStatus.boarding;
                }
                if (flight.Status == FlightStatus.departed || flight.Status == FlightStatus.cancelled)
                    throw new Exception($"\nFailed. You cannot update a flight status after {(flight.Status == FlightStatus.departed ? "it has departed" : "it has been cancelled")}.");

                Console.WriteLine($"\nThis flight status is currently {flight.Status}");

                Console.Write("Flight status: 0- go back\n" +
                              "               1- delayed\n" +
                              "               2- cancelled\n");

                Console.Write("Select an option: ");
                int input = int.Parse(Console.ReadLine());
                if (input < 0 || input > 2)
                    throw new Exception("Please select a status between 0 and 2");
                else
                {
                    if (input == 0)
                        throw new Exception("\nOperation has been cancelled.");

                    FlightStatus status = (FlightStatus)input - 1;

                    if (input == 1)
                    {
                        Console.Write("At what date do you want to delay the flight? (yyyy/mm/dd): ");
                        DateOnly departureDate = DateOnly.Parse(Console.ReadLine());
                        if (departureDate <= DateOnly.FromDateTime(DateTime.Now))
                            throw new Exception("\nFailed. Cannot enter a date that already passed.");
                        if (departureDate < flight.DepartureDate)
                            throw new Exception($"\nFailed. Cannot delay a flight by selecting a date earlier the actual flight date {flight.DepartureDate}.");

                        Console.Write("At what time do you want to delay the flight? in 24h format (hh:mm): ");
                        TimeOnly departureTime = TimeOnly.Parse(Console.ReadLine());
                        AirportSystem.CheckIfTimeIsValid(departureTime);
                        if (departureTime < flight.DepartureTime)
                            throw new Exception($"\nFailed. Cannot delay a flight to a time before the departure time {flight.DepartureTime}.");


                        int flightIndex = flight.Gate.AssignedFlights.IndexOf(flight);
                        List<DateTime> otherBookedTimes = flight.Gate.BookedTimes.Where((time, index) => index != flightIndex).ToList();

                        bool canUpdateGateBookedTime = AirportSystem.IsGateAvailable(otherBookedTimes, departureDate, departureTime);
                        if (!canUpdateGateBookedTime)
                            throw new Exception($"\nFailed! Cannot delay flight at that time, as the assigned gate is already reserved for another flight.");

                        flight.DepartureDate = departureDate;
                        flight.DepartureTime = departureTime;
                        flight.Status = FlightStatus.delayed;

                        flight.Gate.BookedTimes[flightIndex] = new DateTime(departureDate, departureTime);

                        Console.Write($"\nThe flight has been delayed to {departureDate} at {departureTime}\n");
                        Console.WriteLine($"The flight gate \"{flight.Gate.Id}\" reserved duration has been updated to between {flight.DepartureTime.AddMinutes(-30)} and {flight.DepartureTime.AddMinutes(+15)} on {flight.DepartureDate}");
                    }
                    else
                    {
                        Console.Write("Cancelling a flight will affect the passengers, staff and the assigned gate, do you want to proceed. Type \"y\" to confirm. ");
                        if (Console.ReadLine().ToLower() == "y")
                        {
                            flight.Status = FlightStatus.cancelled;

                            List<Passenger> passengers = flight.Passengers;
                            passengers.ForEach(p =>
                            {
                                if (p.Flights.Contains(flight))
                                    p.Flights.Remove(flight);
                            });

                            List<GroundStaff> staffs = flight.GroundStaffs;
                            staffs.ForEach(s =>
                            {
                                if (s.Flights.Contains(flight))
                                {
                                    s.Flights.Remove(flight);
                                    WorkingHoursPerDay flightWorkingHour = s.WorkingHoursPerDays.First(wh => wh.Date == flight.DepartureDate);
                                    flightWorkingHour.WorkedDutyHours -= TimeSpan.Parse("01:30");
                                }
                            });

                            Gate gate = flight.Gate;
                            int flightIndex = gate.AssignedFlights.IndexOf(flight);
                            gate.AssignedFlights.Remove(flight);
                            gate.BookedTimes.RemoveAt(flightIndex);
                            flight.Passengers.ForEach(p => p.Flights.Remove(flight));
                            flight.Passengers.Clear();
                            flight.GroundStaffs.ForEach(s =>
                            {
                                s.Flights.Remove(flight);
                                s.WorkingHoursPerDays.RemoveAll(wd => wd.WorkedDutyHours == TimeSpan.Zero);
                            });
                            flight.GroundStaffs.Clear();


                            Console.WriteLine("The Flight has been cancelled.");
                        }
                        else
                            throw new Exception("\n\nOperation has been cancelled.");
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // --------------------------------------- Gates --------------------------------------- //    

        public void AssignFlightToGate(Flight flight, Gate gate)
        {
            flight.Gate = gate;
            gate.AssignedFlights.Add(flight);
            gate.BookedTimes.Add(new DateTime(flight.DepartureDate, flight.DepartureTime));

            Console.WriteLine($"The Flight with ID {flight.Id} is now assiged to gate {gate.Id}, the gate will be reserved between {flight.DepartureTime.AddMinutes(-30)} and {flight.DepartureTime.AddMinutes(+15)} on {flight.DepartureDate}");
        }
        public Gate GetAvailableGateBySchedule(DateOnly scheduledDate, TimeOnly scheduledTime)
        {

            string availableGates = AirportSystem.ListAvailableGateBasedOnSchedule(scheduledDate, scheduledTime);

            Console.Write("Available gates:\n" + availableGates + "Enter the gate ID to assign the flight to it (Enter \"None\" to cancel): ");
            string selectedGate;
            while (true)
            {
                selectedGate = Console.ReadLine().ToLower();
                if (selectedGate == "")
                    Console.Write("\nSeems like you did not select a gate. Please enter the gate ID to assign the flight to it (Enter \"None\" to cancel): ");
                else if (selectedGate == "None".ToLower())
                    throw new Exception($"\nOperation has been cancelled.");
                else if (!availableGates.Contains($"- {selectedGate} "))
                    Console.Write("\nThe gate you provided is not included in the list. Please try again (Enter \"None\" to cancel): ");
                else
                    break;

                continue;
            }

            Gate gate = AirportSystem.GetGateById(int.Parse(selectedGate));
            if (gate == null)
                throw new Exception($"\nFailed! could not find a gate with ID {int.Parse(selectedGate)}");
            return gate;
        }
        public void ViewGateReservedDurations()
        {
            Console.Write("Enter Gate ID: ");
            int id = int.Parse(Console.ReadLine());
            isInputAboveZero(id);

            Gate gate = AirportSystem.GetGateById(id);
            if (gate == null)
                throw new Exception($"\nFailed! could not find a gate with ID {id}");

            AirportSystem.ListGateReservedDurations(gate);

        }
        // ------------------------------------- Passengers ------------------------------------- //

        public void RegisterPassenger()
        {
            try
            {
                Console.Write("Enter Passenger id: ");
                int id = int.Parse(Console.ReadLine());
                isInputAboveZero(id);

                if (AirportSystem.GetPassengerById(id) != null)
                    throw new Exception("\nFailed! A passenger with the same ID already exist in the system");
                else
                {
                    Console.Write("Enter passenger name: ");
                    string name = Console.ReadLine();
                    isstringEmpty(name);


                    Console.Write("Passenger Type:1- standard\n" +
                                  "               2- VIP\n" +
                                  "               3- reducedMobility\n");

                    Console.Write("Select an option: ");
                    int input = int.Parse(Console.ReadLine());
                    if (input < 1 || input > 3)
                        throw new Exception("Please select a type between 1 and 3");
                    PassengerType type = (PassengerType)input - 1;


                    Passenger passenger = new Passenger(id, name, type);
                    Console.WriteLine($"\nSuccess! Passenger has been added to the system.\n");

                    Console.Write("Does the passenger have a connected flight? Type \"y\" to confirm. ");
                    Flight flight = null;
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        Console.Write("Enter Flight id: ");
                        int flightId = int.Parse(Console.ReadLine());
                        flight = AirportSystem.GetFlightById(flightId);
                        if (flight == null)
                            throw new Exception($"\nFailed! The provided flight ID {flightId} does not exist.");

                        Console.Write("How many baggages does the passenger have? (inserting 0 will cancel the flight connection). ");
                        int baggagesNum = int.Parse(Console.ReadLine());
                        if (baggagesNum == 0)
                            throw new Exception("\nConnection a flight step has been cancelled.");

                        for (int i = 1; i <= baggagesNum; i++)
                        {
                            Console.Write($"{i}- ");
                            RegisterPassengerBaggage(passenger, flight);
                        }

                        CheckPassengerEligibility(passenger, flight, true);

                        BookPassengerOnFlight(passenger, flight);
                    }

                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void GetPassengerAndFlightToCheckForPassengerEligibility()
        {
            try
            {
                Console.Write("Enter passenger ID: ");
                int passengerId = int.Parse(Console.ReadLine());
                isInputAboveZero(passengerId);
                Passenger passenger = AirportSystem.GetPassengerById(passengerId);
                if (passenger == null)
                    throw new Exception($"\nFailed! could not find a passenger with ID {passengerId}");

                Console.Write("Enter flight ID: ");
                int flightId = int.Parse(Console.ReadLine());
                isInputAboveZero(flightId);
                Flight flight = AirportSystem.GetFlightById(flightId);
                if (flight == null)
                    throw new Exception($"\nFailed! could not find a flight with ID {flightId}");

                CheckPassengerEligibility(passenger, flight, false);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void CheckPassengerEligibility(Passenger passenger, Flight flight, bool isBooking)
        {
            if (flight.GroundStaffs.Count() == 0)
                throw new Exception($"\nBoarding Denied! Cant start booking flights before assinging staff to the flight");

            if (!flight.GroundStaffs.Any(gs => gs.Position == Position.baggageStaff))
                throw new Exception($"\nBoarding Denied! Cant start booking flights before assinging at least one Baggage Staff for the flight");

            if (!flight.GroundStaffs.Any(gs => gs.Position == Position.gateAgent))
                throw new Exception($"\nBoarding Denied! Cant start booking flights before assinging at least one Gate Agent for the flight");

            if (passenger.BaggagesWeights.Count == 0)
                throw new Exception($"\nBoarding Denied! cant check a passenger eligibility if his baggages have not been registered yet.");

            if (passenger.Flights.Contains(flight))
                throw new Exception("\nBoarding Denied!The passenger is already booked in this flight.");

            if (flight.Status == FlightStatus.departed || flight.Status == FlightStatus.cancelled)
                throw new Exception($"\nBoarding Denied! The flight {(flight.Status == FlightStatus.departed ? "has already departed" : "got cancelled")}.");

            if (!isBooking && flight.Passengers.Count() >= flight.SeatCapacity)
                throw new Exception($"\nBoarding Denied! The flight has reached its maximum capacity, but the passenger can be added in the standby list.");

            double totalBaggageWeight = passenger.BaggagesWeights.Sum(); // passenger.BaggagesWeights.Select(b => b.Weight).ToList().Sum();
            if (flight.MaxBaggageWeightPerPassenger < totalBaggageWeight)
                throw new Exception($"\nBoarding Denied! The maximum baggage weight ({totalBaggageWeight}kg) exeeds the limits ({flight.MaxBaggageWeightPerPassenger}kg).");

            CheckForConnectingFlights(passenger, flight);

            if (!isBooking)
                Console.WriteLine($"\nPassenger can book this flight");
        }
        public void CheckForConnectingFlights(Passenger passenger, Flight flight)
        {
            foreach (var bookedFlight in passenger.Flights)
            {
                if (bookedFlight.DestinationTo != flight.DepartureFrom)
                    throw new Exception($"\nConnecting Failed! The booked flight (ID: {bookedFlight.Id}) will land in {bookedFlight.DestinationTo} and the seleced flight departure from {flight.DepartureFrom}.");
                if (flight.DepartureTime - bookedFlight.LandingTime < new TimeSpan(0, 45, 0))
                    throw new Exception($"\nConnecting Failed! The time gap between the booked flight landing time ({flight.LandingTime}) and the next flight departure time ({bookedFlight.DepartureTime}) is less than 45m.");
            }
        }

        public void ViewPassengerFlights()
        {

            Console.Write("Enter passenger ID: ");
            int passengerId = int.Parse(Console.ReadLine());
            isInputAboveZero(passengerId);

            Passenger passenger = AirportSystem.GetPassengerById(passengerId);
            if (passenger == null)
                throw new Exception($"\nFailed! could not find a passenger with ID {passengerId}");

            AirportSystem.ListPassengerFlights(passenger);

        }

        // -------------------------------------- Baggages -------------------------------------- //
        public void getGetPassengerToRegisterBaggage()
        {
            try
            {
                Console.Write("Enter passenger ID: ");
                int passengerId = int.Parse(Console.ReadLine());
                isInputAboveZero(passengerId);
                Passenger passenger = AirportSystem.GetPassengerById(passengerId);
                if (passenger == null)
                    throw new Exception($"\nFailed! could not find a passenger with ID {passengerId}");

                Console.Write("Enter Flight id: ");
                int flightId = int.Parse(Console.ReadLine());
                isInputAboveZero(flightId);
                Flight flight = AirportSystem.GetFlightById(flightId);
                if (flight == null)
                    throw new Exception($"\nFailed! The provided flight ID {flightId} does not exist.");
                if (flight.Status == FlightStatus.departed || flight.Status == FlightStatus.cancelled)
                    throw new Exception($"\nFailed! The flight {(flight.Status == FlightStatus.departed ? "has already departed" : "got cancelled")}.");

                RegisterPassengerBaggage(passenger, flight);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void RegisterPassengerBaggage(Passenger passenger, Flight flight)
        {
            if (passenger.BaggagesWeights.Count > 5)
                throw new Exception($"\nFailed! Passenger cannot register more than 5 baggages.");

            Console.Write("Enter baggage weight in kg: ");
            double weight = double.Parse(Console.ReadLine());
            isInputAboveZero((int)weight);

            Console.Write("Was the baggage verified that it does not contain any illegal content? Type \"y\" to confirm. ");
            bool checkedBaggage = Console.ReadLine().ToLower() == "y" ? true : false;
            if (!checkedBaggage)
                throw new Exception($"\nFailed! Cannot register a baggage containing any illegal content.");

            if (passenger.BaggagesWeights.Sum() + weight > flight.MaxBaggageWeightPerPassenger)
                throw new Exception($"\nFailed! The passenger would reach the baggages weight to {passenger.BaggagesWeights.Sum() + weight}kg exceeding the limit {flight.MaxBaggageWeightPerPassenger}kg.");

            passenger.BaggagesWeights.Add(weight);

            Console.WriteLine($"\nSuccess! The Baggage has been registered.\n");
        }
        public void ViewPassengerBaggageWeightLimit()
        {
            try
            {
                Console.Write("Enter Passenger id: ");
                int passengerId = int.Parse(Console.ReadLine());
                isInputAboveZero(passengerId);
                Passenger passenger = AirportSystem.GetPassengerById(passengerId);
                if (passenger == null)
                    throw new Exception($"\nFailed! Could not find a passenger with ID {passengerId}");

                Console.Write("Enter Flight id: ");
                int flightId = int.Parse(Console.ReadLine());
                isInputAboveZero(flightId);
                Flight flight = AirportSystem.GetFlightById(flightId);
                if (flight == null)
                    throw new Exception($"\nFailed! Could not find a flight with ID {flightId}");

                if (!passenger.Flights.Contains(flight))
                    throw new Exception($"\nFailed! Passenger is not booked to the flight with ID {flightId}");

                double flightMaxWeightPerPassenger = flight.MaxBaggageWeightPerPassenger;
                if (passenger.BaggagesWeights.Count == 0)
                    throw new Exception($"\nThe passenger did not register any baggages to check if it reached the flight maxumim limit {flightMaxWeightPerPassenger}kg");

                double passengerTotalBaggagesWeight = passenger.BaggagesWeights.Sum(); //passenger.BaggagesWeights.Select(baggage => baggage.Weight).ToList().Sum()

                Console.WriteLine($"The passenger have reached {passengerTotalBaggagesWeight}kg in total baggage weight allowed out of {flightMaxWeightPerPassenger}kg ({(flightMaxWeightPerPassenger - passengerTotalBaggagesWeight > 0 ? $"can add {flightMaxWeightPerPassenger - passengerTotalBaggagesWeight}kg more" : "Exceeded the limit")})");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // --------------------------------- Booking & Standby --------------------------------- //

        public void GetPassengerAndFlightIDToBookPassenger()
        {
            try
            {

                Console.Write("Enter Passenger id: ");
                int passengerId = int.Parse(Console.ReadLine());
                isInputAboveZero(passengerId);
                Passenger passenger = AirportSystem.GetPassengerById(passengerId);
                if (passenger == null)
                    throw new Exception($"\nFailed! could not find a passenger with ID {passengerId}");

                Console.Write("Enter Flight id: ");
                int flightId = int.Parse(Console.ReadLine());
                isInputAboveZero(flightId);
                Flight flight = AirportSystem.GetFlightById(flightId);
                if (flight == null)
                    throw new Exception($"\nFailed! could not find a flight with ID {flightId}");

                Console.Write("Are all passenger's documents valid? Type \"y\" to confirm. ");
                if (!(Console.ReadLine().ToLower() == "y"))
                    throw new Exception("\nFailed! cant book flight if the required passenger documents are not valid.");
                Console.Write("Did the passenger pay for the ticket? Type \"y\" to confirm. ");
                if (!(Console.ReadLine().ToLower() == "y"))
                    throw new Exception("\nFailed! Cant book flight if the passenger did not pay for the ticket.");

                if (passenger.BaggagesWeights.Count == 0)
                    throw new Exception("\nFailed! The pasenger must register his baggages before booking a flight.");

                CheckPassengerEligibility(passenger, flight, true);

                BookPassengerOnFlight(passenger, flight);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public void BookPassengerOnFlight(Passenger passenger, Flight flight)
        {
            try
            {
                if (flight.Passengers.Count() >= flight.SeatCapacity)
                {
                    if (flight.StandbyList.Passengers.Count >= 15)
                        Console.WriteLine($"\nFailed! The flight is fully booked, as well as the standby list.");

                    Console.Write("\nThe flight is currently fully booked, do you want to add passenger to the standby list? Enter \"y\" to proceed. ");
                    if (!(Console.ReadLine().ToLower() == "y"))
                        throw new Exception("\nBooking has been cancelled.");
                }
                if (passenger.Flights.Count > 0)
                    passenger.Flights.ForEach(f =>
                    {
                        TimeSpan timeBetweenFlights = new DateTime(flight.DepartureDate, flight.DepartureTime) - new DateTime(f.LandingDate, f.LandingTime); //connecting Flight Departure Schedule - Flight Landing Schedule

                        if (timeBetweenFlights < new TimeSpan(0, 45, 0))
                            throw new Exception($"\nFailed! Only {timeBetweenFlights} minutes remain after the Landing flight (ID {f.Id}). The minimum connection time between flights is 45 minutes.");
                    });

                if (flight.Passengers.Count() < flight.SeatCapacity)
                {
                    passenger.Flights.Add(flight);
                    flight.Passengers.Add(passenger);
                    Console.WriteLine($"\nSuccess! The passenger has booked the flight.");
                }
                else
                {
                    flight.StandbyList.Passengers.Add(passenger);
                    Console.WriteLine($"\nThe passenger has been added to the standby list, he will be added automatically if flight opened for available seats.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void CancelBooking()
        {
            try
            {
                Console.Write("Enter Passenger id: ");
                int passengerId = int.Parse(Console.ReadLine());
                isInputAboveZero(passengerId);
                Passenger passenger = AirportSystem.GetPassengerById(passengerId);
                if (passenger == null)
                    throw new Exception($"\nFailed! could not find a passenger with ID {passengerId}");

                Console.Write("Enter Flight id: ");
                int flightId = int.Parse(Console.ReadLine());
                isInputAboveZero(flightId);
                Flight flight = AirportSystem.GetFlightById(flightId);
                if (flight == null)
                    throw new Exception($"\nFailed! could not find a flight with ID {flightId}");

                if (!passenger.Flights.Contains(flight))
                    throw new Exception($"\nFailed! This passenger is not booked to flight {flight.Id}");

                Console.Write("Are you sure you want to cancel? Type \"y\" to confirm. ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    flight.Passengers.Remove(passenger);
                    passenger.Flights.Remove(flight);
                    Console.WriteLine($"\nPassenger with ID {passenger.Id} has cancelld his flight.");
                    if (flight.StandbyList.Passengers.Count > 0)
                    {
                        Passenger standbyPassenger = flight.StandbyList.Passengers[0];
                        flight.Passengers.Add(standbyPassenger);
                        flight.StandbyList.Passengers.Remove(standbyPassenger);
                        standbyPassenger.Flights.Add(flight);
                        Console.WriteLine($"Also, Passenger with ID {standbyPassenger.Id} has been removed from the standby list and successfully booked this flight.");
                    }
                }
                else
                    throw new Exception("\nOperation has been cancelled.");

            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ViewStandbyList()
        {

            Console.Write("Enter Flight id: ");
            int id = int.Parse(Console.ReadLine());
            isInputAboveZero(id);

            Flight flight = AirportSystem.GetFlightById(id);
            if (flight == null)
                throw new Exception($"\nFailed! could not find a flight with ID {id}");

            AirportSystem.ListAFlightStandbyList(flight.StandbyList.Passengers);
        }
        public void GetFlightToViewItsPassengersList()
        {
            try
            {
                Console.Write("Enter Flight id: ");
                int id = int.Parse(Console.ReadLine());
                isInputAboveZero(id);

                Flight flight = AirportSystem.GetFlightById(id);
                if (flight == null)
                    throw new Exception($"\nFailed! could not find a flight with ID {id}");

                AirportSystem.ListPassengersPerFlight(flight.Passengers);
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // --------------------------------------- Staff --------------------------------------- //

        public void HireStaff()
        {
            try
            {
                Console.Write("Enter staff id: ");
                int id = int.Parse(Console.ReadLine());
                isInputAboveZero(id);

                if (AirportSystem.GetStaffById(id) != null)
                    throw new Exception("\nFailed! a staff already exists with the same ID provided");
                else
                {
                    Console.Write("Position name: 1- gateAgent\n" +
                                  "               2- baggageStaff\n");
                    Console.Write("Enter position name: ");
                    int input = int.Parse(Console.ReadLine());
                    if (input != 1 && input != 2)
                        throw new Exception("\nFailed! Please select position 1 or 2");
                    Position position = (Position)input - 1;

                    GroundStaff emp = new GroundStaff(id, position);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AssignStaff()
        {
            try
            {
                Console.Write("Enter staff id: ");
                int staffId = int.Parse(Console.ReadLine());
                GroundStaff staff = AirportSystem.GetStaffById(staffId);
                if (staffId == 0)
                    throw new Exception("\nFailed! It is not the job of the admin to be assigned to do gateAgents or baggageStaffs jobs.");
                isInputAboveZero(staffId);
                if (staff == null)
                    throw new Exception($"\nFailed! Could not find a ground staff with ID {staffId}");

                Console.Write("Enter Flight id: ");
                int flightId = int.Parse(Console.ReadLine());
                isInputAboveZero(flightId);
                Flight flight = AirportSystem.GetFlightById(flightId);
                if (flight == null)
                    throw new Exception($"\nFailed! Could not find a flight with ID {flightId}");

                if (staff.Flights.Any(f => f == flight))
                    throw new Exception("\nFailed! Employee is already assigned to this flight");
                if (flight.GroundStaffCount <= flight.GroundStaffs.Count)
                    throw new Exception("\nFailed! This flight has all the needed staff assigned to it");

                Console.Write($"\nNote: This staff works as a {staff.Position} do you want to assign him to {(staff.Position == Position.baggageStaff ? "this flight" : $"gate {flight.Gate.Id}")}? Type \"y\" to confirm.");
                if (Console.ReadLine().ToLower() == "y")
                {
                    AirportSystem.IsStaffAvailable(staff, flight.DepartureDate, flight.DepartureTime);

                    flight.GroundStaffs.Add(staff);
                    staff.Flights.Add(flight);

                    if (staff.WorkingHoursPerDays.Count == 0 || !staff.WorkingHoursPerDays.Any(date => date.Date == flight.DepartureDate))
                    {
                        staff.WorkingHoursPerDays.Add(new WorkingHoursPerDay(flight.DepartureDate, TimeSpan.Parse("01:30")));
                    }
                    else
                    {
                        WorkingHoursPerDay workingHoursPerDay = staff.WorkingHoursPerDays.First(date => date.Date == flight.DepartureDate);
                        workingHoursPerDay.WorkedDutyHours += TimeSpan.Parse("01:30");
                    }

                    Console.WriteLine($"\nSuccess! Employee has been assigned to the flight {(staff.Position == Position.baggageStaff ? $"with ID {flight.Id}" : $"gate number {flight.Gate.Id}")}");
                }
                else
                    throw new Exception("\nOperation has been cancelled.");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ViewStaffDutyHours()
        {
            Console.Write("Enter staff id: ");
            int id = int.Parse(Console.ReadLine());
            if (id == 0)
                throw new Exception("\nThe duty hours are tracked for gateAgents and baggageStaffs, not the admin.");
            isInputAboveZero(id);

            GroundStaff staff = AirportSystem.GetStaffById(id);

            if (staff == null)
                throw new Exception($"\nFailed! could not find a ground staff with ID {id}");

            AirportSystem.ListStaffWorkingHours(staff);
        }


        // ------------- Reusable ------------- //
        public void isInputAboveZero(int input)
        {
            if (input <= 0)
                throw new Exception($"\nFailed! cannot insert a 0 value or below.");
        }
        public void isstringEmpty(string input)
        {
            if (input == "")
                throw new Exception($"\nFailed! cannot insert an empty value.");
        }

    }
}