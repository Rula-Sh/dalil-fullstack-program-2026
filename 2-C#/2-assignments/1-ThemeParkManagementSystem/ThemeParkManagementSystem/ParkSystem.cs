namespace ThemeParkManagementSystem
{
    public enum Tier
    {
        generalAdmission,
        VIP,
        child,
        senior,
        staffAccompaniedMinor
    }
    public class ParkSystem
    {
        //public static List<Employee> Employees { get; set; }
        //public static List<Visitor> Visitors { get; set; }
        //public static List<Ride> Rides { get; set; }
        //public static List<Reservation> Reservations { get; set; }
        //public static List<Ticket> Tickets { get; set; }

        //public static Employee[] Employees { get; set; }
        //public static Visitor[] Visitors { get; set; }
        //public static Ride[] Rides { get; set; }
        //public static Reservation[] Reservations { get; set; }
        //public static Ticket[] Tickets { get; set; }

        private static Employee[] employees;
        private static Visitor[] visitors;
        private static Ride[] rides;
        private static Reservation[] reservations;
        private static Ticket[] tickets;

        public static Employee[] Employees { get => employees; set => employees = value; }
        public static Visitor[] Visitors { get => visitors; set => visitors = value; }
        public static Ride[] Rides { get => rides; set => rides = value; }
        public static Reservation[] Reservations { get => reservations; set => reservations = value; }
        public static Ticket[] Tickets { get => tickets; set => tickets = value; }

        static ParkSystem()
        {
            //    Visitors = new List<Visitor>();
            //    Employees = new List<Employee>();
            //    Rides = new List<Ride>();
            //    Reservations = new List<Reservation>();
            //    Tickets = new List<Ticket>();

            try
            {
                Console.WriteLine("Congratulations for the opening of \"Horizon Adventure Park\" to get started, please enter the total number of each of the following you want to manage in the park:\n");

                Console.Write("Total Visitors: ");
                Visitors = new Visitor[int.Parse(Console.ReadLine())];
                Console.Write("Total Employees: ");
                Employees = new Employee[int.Parse(Console.ReadLine())];
                Console.Write("Total Rides: ");
                Rides = new Ride[int.Parse(Console.ReadLine())];
                Console.Write("Total Tickets: ");
                Tickets = new Ticket[int.Parse(Console.ReadLine())];
                Console.Write("Total Reservations: ");
                Reservations = new Reservation[int.Parse(Console.ReadLine())];
                Console.WriteLine("");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to enter the correct input type as requested.\n");
            }
            catch (OverflowException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        // ---------------------------------- Create & Delete ---------------------------------- //
        internal static void Add<T>(T entity)
        {
            string type = entity.GetType().Name;
            string extraMessage = "";
            bool ignoreCreationMessage = false;

            bool Added = false;
            if (entity is Employee emp)
            {
                //Employees.Add(emp);
                for (int i = 0; i < Employees.Length; i++)
                {
                    if (Employees[i] == null)
                    {
                        Employees[i] = emp;
                        Added = true;
                        extraMessage = $" And he has been assigned to {emp.AssignedRideName}";
                        break;
                    }
                }
                if (emp.Position == Position.Admin) ignoreCreationMessage = true;
            }
            else if (entity is Visitor vis)
            {
                //Visitors.Add(vis);
                for (int i = 0; i < Visitors.Length; i++)
                {
                    if (Visitors[i] == null)
                    {
                        Visitors[i] = vis;
                        Added = true;
                        break;
                    }
                }
            }
            else if (entity is Ride ride)
            {
                //Rides.Add(ride);
                for (int i = 0; i < Rides.Length; i++)
                {
                    if (Rides[i] == null)
                    {
                        Rides[i] = ride;
                        Added = true;

                        if (Rides[i].AssignedEmployeeId != 0)
                        {
                            Employee employee = GetEmployeeById(Rides[i].AssignedEmployeeId);
                            employee.IsAssigned = true;
                            employee.AssignedRideName = ride.Name;
                            extraMessage = $" And it has been assigned to Employee with ID {ride.AssignedEmployeeId}";
                        }

                        break;
                    }
                }
            }
            else if (entity is Reservation res)
            {
                //Reservations.Add(res); 
                for (int i = 0; i < Reservations.Length; i++)
                {
                    if (Reservations[i] == null)
                    {
                        Reservations[i] = res;
                        Added = true;
                        break;
                    }
                }
            }
            else if (entity is Ticket tik)
            {
                //Tickets.Add(tik);
                for (int i = 0; i < Tickets.Length; i++)
                {
                    if (Tickets[i] == null)
                    {
                        Tickets[i] = tik;
                        Added = true;
                        break;
                    }
                }

                extraMessage = tik.Status == TicketStatus.invalid ? "\nThough, its status has been set as invalid, as either the ride status is not open or it does not have an employee assigned to it." : "";
            }
            else
            {
                throw new Exception($"Error, could not add {type}\n");
            }
            if (Added)
            {
                if (!ignoreCreationMessage)
                    Console.WriteLine($"\n{type} has been created!{extraMessage}\n");
            }
            else
            {
                throw new Exception($"\nCould not add another {type}. All slots have been filled.\n");
            }
        }
        internal static void Delete<T>(T entity)
        {
            string type = entity.GetType().Name;
            bool deleted = false;
            string extraMessage = "";

            if (entity is Ride ride)
            {
                //Rides.Remove(ride);
                for (int i = 0; i < Rides.Length; i++)
                {
                    if (Rides[i] == ride)
                    {
                        if (Rides[i].AssignedEmployeeId != 0)
                        {
                            Employee employee = GetEmployeeById(Rides[i].AssignedEmployeeId);
                            employee.IsAssigned = false;
                            employee.AssignedRideName = "None";
                            extraMessage = $" And it has been unassiged to Employee with ID {employee.Id}";
                        }

                        Rides[i] = null;
                        Ride.Count--;
                        deleted = true;
                        break;
                    }
                }

                foreach (var tik in Tickets)
                    if (tik != null && tik.RideName == ride.Name)
                    {
                        //Tickets.Remove(tik);
                        for (int i = 0; i < Tickets.Length; i++)
                        {
                            if (Tickets[i] == tik)
                            {
                                Tickets[i] = null;
                                Ticket.Count--;
                                break;
                            }
                        }
                    }
                foreach (var res in Reservations)
                    if (res != null && res.RideName == ride.Name)
                    {
                        //Reservations.Remove(res);
                        for (int i = 0; i < Reservations.Length; i++)
                        {
                            if (Reservations[i] == res)
                            {
                                Reservations[i] = null;
                                Reservation.Count--;
                                break;
                            }
                        }
                    }

                extraMessage += $"\nAlso, all releated tickets and reservation have been deleted.";
            }
            else if (entity is Reservation reservation)
            {
                //Reservations.Remove(reservation);
                for (int i = 0; i < Reservations.Length; i++)
                {
                    if (Reservations[i] == reservation)
                    {
                        Reservations[i] = null;
                        Reservation.Count--;
                        deleted = true;
                        break;
                    }
                }
            }
            else if (entity is Ticket ticket)
            {
                //Tickets.Remove(ticket);

                for (int i = 0; i < Tickets.Length; i++)
                {
                    if (Tickets[i] == ticket)
                    {
                        Tickets[i] = null;
                        Ride.Count--;
                        deleted = true;
                        break;
                    }
                }

                foreach (var res in Reservations)
                    if (res != null && res.TicketId == ticket.Id)
                    {
                        //Reservations.Remove(res);
                        for (int i = 0; i < Reservations.Length; i++)
                        {
                            if (Reservations[i] == res)
                            {
                                Reservations[i] = null;
                                Reservation.Count--;
                                break;
                            }
                        }
                    }
            }
            else
            {
                throw new Exception($"Error, could not delete the {type}\n");
            }

            if (deleted)
            {
                Console.WriteLine($"\n\n{type} has been deleted!{extraMessage}\n");
            }
        }

        // ----------------------------------- Get Object By ----------------------------------- //
        public static Employee GetEmployeeById(int id)
        {
            foreach (var emp in Employees)
            {
                if (emp != null && emp.Id == id)
                    return emp;
            }
            return null;
        }
        public static Visitor GetVisitorById(int id)
        {
            foreach (var vis in Visitors)
            {
                if (vis != null && vis.Id == id)
                    return vis;
            }
            return null;
        }
        public static Ride GetRidebyName(string name)
        {
            foreach (var ride in Rides)
            {
                if (ride != null && ride.Name == name)
                    return ride;
            }
            return null;
        }
        public static Ticket GetTicketById(int id)
        {
            foreach (var tik in Tickets)
            {
                if (tik != null && tik.Id == id)
                    return tik;
            }
            return null;
        }
        public static Reservation GetReservationById(int id)
        {
            foreach (var res in Reservations)
            {
                if (res != null && res.Id == id)
                    return res;
            }
            return null;
        }

        // -----------------------------------  ----------------------------------- //
        ///          ManageRideStatus          ///
        public static void UpdateTicketsStatusByRideName(string name,TicketStatus status)
        {
            foreach(var tik in Tickets)
            {
                if(tik != null && tik.RideName == name)
                {
                    tik.Status = status;
                }
            }
        }
        ///         Create Reservation         ///
        public static bool DidVisitorReserveOnTheSameDateAndTimeInAnotherTicket(int id, DateOnly date, TimeOnly time)
        {
            foreach(var res in Reservations)
            {
                if (res != null && res.VisitorId == id && res.Date == date && res.TimeSlot == time)
                    return true;
            }
            return false;
        }
        /// Create Reservation & CheckReservationCapacityPerRide ///
        public static int CountReservationsPerTimeSlot(string name, TimeOnly timeSlot)
        {
            int count = 0;

            foreach (var reserv in Reservations)
            {
                if (reserv != null && reserv.RideName == name && reserv.TimeSlot == timeSlot)
                    count++;
            }

            return count;
        }
        ///        Hire & Assign Staff        ///
        public static string ListCurrentRidesAndAssignOnes(bool isHiring)
        {
            string rideName = "None";

            if (Rides.All(item => item == null))
                Console.Write("\nCant assign employee, as no rides have been created yet to assign it to him. He has been assigned as \"None\" for now \n");
            else
            {
                string availableRides = "";
                foreach (var ride in Rides)
                {
                    if (ride != null && ride.AssignedEmployeeId == 0) //&& ride.Status == Status.open------------------------------------------------------------------------------------------------------------------------
                        availableRides += $"- {ride.Name}\n";
                }

                if (availableRides == "")
                {
                    if (isHiring)
                        Console.Write("\nCant assign employee, all rides have an employee assiged to it. He has been assigned as \"None\" for now \n");
                    else
                        throw new Exception("\nCant reassign employee, all rides have an employee assiged to it.\n");
                }
                else
                {
                    Console.Write("Available Rides:\n" + availableRides + "Enter the ride name to assign it to the employee (Enter \"None\" if you dont want him assigned to any of those rides): ");
                    rideName = Console.ReadLine();
                    if (availableRides.Contains(rideName) || rideName == "None")
                        return rideName;
                    else
                        throw new Exception("\nThe ride name you provided does not exist\n");
                }
            }
            return "None";
        }
        public static void UpdateAssignedEmployeeInRide(string rideName, int employeeId, TicketStatus newStatus)
        {
            Ride ride = GetRidebyName(rideName);
            ride.AssignedEmployeeId = newStatus == TicketStatus.valid ? employeeId : 0;

            foreach (var tik in Tickets)
            {
                if (tik != null && tik.RideName == rideName)
                    if (newStatus == TicketStatus.invalid) // when setting the employee status as "None"
                        tik.Status = newStatus;
                    else if (tik.Status == TicketStatus.invalid)  // when assigning an employee to a ride that does not have an employee assigned
                        tik.Status = newStatus;
            }
        }
        
        // ------------------------------------- View Data ------------------------------------- //
        public static void ViewVisitorsData()
        {
            bool allNotNull = Visitors.Any(item => item != null);
            if (allNotNull)
            {
                Console.WriteLine($"ID      Age      Height      Accompanying Adults           Tier");
                for (int i = 0; i < Visitors.Length; i++)
                {
                    if (Visitors[i] != null)
                        Console.WriteLine($"{Visitors[i].Id}       {Visitors[i].Age}        {Visitors[i].Height}                {Visitors[i].NumberOfAccompanyingAdults}              {Visitors[i].Tier}");
                }
                Console.WriteLine($"Total: {Visitor.Count}\n");
            }
            else
            {
                Console.WriteLine("No visitors have been registered yet. \n");
            }
        }
        public static void ViewRidesData()
        {
            bool allNotNull = Rides.Any(item => item != null);
            if (allNotNull)
            {
                Console.WriteLine($"Name      AssignedEmployeeId      MinAge      MinHeight           MinimumAccompanyingAdult           Capacity           CurrentCapacity           Type            Status                 Tier");
                for (int i = 0; i < Rides.Length; i++)
                {
                    if (Rides[i] != null)
                        Console.WriteLine($"{Rides[i].Name}                  {Rides[i].AssignedEmployeeId}                {Rides[i].MinAge}           {Rides[i].MinHeight}                         {Rides[i].MinimumAccompanyingAdult}                          {Rides[i].Capacity}                     {Rides[i].CurrentCapacity}                {Rides[i].Type}           {Rides[i].Status}    {Rides[i].Tier}");
                }
                Console.WriteLine($"Total: {Ride.Count}");
            }
            else
            {
                Console.WriteLine("No rides have been added yet. \n");
            }
        }
        public static void ViewTicketsData()
        {
            bool allNotNull = Tickets.Any(item => item != null);
            if (allNotNull)
            {
                Console.WriteLine($"ID      RideName         ExpireDate          Tier               Price           Status");
                for (int i = 0; i < Tickets.Length; i++)
                {
                    if (Tickets[i] != null)
                        Console.WriteLine($"{Tickets[i].Id}        {Tickets[i].RideName}             {Tickets[i].ExpireDate}        {Tickets[i].Tier}          {Tickets[i].Price}           {Tickets[i].Status}");
                }
                Console.WriteLine($"Total: {Ticket.Count}");
            }
            else
            {
                Console.WriteLine("No tickets have been created yet. \n");
            }
        }
        public static void ViewReservationsData()
        {
            bool allNotNull = Reservations.Any(item => item != null);
            if (allNotNull)
            {
                Console.WriteLine($"ID      VisitorId      TicketId       RideName          Date          TimeSlot");
                for (int i = 0; i < Reservations.Length; i++)
                {
                    if (Reservations[i] != null)
                        Console.WriteLine($"{Reservations[i].Id}          {Reservations[i].VisitorId}             {Reservations[i].TicketId}           {Reservations[i].RideName}           {Reservations[i].Date}           {Reservations[i].TimeSlot}");
                }
                Console.WriteLine($"Total: {Reservation.Count}");
            }
            else
            {
                Console.WriteLine("No reservations have been booked yet. \n");
            }
        }
        public static void ViewEmployeesData()
        {
            bool allNotNull = Employees.Any(item => item != null);
            if (allNotNull)
            {
                Console.WriteLine($"ID       Position         Assigned Ride Name      IsAssigned");
                for (int i = 0; i < Employees.Length; i++)
                {
                    if (Employees[i] != null)
                        Console.WriteLine($"{Employees[i].Id}    {Employees[i].Position}    {Employees[i].AssignedRideName}         {Employees[i].IsAssigned}");
                }
                Console.WriteLine($"Total: {Employee.Count}");
            }
            else
            {
                Console.WriteLine("No employees have been hired yet. \n");
            }
        }
    }
}
