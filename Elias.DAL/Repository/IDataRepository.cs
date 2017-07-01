using Elias.DAL.Entities;
using Elias.Shared.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Repository
{
    public partial interface IDataRepository
    {
        /// <summary>
        /// Returns a user entity
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>The user entity or null if it doesn't exist</returns>
        User GetUser(string username);

        SCIPagedList<Employee> GetPagedEmployees(int page, int pageSize, string searchTerm, string sortBy, bool isAsc = true);
    }
}
