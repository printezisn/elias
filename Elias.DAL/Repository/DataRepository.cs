using Elias.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
