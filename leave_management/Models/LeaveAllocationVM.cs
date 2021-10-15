using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Models
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        public int NumberOfdays { get; set; }

        public DateTime DateCreated { get; set; }

        public int Period { get; set; }

        public EmployeeVM Employee { get; set; }

        public string EmployeeId { get; set; }

        public LeaveTypeVM LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

    }

    public class CreateLeaveAllocationVM
    {
        public int NumberUpdated { get; set; }

        public List<LeaveTypeVM> LeaveType { get; set; }
    }
}
