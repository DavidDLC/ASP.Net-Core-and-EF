using AutoMapper;
using leave_management.Data;
using leave_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Mappings
{
    public class Maps : Profile
    {
        public Maps() 
        {
            CreateMap<LeaveType, DetailsLeaveTypeVM>().ReverseMap();
            CreateMap<LeaveType, CreateLeaveTypeVM>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestVM>();
            CreateMap<LeaveRequest, AdminLeaveRequestViewVM>();
            CreateMap<LeaveRequest, CreateLeaveRequestVM>();
            CreateMap<LeaveRequest, EmployeeLeaveRequestViewVM>();
            CreateMap<LeaveAllocation, LeaveAllocationVM>();
        }
    }
}
