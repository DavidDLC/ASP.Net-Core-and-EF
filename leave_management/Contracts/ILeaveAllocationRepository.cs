﻿using leave_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        bool CheckAllAllocation(int leaveTypeId, string employeeId);

        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id);
        LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string id,int leaveTypeId );

    }
}
