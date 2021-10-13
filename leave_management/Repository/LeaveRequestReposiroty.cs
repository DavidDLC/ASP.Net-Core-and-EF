using leave_management.Contracts;
using leave_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repository
{
    public class LeaveRequestReposiroty : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestReposiroty(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(LeaveRequest entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(LeaveRequest entity)
        {
            throw new NotImplementedException();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            throw new NotImplementedException();
        }

        public LeaveRequest FindById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(LeaveRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
