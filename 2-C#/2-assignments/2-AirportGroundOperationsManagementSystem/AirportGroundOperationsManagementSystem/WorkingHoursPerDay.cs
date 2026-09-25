using System;
using System.Collections.Generic;
using System.Text;

namespace AirportGroundOperationsManagementSystem
{
    public class WorkingHoursPerDay
    {
        public DateOnly Date { get; set; }
        public TimeSpan WorkedDutyHours { get; set; }
        public WorkingHoursPerDay(DateOnly date, TimeSpan workedDutyHours)
        {
            Date = date;
            WorkedDutyHours = workedDutyHours;
        }
    }
}
