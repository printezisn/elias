using Elias.DAL.Entities;
using Elias.Shared.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elias.Shared.Extensions;
using System.Data.Entity;

namespace Elias.DAL.Repository
{
    public class DataRepository : DataRepositoryBase, IDataRepository
    {
        protected override IQueryable<Employee> SearchEmployeesQuery(IQueryable<Employee> query, string searchTerm)
        {
            return query.Where(w => (w.FirstName != null && w.FirstName.Contains(searchTerm)) || (w.LastName != null && w.LastName.Contains(searchTerm)) || (w.Email != null && w.Email.Contains(searchTerm)));
        }

        /// <summary>
        /// Returns a user entity
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>The user entity or null if it doesn't exist</returns>
        public User GetUser(string username)
        {
            return Db.Users.FirstOrDefault(f => f.Username == username);
        }

        public SCIPagedList<Employee> GetPagedEmployees(int page, int pageSize, string searchTerm, string sortBy, bool isAsc = true)
        {
            var query = string.IsNullOrWhiteSpace(searchTerm) ? GetEmployees() : SearchEmployees(searchTerm);
            query = query.OrderBy(sortBy, isAsc).ThenBy(o => o.Id);

            return new SCIPagedList<Employee>(query, page, pageSize, searchTerm, sortBy, isAsc);
        }

        public SCIPagedList<LeaveRequest> GetPagedLeaveRequests(int page, int pageSize, string searchTerm, string sortBy, bool isAsc = true)
        {
            var query = Db.LeaveRequests.Include(i => i.Employee).Include(i => i.Status);
            query = string.IsNullOrWhiteSpace(searchTerm)
                ? query
                : query.Where(w => (w.Employee.FirstName + " " + w.Employee.LastName).Contains(searchTerm));
            query = query.OrderBy(sortBy, isAsc).ThenBy(o => o.Id);

            return new SCIPagedList<LeaveRequest>(query, page, pageSize, searchTerm, sortBy, isAsc);
        }
    }
}
