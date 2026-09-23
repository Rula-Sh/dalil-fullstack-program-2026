
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Channels;

namespace ThemeParkManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {            
            new ParkSystem();

            Console.WriteLine("============ Horizon Adventure Park — Operating System ============\n\n" +
                "1) Visitors\n" +
                    "   1.1) Register Visitor\n" +
                    "   1.2) View Visitors\n" +
                "2) Rides\n" +
                    "   2.1) Create a Ride\n" +
                    "   2.2) Validate Visitor Ride Access\n" +
                    "   2.3) Manage Ride Status\n" +
                    "   2.4) Cancel a Ride\n" +
                    "   2.5) View Rides\n" +
                "3) Tickets\n" +
                    "   3.1) Issue Ticket\n" +
                    "   3.2) Manage Ticket Status\n" +
                    "   3.3) Delete Ticket\n" +
                    "   3.4) View Tickets\n" + 
                "4) Reservations\n" +
                    "   4.1) Create Reservation\n" +
                    "   4.2) Check Reservation Capacity Per Ride\n" +
                    "   4.3) Cancel Reservation\n" +
                    "   4.4) View Reservations\n" + 
                "5) Staff\n" +
                    "   5.1) Hire staff\n" +
                    "   5.2) Assign Staff\n" +
                    "   5.3) Check Staff Status\n" +
                    "   5.4) View Employees\n" +
                "6) Exit");

            Employee admin = new Employee(1, Position.Admin, "None");
            Console.WriteLine();
            double input = MainSelectOption();

            while (true) // input != 6 // exit the while loop based on case 6
            {
                switch (input)
                {
                    /* ----- 1. Visitors ----- */
                    case 1:
                        Console.WriteLine("Do not select an option that has sub categoris. Please select the options under Visitors.\n");
                        break;
                    case 1.1: // 1.1) Register Visitor
                        Console.WriteLine("--------------- 1.1) Register Visitor ---------------");
                        admin.CreateVisitor();
                        break; // 1.2) View Visitors
                    case 1.2:
                        Console.WriteLine("----------------- 1.2) View Visitors -----------------");
                        ParkSystem.ViewVisitorsData();
                        break;

                    /* ------ 2. Rides ------ */
                    case 2:
                        Console.WriteLine("Do not select an option that has sub categoris. Please select the options under Rides.\n");
                        break;
                    case 2.1: // 2.1) Create a Ride
                        Console.WriteLine("----------------- 2.1) Create a Ride -----------------");
                        admin.CreateRide();
                        break;
                    case 2.2: // 2.2) Validate Visitor Ride Access
                        Console.WriteLine("--------- 2.2) Validate Visitor Ride Access ---------");
                        if (admin.ValidateRideAccess(false, 0, ""))
                            Console.WriteLine("Result: Visitor can reserve in this ride.\n");
                        break;
                    case 2.3: // 2.3) Manage Ride Status
                        Console.WriteLine("-------------- 2.3) Manage Ride Status --------------");
                        admin.ManageRideStatus();
                        break;
                    case 2.4: // 2.4) Cancel a Ride
                        Console.WriteLine("----------------- 2.4) Cancel a Ride -----------------");
                        admin.CancelRide();
                        break;
                    case 2.5: // 2.5) View Rides
                        Console.WriteLine("------------------ 2.5) View Rides ------------------");
                        ParkSystem.ViewRidesData();
                        break;

                    /* ------ 3. Tickets ------ */
                    case 3:
                        Console.WriteLine("Do not select an option that has sub categoris. Please select the options under Tickets.\n");
                        break;
                    case 3.1: // 3.1) Issue Ticket
                        Console.WriteLine("----------------- 3.1) Issue Ticket -----------------");
                        admin.CreateTicket();
                        break;
                    case 3.2: // 3.2) Manage Ticket Status
                        Console.WriteLine("------------- 3.2) Manage Ticket Status -------------");
                        admin.ManageTicketStatus();
                        break;
                    case 3.3: // 3.3) Delete Ticket
                        Console.WriteLine("----------------- 3.3) Delete Ticket -----------------");
                        admin.DeleteTicket();
                        break;
                    case 3.4: // 3.4) View Ticketss
                        Console.WriteLine("----------------- 3.4) View Ticketss -----------------");
                        ParkSystem.ViewTicketsData();
                        break;

                    /* --- 4. Reservations --- */
                    case 4:
                        Console.WriteLine("Do not select an option that has sub categoris. Please select the options under Reservations.\n");
                        break;
                    case 4.1: // 4.1) Create Reservation
                        Console.WriteLine("-------------- 4.1) Create Reservation --------------");
                        admin.CreateReservation();
                        break;
                    case 4.2: // 4.2) Check Reservation Capacity Per Ride
                        Console.WriteLine("------ 4.2) Check Reservation Capacity Per Ride ------");
                        admin.CheckReservationCapacityPerRide(); 
                        break;
                    case 4.3: // 4.3) Cancel Reservation
                        Console.WriteLine("-------------- 4.3) Cancel Reservation --------------");
                        admin.CancelReservation();
                        break;
                    case 4.4: // 4.4) View Reservations
                        Console.WriteLine("--------------- 4.4) View Reservations ---------------");
                        ParkSystem.ViewReservationsData();
                        break;

                    /* ------ 5. Staff ------ */
                    case 5:
                        Console.WriteLine("Do not select an option that has sub categoris. Please select the options under Staff.\n");
                        break;
                    case 5.1: // 5.1) Hire staff
                        Console.WriteLine("------------------ 5.1) Hire staff ------------------");
                        admin.HireStaff();
                        break;
                    case 5.2: // 5.2) Assign Staff
                        Console.WriteLine("----------------- 5.2) Assign Staff -----------------");
                        admin.AssignStaff();
                        break;
                    case 5.3: // 5.3) Check Staff Status
                        Console.WriteLine("-------------- 5.3) Check Staff Status --------------");
                        admin.CheckStaffStatus();
                        break;
                    case 5.4: // 5.4) View Employees
                        Console.WriteLine("---------------- 5.4) View Employees ----------------");
                        ParkSystem.ViewEmployeesData();
                        break;

                    /* ------- 6. Exit ------- */
                    case 6:
                        // No need to add code here, if the while loop condition is (input != 6) the program will end sideways since there is no code after the while, if i wanted to, i'll need to change the conditio to (true)
                        //Environment.Exit(0); // Ends the application 
                        // OR (to include a confirmation message first)
                        Console.WriteLine("Press \"y\" to confirm exit");
                        if (Console.ReadKey().KeyChar == 'y') { Environment.Exit(0); } else
                        {
                            Console.WriteLine("\nClosing the application has been cancled\n");
                        }
                        break;
                    default:
                        Console.WriteLine("invalid input, Please select only one of the following options listed.");
                        break;
                }
                input = MainSelectOption();
            }
        }

        public static double MainSelectOption()
        {
            try
            {
                Console.WriteLine("===================================================================================================\n");
                Console.Write("Select an option: ");
                double option = double.Parse(Console.ReadLine());
                return option;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please make sure to select an option based on number only.\n");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 0;
        }

    }
}
