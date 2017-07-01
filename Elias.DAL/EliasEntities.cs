using Elias.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL
{
    public class EliasEntities : DbContext
    {
        public EliasEntities()
            : base("EliasConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; }
    }
}
