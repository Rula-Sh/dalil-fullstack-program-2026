using System.ComponentModel.DataAnnotations;

namespace AirportGroundOperationsManagementSystem
{
    class Program
    {
        /// <summary>
        /// System Rules:
        /// * Flights times in minutes should only be set in quarters (:00, :15, :30, :45)
        /// * The maximum number of bags per passenger is 4
        /// * The Maximum length of a standby list is 15
        /// * The allowed duration to book a fight after a connected flight is one hour
        /// * Once a flight is a assigned on a gate, the gate will be reserved 30m before and 15m after departure time.
        /// * Both gateAgent baggageStaff assignments take 1:30 
        ///   * gateAgent 1:15 before and 0:15 after (to prepare opening and closing the gate)
        ///   * baggageStaff 1:30 before (to make sure all the baggages are loaded before flight)
        /// </summary>
        public static void DisplayMenu()
        {
            Console.WriteLine("======================== Meridian Terminal Operations ========================\n\n" +
                " 1) Flight & Gate Management\n" +
                    "    1.1) Add a gate\n" +
                    "    1.2) Register a new flight\n" +
                    "    1.3) Assign a flight to another gate\n" +
                    "    1.4) Update flight status\n" +
                    "    1.5) View gate reserved durations\n" +
                    "    1.6) View gates\n" +
                    "    1.7) View flights\n" +
                " 2) Passenger & Boarding\n" +
                    "    2.1) Register a passenger\n" +
                    "    2.2) Check passenger booking eligibility\n" +
                    "    2.3) View passenger flights\n" +
                    "    2.4) View passengers\n" +
                " 3) Baggage\n" +
                    "    3.1) Register baggage for a passenger\n" +
                    "    3.2) Check passenger baggage weight limit\n" +
                " 4) Booking & Standby\n" +
                    "    4.1) Book passenger on a flight\n" +
                    "    4.2) Cancel a confirmed booking\n" +
                    "    4.3) View a flight's standby list\n" +
                    "    4.4) View booked passengers in a flight\n" +
                " 5) Staff Management\n" +
                    "    5.1) Hire staff\n" +
                    "    5.2) Assign staff\n" +
                    "    5.3) View a staff duty hours\n" +
                    "    5.4) View staffs\n" +
                " 6) Clear Terminal\n" +
                " 7) Exit");
        }
        static void Main(string[] args)
        {
            new AirportSystem();

            DisplayMenu();

            GroundStaff admin = new GroundStaff(0, Position.admin);

            try
            {
                double input = MainSelectOption();
                while (true) // input != 6 // exit the while loop based on case 6
                {
                    switch (input)
                    {
                        /* -- 1. Flight & Gate Management -- */
                        case 1:
                            Console.WriteLine("Do not select an option that has sub categoris. Please select the options under \"Flight & Gate Management\".\n");
                            break;
                        case 1.1:
                            Console.WriteLine("--------------- 1.1) Add a gate ---------------");
                            admin.RegisterGate();
                            break;
                        case 1.2:
                            Console.WriteLine("--------------- 1.2) Register a new flight ---------------");
                            admin.RegisterFlight();
                            break;
                        case 1.3:
                            Console.WriteLine("----------------- 1.3) Assign a flight to another gate -----------------");
                            admin.getFlightToAssignToGate();
                            break;
                        case 1.4:
                            Console.WriteLine("----------------- 1.4) Update flight status -----------------");
                            admin.updateFlightStatus();
                            break;
                        case 1.5:
                            Console.WriteLine("----------------- 1.5) View gate reserved durations -----------------");
                            admin.ViewGateReservedDurations();
                            break;
                        case 1.6:
                            Console.WriteLine("----------------- 1.6) View gates -----------------");
                            AirportSystem.ListGates();
                            break;
                        case 1.7:
                            Console.WriteLine("----------------- 1.7) View flights -----------------");
                            AirportSystem.ListFlights();
                            break;

                        /* ---- 2. Passenger & Boarding ---- */
                        case 2:
                            Console.WriteLine("Do not select an option that has sub categoris. Please select the options under \"Passenger & Boarding\".\n");
                            break;
                        case 2.1:
                            Console.WriteLine("----------------- 2.1) Register a passenger -----------------");
                            admin.RegisterPassenger();
                            break;
                        case 2.2:
                            Console.WriteLine("--------- 2.2) Check passenger booking eligibility ---------");
                            admin.GetPassengerAndFlightToCheckForPassengerEligibility();
                            break;
                        case 2.3:
                            Console.WriteLine("--------- 2.3) View passenger flights ---------");
                            admin.ViewPassengerFlights();
                            break;
                        case 2.4:
                            Console.WriteLine("--------- 2.4) View passengers ---------");
                            AirportSystem.ListPassengers();
                            break;

                        /* ----------- 3. Baggage ----------- */
                        case 3:
                            Console.WriteLine("Do not select an option that has sub categoris. Please select the options under \"Baggage\".\n");
                            break;
                        case 3.1:
                            Console.WriteLine("----------------- 3.1) Register baggage for a passenger -----------------");
                            admin.getGetPassengerToRegisterBaggage();
                            break;
                        case 3.2:
                            Console.WriteLine("------------- 3.2) Check passenger baggage weight limit -------------");
                            admin.ViewPassengerBaggageWeightLimit();
                            break;

                        /* ------ 4. Booking & Standby ------ */
                        case 4:
                            Console.WriteLine("Do not select an option that has sub categoris. Please select the options under \"Booking & Standby\".\n");
                            break;
                        case 4.1:
                            Console.WriteLine("-------------- 4.1) Book passenger on a flight --------------");
                            admin.GetPassengerAndFlightIDToBookPassenger();
                            break;
                        case 4.2:
                            Console.WriteLine("------ 4.2) Cancel a confirmed booking ------");
                            admin.CancelBooking();
                            break;
                        case 4.3:
                            Console.WriteLine("-------------- 4.3) View a flight's standby list--------------");
                            admin.ViewStandbyList();
                            break;
                        case 4.4:
                            Console.WriteLine("--------------- 4.4) View booked passengers in a flight ---------------");
                            admin.GetFlightToViewItsPassengersList();
                            break;

                        /* ------ 5. Staff Management ------ */
                        case 5:
                            Console.WriteLine("Do not select an option that has sub categoris. Please select the options under \"Staff Management\".\n");
                            break;
                        case 5.1:
                            Console.WriteLine("------------------ 5.1) Hire staff ------------------");
                            admin.HireStaff();
                            break;
                        case 5.2:
                            Console.WriteLine("------------------ 5.2) Assign staff ------------------");
                            admin.AssignStaff();
                            break;
                        case 5.3:
                            Console.WriteLine("----------------- 5.3) View a staff duty hours -----------------");
                            admin.ViewStaffDutyHours();
                            break;
                        case 5.4:
                            Console.WriteLine("----------------- 5.4) View staffs -----------------");
                            AirportSystem.ListGroundStaff();
                            break;

                        /* -------- 6. Clear Terminal-------- */
                        case 6:
                            try
                            {
                                Console.Clear();
                                DisplayMenu();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Couldn't clear the terminal. Please try again later.");
                            }
                            break;

                        /* ------------ 7. Exit ------------ */
                        case 7:
                            // No need to add code here, if the while loop condition is (input != 6) the program will end sideways since there is no code after the while, if i wanted to, i'll need to change the conditio to (true)
                            //Environment.Exit(0); // Ends the application 
                            // OR (to include a confirmation message first)
                            Console.Write("Press \"y\" to confirm exit");
                            if (Console.ReadKey().KeyChar == 'y') { Environment.Exit(0); }
                            else
                            {
                                Console.WriteLine("\nClosing the application has been cancelled.\n");
                            }
                            break;
                        default:
                            Console.WriteLine("\nInvalid input. Please select only one of the following options listed.");
                            break;
                    }
                    input = MainSelectOption();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\nInvalid input. Please make sure to select an option based on number only.");
                MainSelectOption();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static double MainSelectOption()
        {
            Console.WriteLine("\n===================================================================================================\n");
            Console.Write("Select an option: ");
            return double.Parse(Console.ReadLine());
        }

    }
}
