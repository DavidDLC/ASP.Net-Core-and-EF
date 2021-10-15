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
            CreateMap<LeaveType, LeaveTypeVM>().ReverseMap();
            CreateMap<Employee, EmployeeVM>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestVM>().ReverseMap();
            CreateMap<LeaveRequest, AdminLeaveRequestViewVM>().ReverseMap();
            CreateMap<LeaveRequest, CreateLeaveRequestVM>().ReverseMap();
            CreateMap<LeaveRequest, EmployeeLeaveRequestViewVM>().ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationVM>().ReverseMap();
        }
    }
}
