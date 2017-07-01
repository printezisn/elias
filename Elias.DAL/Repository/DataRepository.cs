using Elias.DAL.Entities;
using Elias.Shared.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elias.Shared.Extensions;

namespace Elias.DAL.Repository
{
    public class DataRepository : DataRepositoryBase, IDataRepository
    {
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
    }
}
