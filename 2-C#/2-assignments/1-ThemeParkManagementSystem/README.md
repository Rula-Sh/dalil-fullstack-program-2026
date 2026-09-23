# 🎢 Horizon Adventure Park

## 🎯 Project Overview

**Horizon Adventure Park Operations System** is a console-based C# application designed to manage park operations, including visitor registration, ticketing, ride access control, reservations, and staff assignment. The system replaces paper-based processes with a digital solution that enforces business rules and safety requirements.

---

## 🚀 How to Run the Program

1. **Open the Solution**  
   - Navigate to the project folder and double‑click the `.sln` file to open it in Visual Studio.

2. **Initial Setup**  
   - On startup, you are prompted to enter the maximum number of **Visitors**, **Employees**, **Rides**, **Tickets**, and **Reservations** the system will hold.  
   - Enter positive integers for each.

3. **Use the System**  
   - The main menu displays all available operations (see below).  
   - Enter the number (or sub‑number) corresponding to the desired operation.  
   - Follow the on‑screen prompts.  
   - To exit, select option `6` and confirm with `y`.

### 💻 Example Session

```bash
Congratulations for the opening of "Horizon Adventure Park" to get started, please enter the total number of each of the following you want to manage in the park:

Total Visitors: 10
Total Employees: 10
Total Rides: 10
Total Tickets: 10
Total Reservations: 10

============ Horizon Adventure Park - Operating System ============

1) Visitors
   1.1) Register Visitor
   1.2) View Visitors
2) Rides
   2.1) Create a Ride
   2.2) Validate Visitor Ride Access
   2.3) Manage Ride Status
   2.4) Cancel a Ride
   2.5) View Rides
3) Tickets
   3.1) Issue Ticket
   3.2) Manage Ticket Status
   3.3) Delete Ticket
   3.4) View Tickets
4) Reservations
   4.1) Create Reservation
   4.2) Check Reservation Capacity Per Ride
   4.3) Cancel Reservation
   4.4) View Reservations
5) Staff
   5.1) Hire staff
   5.2) Assign Staff
   5.3) Check Staff Status
   5.4) View Employees
6) Exit

===================================================================================================

Select an option: 1.1
--------------- 1.1) Register Visitor ---------------
Enter visitor id: 1001
Enter visitor age: 25
Enter visitor height: 170
Enter visitor number of accompanying adults: 0
Vistor tier: 1- GeneralAdmission
             2- VIP
             3- Child
             4- Senior
             5- Staff Accompanied Minor
Select an option: 1

Visitor has been created!

===================================================================================================

Select an option:
```

---

## ✨ Features Implemented

### 🔹 Functional Requirements

| Feature | Description |
|---------|-------------|
| 👤 **Visitor Registration** | Register new visitors with ID, age, height, number of accompanying adults, and tier category. |
| 🎫 **Ticket Issuance** | Issue a ticket linked to a specific ride, with a matching tier and expiry date. |
| ✅ **Ticket Validation** | Validate a ticket’s status (valid/invalid/expired/cancelled) before creating a reservation. |
| 🎢 **Ride Eligibility Check** | Verify that a visitor meets age, height, accompanying‑adult, and tier requirements for a ride, with specific rejection reasons. |
| 📊 **Ride Capacity Tracking** | Count reservations per time slot and prevent exceeding the ride’s declared capacity. |
| 🗓️ **Ride Reservations** | Create time‑slot reservations; enforce that a visitor cannot have two reservations at the same date/time on different tickets. |
| 🔧 **Ride Status Management** | Update a ride’s status (open, closed, under maintenance). Status changes automatically affect related tickets. |
| 👨‍💼 **Staff Assignment** | Assign employees to rides; prevent assigning an employee who is already assigned elsewhere. |
| ❌ **Cancellation & Deletion** | Cancel reservations or delete tickets/rides with cascading removal of dependent entities. |

---

### 🔸 Detailed Business Rules Enforced

- **Eligibility Rules** – Each ride defines minimum age, height, and accompanying‑adult requirements. Failure of any criterion returns the specific reason.
- **Capacity Rules** – Both reservations and (future) admissions are checked against the ride’s capacity; no more than `Capacity` reservations are allowed for the same time slot.
- **Access Tier Rules** – Visitors with `VIP` tier can access any ride regardless of the ride’s tier; other visitors must have a matching tier.
- **Staff Assignment Rules** – An employee cannot be assigned to more than one ride at the same time.
- **Ride Status Rules** – No reservations are allowed on a ride that is `closed` or `underMaintenance`.
- **Duplicate Prevention** – Duplicate visitor IDs, ticket IDs, and reservations for the same visitor/time slot are rejected.
- **Ticket Auto‑Status Rules** – Ticket status is automatically managed: `valid` when the linked ride is `open` and has an assigned employee; `invalid` when the ride is `closed`/`underMaintenance` or lacks an assigned employee; `expired` when the expiry date passes.

- **Cascading Deletion Rules** – Deleting a ride automatically deletes all associated tickets and reservations. Deleting a ticket automatically deletes all associated reservations.

### 🔹 Main Menu Operations

**👥 Visitor & Ticketing:**
- Register a new visitor
- View all visitors

**🎢 Ride Management:**
- Create a new ride
- Validate visitor's access to a specific ride 
- Manage ride operational status
- Cancel a ride with cascading deletion
- View all rides 

**🎫 Ticket Management:**
- Issue a ticket to a visitor
- Manage ticket status (cancel/reactivate)
- Delete a ticket
- View all tickets

**📅 Reservation Management:**
- Create a ride reservation
- Check reservation capacity per ride
- Cancel a ride reservation
- View all reservations 

**👨‍💼 Staff Management:**
- Hire new staff 
- Assign staff to rides
- Check staff status
- View all employees

---

## 🛡️ Validation & Error Handling

The system implements comprehensive validation across all operations with clear, user-friendly error messages.

### Input Validation

| Category | Validation Rules |
|----------|------------------|
| **General Input** | • Non-numeric input handled with `FormatException`<br>• Empty/null input rejected with clear message<br>• Out-of-range values rejected with specific reason<br>• Category selection without sub-option prompts correct navigation |
| **System Limits** | • Array capacity validation prevents exceeding allocated slots<br>• Entity existence verified before operations<br>• Duplicate entries prevented across all entity types |

### Entity Validation Rules

| Entity | Key Validations |
|--------|-----------------|
| **Visitor** | Age (0-100), Height (45-250cm), Accompanying Adults (0-2), Tier (1-5), Duplicate ID prevention |
| **Ride** | Name uniqueness, Min Age (6-20), Min Height (80-160cm), Min Adults (0-2), Type (1-3), Status (1-3), Tier (1-5), Employee assignment validation |
| **Ticket** | ID uniqueness, Ride existence, Tier matching with ride, Future expiry date, Auto-status management based on ride availability |
| **Ride Access** | Visitor/Ride existence, Age/Height requirements, Accompanying adult check, Tier access (VIP = all access), Ride must be open |
| **Reservation** | All entities exist, Ticket matches ride, Valid ticket status (not invalid/expired/cancelled), Future date, Within ticket expiry, No duplicate time-slot per visitor, Capacity limits respected |
| **Staff Management** | Employee exists, Not already assigned, Ride exists, Assignment conflict prevention |

### Error Messaging

The system provides **specific, actionable feedback** for every failure scenario rather than generic errors:

- **Eligibility failures**: "Visitor does not meet minimum height requirement (110cm) for this ride"
- **Capacity failures**: "Ride has reached maximum capacity (30) for the selected time slot"
- **Status failures**: "The ride is currently under maintenance"
- **Ticket failures**: "The provided ticket ID has expired since 2024-12-31"
- **Assignment failures**: "Employee is already assigned to another ride"

This approach ensures users understand exactly why an operation failed and what action to take.

## 🏗️ High-Level Architecture

The system follows a simple object-oriented design with the following core entities:

- **ParkSystem**: Central static container managing all data arrays and providing CRUD operations
- **Visitor**: Represents park guests with tier-based categorization
- **Employee**: Staff members with roles and ride assignments
- **Ride**: Attractions with safety requirements, capacity limits, and operational status
- **Ticket**: Access credentials linked to specific rides with validity tracking
- **Reservation**: Time-slot bookings linked to visitors, tickets, and rides

**Key Relationships:**
- Visitors have Tickets
- Tickets are linked to specific Rides
- Reservations require valid Visitors and Tickets
- Employees can be assigned to Rides
- Ride access is validated against Visitor eligibility rules

The console interface provides full CRUD operations for most entities with comprehensive validation and error handling.

---

## 🔢 Enumerations

| Enum | Values |
|------|--------|
| `Tier` | GeneralAdmission, VIP, Child, Senior, StaffAccompaniedMinor |
| `Position` | TicketBoothStaff, RideOperator, OperationsManager, Admin |
| `RideType` | Thrill, Family, Water |
| `Status` | Open, Closed, UnderMaintenance |
| `TicketStatus` | Valid, Invalid, Expired, Cancelled |

---

## ✅ Test Cases Covered

The system gracefully handles all specified error scenarios:

| # | Test Case | Status |
|---|-----------|--------|
| 1 | Non-numeric input entry | ✅ |
| 2 | Visitor with no ticket or invalid/expired/cancelled ticket | ✅ |
| 3 | Ride at full capacity | ✅ |
| 4 | Visitor not meeting eligibility requirements | ✅ |
| 5 | Operations on non-existent rides | ✅ |
| 6 | Reservations/admissions to closed/under maintenance rides | ✅ |
| 7 | Reusing expired/cancelled tickets | ✅ |
| 8 | Assigning employee to conflicting rides | ✅ |
| 9 | Specific rejection reasons for eligibility failures | ✅ |
| 10 | Clear and meaningful error messages | ✅ |
---

## 🚀 Future Enhancements

- **Time‑slot granularity** – Restrict reservation minutes to `00` or `30` (e.g., 10:00, 10:30, 11:00) to standardise ride scheduling.
- **Per‑time‑slot staff assignment** – Allow employees to be assigned to rides based on specific time slots or shifts, rather than globally for the entire day.
- **Role‑based access control (RBAC)** – Restrict operations based on the employee's position (e.g., only `OperationsManager` can update ride status or view reports).

---
