namespace ThemeParkManagementSystem
{
    public enum Position
    {
        TicketBoothStaff,
        RideOperator,
        OperationsManager,
        Admin
    }
    public class Employee
    {
        public int Id { get; set; }
        public Position Position { get; set; }
        public string AssignedRideName { get; set; }
        public bool IsAssigned { get; set; }
        internal static int Count { get; set; }

        public Employee(int id, Position position, string assignedRideName)
        {
            Id = id;
            Position = position;
            AssignedRideName = assignedRideName;
            IsAssigned = AssignedRideName != "None";
            ParkSystem.Add(this);
            Count++;
        }


        // -------------------------------------- Visitors -------------------------------------- //
        public void CreateVisitor()
        {
            if (this.Position == Position.Admin || this.Position == Position.TicketBoothStaff)
                // the validInput was used across most of the methods so that if the user inserted the wrong input, he will need to start the process all over without being able to go back (sol: cancel it until i add the option to go back when the user press X for example)
                //bool validInput = false;
                //while (!validInput)
                //{
                try
                {
                    Console.Write("Enter visitor id: ");
                    int id = int.Parse(Console.ReadLine());

                    if (ParkSystem.GetVisitorById(id) != null)
                        throw new Exception("\nFailed! a visitor already exists with the same ID provided\n");
                    else
                    {
                        Console.Write("Enter visitor age: ");
                        int age = int.Parse(Console.ReadLine());
                        if (age < 0 || age > 100)
                            throw new Exception("\nAge cannot be below 0 or above 100\n"); // age restriction will be applied on reservations, but vistors can enter the park

                        Console.Write("Enter visitor height: ");
                        int height = int.Parse(Console.ReadLine());
                        if (height < 45 || height > 250)
                            throw new Exception("\nHeight cannot be below 45cm or above 250cm\n"); // height restriction will be applied on reservations, but vistors can enter the park

                        Console.Write("Enter visitor number of accompanying adults: ");
                        int numberOfAccompanyingAdults = int.Parse(Console.ReadLine());
                        if (numberOfAccompanyingAdults < 0 || numberOfAccompanyingAdults > 2)
                            throw new Exception("\nHeight cannot be below 0 or above 2\n");

                        Console.Write("Vistor tier: 1- GeneralAdmission\n" +
                                      "             2- VIP\n" +
                                      "             3- Child\n" +
                                      "             4- Senior\n" +
                                      "             5- Staff Accompanied Minor\n");

                        Console.Write("Select an option: ");
                        int input = int.Parse(Console.ReadLine());
                        if (input < 1 || input > 5)
                            throw new Exception("\nPlease select a vistor tier between 1 and 5\n");
                        Tier type = (Tier)input - 1;


                        Visitor newVisitor = new Visitor(id, age, height, numberOfAccompanyingAdults, type);

                        //validInput = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            //}
        }

        // --------------------------------------- Rides --------------------------------------- //
        public void CreateRide()
        {
            try
            {
                Console.Write("Enter ride name: ");
                string name = Console.ReadLine();
                if (name == "")
                    throw new FormatException();

                if (ParkSystem.GetRidebyName(name) != null)
                    throw new Exception("Failed! a ride already exists with the same name provided\n");
                else
                {
                    Console.Write("Enter minimun allowed age: ");
                    int minAge = int.Parse(Console.ReadLine());
                    if (minAge < 6 || minAge > 20)
                        throw new Exception("Age cannot be below 6 or above 20\n");

                    Console.Write("Enter minimun allowed height: ");
                    int minHeight = int.Parse(Console.ReadLine());
                    if (minHeight < 80 || minHeight > 160)
                        throw new Exception("Height cannot be below 80 or above 160\n");

                    Console.Write("Enter minimun accompanying adult allowed: ");
                    int minimumAccompanyingAdult = int.Parse(Console.ReadLine());
                    if (minimumAccompanyingAdult < 0 || minimumAccompanyingAdult > 2)
                        throw new Exception("accompanying adults cannot be below 0 or above 2\n");

                    Console.Write("Enter assigned employee id (enter \"0\" if you dont want it to be assigned yet): ");
                    int employeeId = int.Parse(Console.ReadLine());
                    if (ParkSystem.GetEmployeeById(employeeId) == null && employeeId != 0)
                        throw new Exception($"Could not find an employee with the ID: {employeeId}\n");

                    Console.Write("Enter ride capacity: ");
                    int capacity = int.Parse(Console.ReadLine());
                    Console.Write("Ride types: 1- thrill\n" +
                                  "            2- family\n" +
                                  "            3- water\n");
                    Console.Write("Select an option: ");
                    int input = int.Parse(Console.ReadLine());
                    if (input < 1 || input > 3)
                        throw new Exception("Please select a ride type between 1 and 3\n");
                    RideType type = (RideType)input - 1;

                    Console.Write("Ride status: 1- open\n" +
                                  "             2- closed\n" +
                                  "             3- underMaintenance\n");

                    Console.Write("Select an option: ");
                    input = int.Parse(Console.ReadLine());
                    if (input < 1 || input > 3)
                        throw new Exception("Please select a ride status between 1 and 3\n");
                    RideStatus status = (RideStatus)input - 1;

                    Console.Write("Ride tier: 1- GeneralAdmission\n" +
                                  "           2- VIP\n" +
                                  "           3- Child\n" +
                                  "           4- Senior\n" +
                                  "           5- Staff Accompanied Minor\n");

                    Console.Write("Select an option: ");
                    input = int.Parse(Console.ReadLine());
                    if (input < 1 || input > 5)
                        throw new Exception("Please select a ride tier between 1 and 5\n");
                    Tier tier = (Tier)input - 1;

                    if (!CheckIfEmployeeIsAssigned(employeeId))
                    {
                        Ride newRide = new Ride(name, minAge, minHeight, minimumAccompanyingAdult, capacity, type, status, tier, employeeId);
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
        public bool CheckIfEmployeeIsAssigned(int employeeId)
        {
            Employee employee = ParkSystem.GetEmployeeById(employeeId);

            if (employee != null && employee.AssignedRideName != "None")
                throw new Exception($"\nThe employee with the ID {employeeId} is already assigned to the ride {employee.AssignedRideName}.\n");

            return false;
        }
        public bool ValidateRideAccess(bool forReservation, int visitorId, string rideName)
        {
            string result = !forReservation ? "ACCESS DENIED" : "RESERVATION FAILED";
            bool returnResult = false;
            try
            {
                if (!forReservation)
                {
                    Console.Write("Enter visitor id: ");
                    visitorId = int.Parse(Console.ReadLine());

                    Console.Write("Enter ride name: ");
                    rideName = Console.ReadLine();
                }

                Visitor visitor = ParkSystem.GetVisitorById(visitorId);
                Ride ride = ParkSystem.GetRidebyName(rideName);

                if (visitor == null)
                    throw new Exception($"\nResult: {result}\nReason: Could not find the visitor you are refering to.\n");
                else if (ride == null)
                    throw new Exception($"\nResult: {result}\nReason: Could not find the ride you are refering to.\n");
                else if (visitor.Age < ride.MinAge)
                    throw new Exception($"\nResult: {result}\nReason: Visitor does not meet the minimum age requirement ({ride.MinAge}) for this ride.\n");
                else if (visitor.Height < ride.MinHeight)
                    throw new Exception($"\nResult: {result}\nVisitor does not meet the minimum height requirement ({ride.MinHeight}cm) for this ride.\n");
                else if (visitor.Age < ride.MinAge && ride.MinimumAccompanyingAdult != 0 && visitor.NumberOfAccompanyingAdults < ride.MinimumAccompanyingAdult)
                    throw new Exception($"\nResult: {result}\nReason: Visitor must have at least {ride.MinimumAccompanyingAdult} accompanying adults with him.\n");
                else if (visitor.Tier != ride.Tier && visitor.Tier != Tier.VIP)
                    throw new Exception($"\nResult: {result}\nReason: Visitor tier ({visitor.Tier}) does not match the ride tier ({ride.Tier}).\n");
                else if (ride.Status == RideStatus.closed || ride.Status == RideStatus.underMaintenance)
                    throw new Exception($"\nResult: {result}\nReason: The ride is currently {ride.Status}.\n");
                else
                    returnResult = true;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("\nThe provided Visitor ID or Ride Name does not exist in the system.\n");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return returnResult;
        }
        public void ManageRideStatus()
        {
            try
            {
                Console.Write("Enter ride name: ");
                string name = Console.ReadLine();

                Ride ride = ParkSystem.GetRidebyName(name);
                if (ride == null)
                    throw new Exception("\nThe provided ride name does not exist. \n");

                Console.WriteLine($"The current Ride status is \"{ride.Status}\"");

                Console.Write("Ride status: 0- Dont change it\n" +
                              "             1- open\n" +
                              "             2- closed\n" +
                              "             3- underMaintenance\n");

                Console.Write("Select an option for the new ride status: ");

                int input = int.Parse(Console.ReadLine());
                if (input == 0)
                    Console.WriteLine($"\n\nOperation has been cancelled.\n");
                else if (input >= 0 && input <= 3)
                {
                    RideStatus status = (RideStatus)input - 1;
                    ride.Status = status;
                    string affectedTicketsMessage = "";
                    if (status == RideStatus.open)
                    {
                        Employee emp = ParkSystem.GetEmployeeById(ride.AssignedEmployeeId);
                        if (ride.AssignedEmployeeId != 0 && emp != null)
                        {
                            ParkSystem.UpdateTicketsStatusByRideName(name, TicketStatus.valid);
                            affectedTicketsMessage = " and all related tickets (if exist) has been set to valid if the ride had an employee assigned to it.";
                        }
                    }
                    else
                    {
                        ParkSystem.UpdateTicketsStatusByRideName(name, TicketStatus.cancelled);
                        affectedTicketsMessage = " and all related tickets (if exist) has been cancelled.";
                    }

                    Console.WriteLine($"\n\nRide status has been updated to \"{status}\".{affectedTicketsMessage}\n");
                }
                else
                {
                    throw new Exception("\nPlease select a ride status between 0 and 3\n");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
        public void CancelRide()
        {
            try
            {
                Console.Write("Enter ride name: ");
                string name = Console.ReadLine();

                Ride ride = ParkSystem.GetRidebyName(name);

                if (ride == null)
                    throw new Exception("\nThe provided ride name does not exist in the system.\n");

                Console.Write("Are you sure you want to cancel this ride? all related tickets and reservations will be cancelled too. Type \'y\' to confirm ");
                if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    ParkSystem.Delete(ride);
                else
                    Console.Write("\n\nOperation has been cancelled.\n");
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

        // -------------------------------------- Tickets -------------------------------------- //
        public void CreateTicket()
        {
            try
            {
                Console.Write("Enter ticket id: ");
                int ticketId = int.Parse(Console.ReadLine());


                if (ParkSystem.GetTicketById(ticketId) != null)
                    throw new Exception("\n! a ticket already exists with the same ID provided\n");
                else
                {
                    Console.Write("Enter ride name: ");
                    string rideName = Console.ReadLine();

                    Ride ride = ParkSystem.GetRidebyName(rideName);
                    if (ride == null)
                        throw new Exception("\nThe ride you provided does not exist.\n");
                    TicketStatus status = ride.AssignedEmployeeId == 0 || ride.Status != RideStatus.open ? TicketStatus.invalid : TicketStatus.valid;

                    Console.Write("Enter ticket expiry date (yyyy/mm/dd): ");
                    DateOnly expireDate = DateOnly.Parse(Console.ReadLine());
                    if (expireDate < DateOnly.FromDateTime(DateTime.Now))
                        throw new Exception("\nCannot enter a date that already passed.\n");

                    Console.Write("Ticket tier: 1- GeneralAdmission\n" +
                                  "             2- VIP\n" +
                                  "             3- Child\n" +
                                  "             4- Senior\n" +
                                  "             5- Staff Accompanied Minor\n");

                    Console.Write("Select an option: ");
                    int input = int.Parse(Console.ReadLine());
                    if (input < 1 || input > 5)
                        throw new Exception("\nPlease select a ticket tier between 1 and 5\n");
                    Tier type = (Tier)input - 1;
                    if (ride.Tier != type)
                        throw new Exception($"\nThe selected teir does not match the ride teir ({ride.Tier})\n");


                    Console.Write("Enter ticket price: ");
                    int price = int.Parse(Console.ReadLine());


                    Ticket newTicket = new Ticket(ticketId, rideName, expireDate, type, price, status);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
        public void ManageTicketStatus()
        {
            try
            {
                Console.Write("Enter ticket id: ");
                int id = int.Parse(Console.ReadLine());

                Ticket ticket = ParkSystem.GetTicketById(id);
                if (ticket == null)
                    throw new Exception("\nThe provided ticket ID does not exist. \n");

                Console.WriteLine($"The current ticket status is \"{ticket.Status}\"");

                if (ticket.Status != TicketStatus.cancelled)
                {
                    Console.Write("Do you want to cancel it? Type \'y\' to confirm.");
                    if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    {
                        ticket.Status = TicketStatus.cancelled;
                        Console.Write($"\n\nThe ticket has been cancelled\n");
                    }
                    else
                        throw new Exception("\n\nOperation has been cancelled.\n");
                }
                else
                {
                    Console.Write("Do you want to set it as valid? Type \'y\' to confirm. ");
                    if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    {
                        Ride ride = ParkSystem.GetRidebyName(ticket.RideName);
                        if (ride.Status == RideStatus.open && ride.AssignedEmployeeId != 0)
                        {
                            ticket.Status = TicketStatus.valid;
                            Console.Write($"\n\nThe ticket is now valid\n");
                        }
                        else
                        {
                            ticket.Status = TicketStatus.invalid;
                            string reason = ride.Status != RideStatus.open ? $"is currently {ride.Status}" : "does not have an employee assigned to it yet";
                            Console.Write($"\n\nThe ticket has been set to invalid, as the \"{ticket.RideName}\" ride status related to it {reason}\n");
                        }
                    }
                    else
                        throw new Exception("\n\nOperation has been cancelled.\n");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
        public void DeleteTicket()
        {
            try
            {
                Console.Write("Enter ticket id: ");
                int id = int.Parse(Console.ReadLine());

                Ticket ticket = ParkSystem.GetTicketById(id);

                if (ticket == null)
                    throw new Exception("\nThe provided ticket ID does not exist in the system.\n");

                if (ticket.ExpireDate > DateOnly.FromDateTime(DateTime.Now))
                    Console.Write("The ticket did not expire yet, are you sure you want to delete it?, all related reservations will be deleted too. Type \'y\' to confirm. ");
                else
                    Console.Write("Deleting this expired ticket will also delete all related reservations too. Type \'y\' to confirm. ");

                if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    ParkSystem.Delete(ticket);
                else
                    Console.Write("\n\nOperation has been cancelled.\n");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please make sure to enter the correct input type as requested.\n");
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

        // ------------------------------------ Reservations ------------------------------------ //
        public void CreateReservation()
        {
            try
            {
                Console.Write("Enter visitor id: ");
                int visitorId = int.Parse(Console.ReadLine());

                Console.Write("Enter ticket id: ");
                int ticketId = int.Parse(Console.ReadLine());

                if (ParkSystem.GetReservationById(int.Parse($"{visitorId}{ticketId}")) != null)
                    throw new Exception("\nFailed! the visitor has already booked a reservation on that ticket\n");
                else
                {
                    Console.Write("Enter ride name: ");
                    string rideName = Console.ReadLine();

                    if (DoEntitiesExist(visitorId, ticketId, rideName) && ValidateTicketRide(ticketId, rideName) && ValidateRideAccess(true, visitorId, rideName))
                    {
                        Console.Write("Enter ticket expiry date (yyyy/mm/dd): ");
                        DateOnly date = DateOnly.Parse(Console.ReadLine());
                        if (date < DateOnly.FromDateTime(DateTime.Now))
                            throw new Exception("\nCannot enter a date that already passed.\n");

                        Ticket ticket = ParkSystem.GetTicketById(ticketId);
                        if (date > ticket.ExpireDate)
                            throw new Exception($"\nCannot create a reservation that is set after the ticket expiry date ({ticket.ExpireDate}).\n");

                        Console.Write("Enter desired time slot (hh:mm): ");
                        TimeOnly timeSlot = TimeOnly.Parse(Console.ReadLine());

                        if (ParkSystem.DidVisitorReserveOnTheSameDateAndTimeInAnotherTicket(visitorId, date, timeSlot))
                            throw new Exception("\nFailed! The visitor is already booked in a reservation on the same date and time in another ticket\n");

                        int reservationsCount = ParkSystem.CountReservationsPerTimeSlot(rideName, timeSlot);
                        Ride ride = ParkSystem.GetRidebyName(rideName);
                        if (ride.Capacity <= reservationsCount)
                            throw new Exception($"\nResult: RESERVATION FAILED\nReason: Ride has reached maximum capacity ({ride.Capacity}) for the selected time slot.\n");

                        Reservation newTicket = new Reservation(visitorId, ticketId, rideName, date, timeSlot);

                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
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
        public bool DoEntitiesExist(int visitorId, int ticketId, string rideName)
        {

            Visitor visitor = ParkSystem.GetVisitorById(visitorId);
            Ticket ticket = ParkSystem.GetTicketById(ticketId);
            Ride ride = ParkSystem.GetRidebyName(rideName);

            if (visitor != null && ticket != null && ride != null)
                return true;

            if (visitor == null)
                throw new Exception("\nThe provided Visitor ID does not exist in the system.\n");
            if (ticket == null)
                throw new Exception("\nThe provided Ticket ID does not exist in the system.\n");
            if (ride == null)
                throw new Exception("\nThe provided Ride Name does not exist in the system.\n");

            return false;

        }
        public bool ValidateTicketRide(int ticketId, string rideName)
        {
            bool returnResult = false;
            try
            {
                Ticket ticket = ParkSystem.GetTicketById(ticketId);
                Ride ride = ParkSystem.GetRidebyName(rideName);

                ticket.Status = ticket.ExpireDate < DateOnly.FromDateTime(DateTime.Now) ? TicketStatus.expired : ticket.Status;
                if (ticket.RideName != rideName)
                    throw new Exception($"\nResult: RESERVATION FAILED\nReason: The provided ticket ID does not match the provided ride name.\n");
                else if (ticket.Status == TicketStatus.invalid || ticket.Status == TicketStatus.expired || ticket.Status == TicketStatus.cancelled)
                {
                    string statusMessage = ticket.Status == TicketStatus.expired ? $"has expired since {ticket.ExpireDate}" :
                        ticket.Status == TicketStatus.invalid && ride.Status == RideStatus.open ? "is invalid as there is no employee assigned to it yet" :
                        ticket.Status == TicketStatus.invalid && ride.Status != RideStatus.open ? $"is invalid as the ride is currently {ride.Status}" :
                                                                                    "has been cancelled";
                    throw new Exception($"\nResult: RESERVATION FAILED\nReason: The provided ticket ID {statusMessage}\n");
                }
                else
                    returnResult = true;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("\nThe provided Ticket ID or Ride Name does not exist in the system.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return returnResult;

        }
        public void CheckReservationCapacityPerRide()
        {
            try
            {
                Console.Write("Enter reservation id (visitor ID and ticket ID): ");
                int id = int.Parse(Console.ReadLine());

                Reservation reservation = ParkSystem.GetReservationById(id);

                if (reservation == null)
                    throw new Exception("\nThe provided reservation ID does not exist in the system.\n");

                int reservationsCount = ParkSystem.CountReservationsPerTimeSlot(reservation.RideName, reservation.TimeSlot);
                Ride ride = ParkSystem.GetRidebyName(reservation.RideName);
                Console.WriteLine($"\"{reservation.RideName}\" ride at {reservation.TimeSlot} has {reservationsCount} reserved out of {ride.Capacity}");

            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
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
        public void CancelReservation()
        {
            try
            {
                Console.Write("Enter reservation id (visitor ID and ticket ID): ");
                int id = int.Parse(Console.ReadLine());

                Reservation reservation = ParkSystem.GetReservationById(id);

                if (reservation == null)
                    throw new Exception("\nThe provided reservation ID does not exist in the system.\n");

                Console.Write("Are you sure you want to delete this reservation? Type \'y\' to confirm ");
                if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    ParkSystem.Delete(reservation);
                else
                    Console.Write("\n\nOperation has been cancelled.\n");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
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
                Console.Write("Enter employee id: ");
                int id = int.Parse(Console.ReadLine());


                if (ParkSystem.GetEmployeeById(id) != null)
                    throw new Exception("\nFailed! an employee already exists with the same ID provided\n");
                else
                {

                    Console.Write("Position name: 1- TicketBoothStaff\n" +
                                  "               2- RideOperator\n" +
                                  "               3- OperationsManager\n" +
                                  "               4- Admin\n");
                    Console.Write("Enter position name: ");
                    int input = int.Parse(Console.ReadLine());
                    if (input < 1 || input > 4)
                        throw new Exception("\nPlease select a position name between 1 and 4\n");
                    Position position = (Position)input - 1;

                    string rideName = ParkSystem.ListCurrentRidesAndAssignOnes(true);

                    Employee emp = new Employee(id, position, rideName);
                    if (rideName != "None")
                        ParkSystem.UpdateAssignedEmployeeInRide(rideName, id, TicketStatus.valid);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
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
                Console.Write("Enter employee id: ");
                int id = int.Parse(Console.ReadLine());

                Employee emp = ParkSystem.GetEmployeeById(id);

                if (emp == null)
                    throw new Exception("\nThe provided employee ID does not exist in the system.\n");

                string rideName;
                if (emp.AssignedRideName == "None")
                {
                    rideName = ParkSystem.ListCurrentRidesAndAssignOnes(false);
                    if (rideName != "None")
                    {
                        ParkSystem.UpdateAssignedEmployeeInRide(rideName, id, TicketStatus.valid);
                        emp.AssignedRideName = rideName;
                        emp.IsAssigned = true;
                        Console.Write($"\nEmployee with ID {emp.Id} has been assigned to {rideName}. And its related tickets status (if exists) have been set to valid\n");
                    }
                    else
                        throw new Exception("\n\nOperation has been cancelled.\n");
                }
                else
                {
                    Console.Write("Cant assign employee as he is already assigned to another ride. Do you want to assign him as \"None\"? Type \'y\' to confirm.");
                    if (Console.ReadKey().KeyChar == 'y' || Console.ReadKey().KeyChar == 'Y')
                    {
                        ParkSystem.UpdateAssignedEmployeeInRide(emp.AssignedRideName, id, TicketStatus.invalid);
                        emp.AssignedRideName = "None";
                        emp.IsAssigned = false;
                        Console.Write($"\nEmployee with ID {emp.Id} has been unassigned from the ride. And its related tickets status (if exists) have been set to invalid\n");
                    }
                    else
                        throw new Exception("\n\nOperation has been cancelled.\n");
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
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
        public void CheckStaffStatus()
        {
            try
            {
                Console.Write("Enter Employee Id: ");
                int id = int.Parse(Console.ReadLine());

                Employee employee = ParkSystem.GetEmployeeById(id);

                if (employee == null)
                    throw new Exception("\nThe employee Id you inserted does not exist.\n");

                string status = employee.IsAssigned ? "Assigned" : "Unassigned";
                Console.WriteLine($"The current Employee status is {status}");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
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


        // ------------- Reusable ------------- //
        public int EnterVistorId()
        {
            try
            {
                Console.Write("Enter Visitor ID: ");
                int visitorId = int.Parse(Console.ReadLine());
                return visitorId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }
        public string EnterRideName()
        {
            try
            {
                Console.Write("Enter Ride Name: ");
                string rideName = Console.ReadLine();
                return rideName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }
    }
}