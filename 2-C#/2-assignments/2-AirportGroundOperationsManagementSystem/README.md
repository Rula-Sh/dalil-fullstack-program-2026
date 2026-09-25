# ✈️ Meridian Terminal Operations System

## 📋 Project Overview

**Meridian Terminal Operations System** is a console-based C# application designed to manage ground-level airport operations for a single terminal. The system handles flight registration, gate assignment, passenger boarding, baggage handling, and staff management. It addresses real-world operational challenges including overbooked flights with standby lists, connecting passengers with minimum connection-time requirements, cumulative baggage weight tracking per passenger, and staff duty-hour limits to prevent over-assignment. By enforcing these business rules and safety requirements digitally.

---

## 🚀 How to Run the Program

1. **Open the Solution**
   - Navigate to the project folder and double‑click the `.sln` file to open it in Visual Studio.

2. **Initial Setup**
   - The system starts with an admin account automatically created (ID: 0, Position: Admin).
   - No additional setup is required—data lives only for the duration of the running session.

3. **Use the System**
   - The main menu displays all available operations (see below).
   - Enter the number (or sub‑number) corresponding to the desired operation.
   - Follow the on‑screen prompts.
   - To exit, select option `7` and confirm with `y`.

### 💻 Example Session

```bash
======================== Meridian Terminal Operations ========================

 1) Flight & Gate Management
    1.1) Add a gate
    1.2) Register a new flight
    1.3) Assign a flight to another gate
    1.4) Update flight status
    1.5) View gate reserved durations
    1.6) View gates
    1.7) View flights
 2) Passenger & Boarding
    2.1) Register a passenger
    2.2) Check passenger booking eligibility
    2.3) View passenger flights
    2.4) View passengers
 3) Baggage
    3.1) Register baggage for a passenger
    3.2) Check passenger baggage weight limit
 4) Booking & Standby
    4.1) Book passenger on a flight
    4.2) Cancel a confirmed booking
    4.3) View a flights standby list
    4.4) View booked passengers in a flight
 5) Staff Management
    5.1) Hire staff
    5.2) Assign staff
    5.3) View a staff duty hours
    5.4) View staffs
 6) Clear Terminal
 7) Exit

===================================================================================================

Select an option: 1.1
--------------- 1.1) Add a gate ---------------
Enter gate id: 1
Does it support international flights? Type "y" to confirm. y

Success: Gate has been added to the system.

===================================================================================================

Select an option: 1.2
--------------- 1.2) Register a new flight ---------------
Enter Flight id: 101
Enter departure location: New York
Enter destination location: London
Enter flight duration (hh:mm): 07:30
Is this flight international? Type "y" to confirm. y
Enter seats capacity: 150
Enter staff members needed for this flight (both baggage staffs and gate agents): 4
Enter the maximum baggage weight (in kg) per passenger: 30
Enter departure date (yyyy/mm/dd): 2026/09/15
Enter departure time in 24h format (hh:mm): 10:00
Available gates:
- 1 (Supports International Flights)
- 2 (Does not Support International Flights)
Enter the gate ID to assign the flight to it (Enter "None" to cancel): 1

Success: Flight has been added to the system.

The flight has been scheduled to departure on 9/15/2026 at 10:00 AM from "New York" and land on "London" at 5:30 PM.
Also, The Flight with ID 101 is now assiged to gate 1, the gate will be reserved between 9:30 AM and 10:15 AM on 9/15/2026

===================================================================================================

Select an option: 5.1
------------------ 5.1) Hire staff ------------------
Enter staff id: 1001
Position name: 1- gateAgent
               2- baggageStaff
Enter position name: 1

Success: GroundStaff has been added to the system.

===================================================================================================

Select an option:
```

## ✨ Features Implemented

### 🔹 Functional Requirements

| Feature                             | Description                                                                                                                                                                            |
| ----------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 🛫 **Flight Registration**          | Register new flights with type (domestic/international), schedule, seat capacity, and staff requirements.                                                                              |
| 🚪 **Gate Assignment**              | Assign flights to gates with conflict prevention for overlapping time windows.                                                                                                         |
| 🛂 **Passenger Registration**       | Register passengers with category (standard, VIP, reduced-mobility) and optional connecting flight link.                                                                               |
| ⏱️ **Connection Time Verification** | Verify that connecting passengers have sufficient time between arrival and departure (minimum 45 minutes).                                                                             |
| 🧳 **Cumulative Baggage Tracking**  | Register multiple bags per passenger and track combined weight against flight allowance.                                                                                               |
| 📋 **Booking & Standby Management** | Book passengers onto flights with automatic standby list booking when seats become available.                                                                                          |
| 👨‍💼 **Staff Management**             | Hire and assign staff to flights with duty-hour tracking and conflict prevention.                                                                                                      |
| 📊 **Staff Duty-Hour Tracking**     | Track cumulative duty hours per staff member and prevent over-assignment (9-hour shift limit).                                                                                         |
| 🔄 **Flight Status Management**     | The system automatically updates status to boarding (30 minutes before departure) and departed (once departure time passes). Manual updates are allowed only for delayed or cancelled. |

---

### 🔸 Detailed Business Rules Enforced

- **Gate Assignment Rules** – A flight occupies a gate from 30 minutes before departure until 15 minutes after departure. Gate conflicts are prevented for overlapping time windows.
- **Connection Time Rules** – Connecting passengers must have at least 45 minutes between their arrival and next departure.
- **Cumulative Baggage Rules** – Passengers may check multiple bags, but the combined weight cannot exceed the flight's per-passenger allowance.
- **Standby Rules** – When a flight reaches seat capacity, passengers are added to a standby list (max 15). When a confirmed booking is cancelled, the earliest standby passenger is automatically promoted.
- **Staff Assignment Rules** – Staff cannot be assigned to overlapping assignments. Each staff member has a 9-hour daily shift limit.
- **Staff Duty-Hour Rules** – Each assignment adds 1.5 hours to a staff member's duty hours (gate agents: 0:30 before + 0:15 after; baggage staff: 1:30 before).
- **Flight Status Rules** – Boarding and baggage operations are prevented if the flight has departed or been cancelled. Status transitions are automatic for boarding (0 minutes before departure) and departed (once departure time passes), and manual only for delayed or cancelled.
- **Time Format Rules** – Flight times must be set in quarters (:00, :15, :30, :45).
- **Duplicate Prevention** – Duplicate IDs for flights, gates, passengers, and staff are rejected.

### 🔹 Main Menu Operations

**🛫 Flight & Gate Management:**

- Add a gate
- Register a new flight
- Assign a flight to another gate
- Update flight status
- View gate reserved durations
- View gates
- View flights

**👤 Passenger & Boarding:**

- Register a passenger
- Check passenger booking eligibility
- View passenger flights
- View passengers

**🧳 Baggage:**

- Register baggage for a passenger
- Check passenger baggage weight limit

**📋 Booking & Standby:**

- Book passenger on a flight
- Cancel a confirmed booking
- View a flight's standby list
- View booked passengers in a flight

**👨‍💼 Staff Management:**

- Hire staff
- Assign staff
- View a staff duty hours
- View staffs

---

## 🛡️ Validation & Error Handling

The system implements comprehensive validation across all operations with clear, user-friendly error messages.

### Input Validation

| Category          | Validation Rules                                                                                                                                                                                                                  |
| ----------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **General Input** | • Non-numeric input handled with `FormatException`<br>• Empty/null input rejected with clear message<br>• Out-of-range values rejected with specific reason<br>• Category selection without sub-option prompts correct navigation |
| **System Limits** | • Entity existence verified before operations<br>• Duplicate entries prevented across all entity types                                                                                                                            |

### Entity Validation Rules

| Entity        | Key Validations                                                                                                  |
| ------------- | ---------------------------------------------------------------------------------------------------------------- |
| **Flight**    | ID uniqueness, Valid duration (≥30 min), Date in future, Time in quarters, Valid gate assignment, Staff count ≥2 |
| **Gate**      | ID uniqueness, International flight support validation, No overlapping assignments                               |
| **Passenger** | ID uniqueness, Name validation, Type selection (1-3), Connecting flight validation                               |
| **Baggage**   | Weight validation, Illegal content check, Cumulative weight limit enforcement, Max 4 bags per passenger          |
| **Staff**     | ID uniqueness, Position selection (gateAgent/baggageStaff), Assignment conflict prevention, 9-hour shift limit   |
| **Booking**   | Flight availability, Passenger eligibility, Connecting time (≥45 min), Document validity check                   |

### Error Messaging

The system provides **specific, actionable feedback** for every failure scenario rather than generic errors:

- **Gate conflicts**: "Failed! The gate you selected does not support international flights"
- **Connection failures**: "Connecting Failed! The time gap between the booked flight landing time (2:00) and the next flight departure time (2:30) is less than 45m."
- **Baggage failures**: "Failed! The passenger would reach the bagges weight to 35kg exceeding the limit 30kg."
- **Capacity failures**: "Boarding Denied: The flight has reached its maximum capacity, but the passenger can be added in the standby list."
- **Staff assignment failures**: "Failed! The staff is already assigned to another flight at that time."

This approach ensures users understand exactly why an operation failed and what action to take.

## 🏗️ High-Level Architecture

The system follows a simple object-oriented design with the following core entities:

- **AirportSystem**: Central static container managing all data lists and providing CRUD operations
- **Flight**: Represents scheduled flights with type, schedule, capacity, and status
- **Gate**: Airport gates with international support and booking schedules
- **Passenger**: Travelers with categories and optional connecting flight links
- **Baggage**: Checked luggage with weight tracking per passenger
- **GroundStaff**: Employees with positions (gateAgent, baggageStaff) and duty-hour tracking
- **StandbyList**: Waiting list for overbooked flights with automatic promotion
- **WorkingHoursPerDay**: Tracks staff cumulative duty hours per day

**Key Relationships:**

- Flights are assigned to Gates
- Passengers book Flights and check Baggage
- GroundStaff are assigned to Flights and Gates
- StandbyList tracks passengers waiting for available seats
- WorkingHoursPerDay tracks staff assignments and duty hours

The console interface provides full CRUD operations for most entities with comprehensive validation and error handling.

---

## 🔢 Enumerations

| Enum            | Values                                       |
| --------------- | -------------------------------------------- |
| `Position`      | gateAgent, baggageStaff, admin               |
| `FlightStatus`  | Idle, boarding, delayed, departed, cancelled |
| `PassengerType` | standard, VIP, reducedMobility               |

---

## ✅ Test Cases Covered

The system gracefully handles all specified error scenarios:

| #   | Test Case                                              | Status |
| --- | ------------------------------------------------------ | ------ |
| 1   | Non-numeric input entry                                | ✅     |
| 2   | Flight assigned to overlapping gate time window        | ✅     |
| 3   | Connecting passenger with insufficient connection time | ✅     |
| 4   | Baggage exceeding cumulative weight allowance          | ✅     |
| 5   | Booking onto full flight with full standby list        | ✅     |
| 6   | Staff assignment exceeding 9-hour shift limit          | ✅     |
| 7   | Automatic standby booking on cancellation              | ✅     |
| 8   | Boarding or baggage on departed/cancelled flight       | ✅     |
| 9   | Specific rejection reasons for eligibility failures    | ✅     |
| 10  | Clear and meaningful error messages                    | ✅     |

---

## 🔧 System Rules

| Rule                        | Value                                  |
| --------------------------- | -------------------------------------- |
| Flight time increments      | :00, :15, :30, :45                     |
| Maximum bags per passenger  | 5                                      |
| Maximum standby list length | 15                                     |
| Minimum connection time     | 45 minutes                             |
| Gate reservation window     | 30 min before / 15 min after departure |
| Staff assignment duration   | 1 hour 30 minutes                      |
| Gate agent window           | 1:15 before / 0:15 after departure     |
| Baggage staff window        | 1:30 before departure                  |
| Maximum daily shift hours   | 9 hours                                |

---

## 🚀 Future Enhancements

- **Role-Based Access Control (RBAC)** – Restrict operations based on staff position (e.g., only admin can register flights, gate agents handle boarding).
- **Advanced Reporting** – Generate operational reports for shift analysis and performance metrics.
- **Real-time Flight Tracking** – Integrate with external systems for live flight status updates.
- **Excess Baggage Fee Handling** – Allow passengers to exceed the flight’s standard baggage allowance by paying an additional fee.
