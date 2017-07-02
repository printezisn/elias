

using System;
using System.Linq;
using Elias.DAL.Entities;
using System.Collections.Generic;

namespace Elias.DAL.Repository
{
	public partial interface IDataRepository : IDisposable
	{
		/// <summary>
        /// Saves the current changes
        /// </summary>
		void Save();

				#region Employees

		/// <summary>
        /// Returns a query for Employees
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		IQueryable<Employee> GetEmployees(bool includeTracking = false);
		
		/// <summary>
        /// Returns a search query for Employees
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		IQueryable<Employee> SearchEmployees(string searchTerm);
		
		/// <summary>
        /// Returns a Employee entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		Employee GetEmployee(int id);

		/// <summary>
        /// Adds a new Employee entity
        /// </summary>
        /// <param name="model">The Employee entity to add</param>
		void Add(Employee model);

		/// <summary>
        /// Deletes a Employee entity
        /// </summary>
        /// <param name="model">The Employee entity to delete</param>
		void Delete(Employee model);

		#endregion

				#region LeaveRequests

		/// <summary>
        /// Returns a query for LeaveRequests
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		IQueryable<LeaveRequest> GetLeaveRequests(bool includeTracking = false);
		
		/// <summary>
        /// Returns a LeaveRequest entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		LeaveRequest GetLeaveRequest(System.Guid id);

		/// <summary>
        /// Adds a new LeaveRequest entity
        /// </summary>
        /// <param name="model">The LeaveRequest entity to add</param>
		void Add(LeaveRequest model);

		/// <summary>
        /// Deletes a LeaveRequest entity
        /// </summary>
        /// <param name="model">The LeaveRequest entity to delete</param>
		void Delete(LeaveRequest model);

		#endregion

				#region LeaveRequestStatuses

		/// <summary>
        /// Returns a query for LeaveRequestStatuses
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		IQueryable<LeaveRequestStatus> GetLeaveRequestStatuses(bool includeTracking = false);
		
		/// <summary>
        /// Returns a search query for LeaveRequestStatuses
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		IQueryable<LeaveRequestStatus> SearchLeaveRequestStatuses(string searchTerm);
		
		/// <summary>
        /// Returns a LeaveRequestStatus entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		LeaveRequestStatus GetLeaveRequestStatus(byte id);

		/// <summary>
        /// Adds a new LeaveRequestStatus entity
        /// </summary>
        /// <param name="model">The LeaveRequestStatus entity to add</param>
		void Add(LeaveRequestStatus model);

		/// <summary>
        /// Deletes a LeaveRequestStatus entity
        /// </summary>
        /// <param name="model">The LeaveRequestStatus entity to delete</param>
		void Delete(LeaveRequestStatus model);

		#endregion

				#region Users

		/// <summary>
        /// Returns a query for Users
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		IQueryable<User> GetUsers(bool includeTracking = false);
		
		/// <summary>
        /// Returns a search query for Users
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		IQueryable<User> SearchUsers(string searchTerm);
		
		/// <summary>
        /// Returns a User entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		User GetUser(System.Guid id);

		/// <summary>
        /// Adds a new User entity
        /// </summary>
        /// <param name="model">The User entity to add</param>
		void Add(User model);

		/// <summary>
        /// Deletes a User entity
        /// </summary>
        /// <param name="model">The User entity to delete</param>
		void Delete(User model);

		#endregion

		
		/// <summary>
        /// Deletes all the data from the database. CAUTION: Only used for integration tests
        /// </summary>
        void ResetDatabase();
	}

	public abstract class DataRepositoryBase
	{
		private Elias.DAL.EliasEntities _db;
        protected Elias.DAL.EliasEntities Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new Elias.DAL.EliasEntities();
                    _db.Configuration.ValidateOnSaveEnabled = false;
                }

                return _db;
            }
        }

		/// <summary>
        /// Disposes the repository
        /// </summary>
        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

		/// <summary>
        /// Saves the current changes
        /// </summary>
        public void Save()
        {
            if (_db != null)
            {
                _db.SaveChanges();
            }
        }

		/// <summary>
        /// Deletes all the data from the database. CAUTION: Only used for integration tests
        /// </summary>
        public void ResetDatabase()
		{
					Db.Employees.RemoveRange(Db.Employees.ToList());
					Db.LeaveRequests.RemoveRange(Db.LeaveRequests.ToList());
					Db.LeaveRequestStatuses.RemoveRange(Db.LeaveRequestStatuses.ToList());
					Db.Users.RemoveRange(Db.Users.ToList());
					Save();
		}

				#region Employees

		/// <summary>
        /// Returns a query for Employees
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<Employee> GetEmployees(bool includeTracking = false)
		{
						if (includeTracking)
            {
                return Db.Employees;
            }

            return Db.Employees.AsNoTracking();
					}

		
		/// <summary>
        /// Applies a search query, for Employees, to an existing query
        /// </summary>
		/// <param name="query">The query to apply the search to</param>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		protected virtual IQueryable<Employee> SearchEmployeesQuery(IQueryable<Employee> query, string searchTerm)
		{
			return query.Where(w => (w.FirstName != null && w.FirstName.Contains(searchTerm)) || (w.LastName != null && w.LastName.Contains(searchTerm)) || (w.Email != null && w.Email.Contains(searchTerm)) || (w.ActivationCode != null && w.ActivationCode.Contains(searchTerm)) || (w.SkypeId != null && w.SkypeId.Contains(searchTerm)) || (w.FacebookId != null && w.FacebookId.Contains(searchTerm)) || (w.ServiceUrl != null && w.ServiceUrl.Contains(searchTerm)) || (w.LastUsedId != null && w.LastUsedId.Contains(searchTerm)) || (w.BotId != null && w.BotId.Contains(searchTerm)));
		}

		/// <summary>
        /// Returns a search query for Employees
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		public virtual IQueryable<Employee> SearchEmployees(string searchTerm)
		{
			return SearchEmployeesQuery(GetEmployees(), searchTerm);
		}

		
		/// <summary>
        /// Returns a Employee entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual Employee GetEmployee(int id)
		{
					return Db.Employees.FirstOrDefault(f => f.Id == id);
				}

		/// <summary>
        /// Adds a new Employee entity
        /// </summary>
        /// <param name="model">The Employee entity to add</param>
		public virtual void Add(Employee model)
		{
			Db.Employees.Add(model);
		}

		/// <summary>
        /// Deletes a Employee entity
        /// </summary>
        /// <param name="model">The Employee entity to delete</param>
		public virtual void Delete(Employee model)
		{
					Db.Employees.Remove(model);
				}

		#endregion

				#region LeaveRequests

		/// <summary>
        /// Returns a query for LeaveRequests
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<LeaveRequest> GetLeaveRequests(bool includeTracking = false)
		{
						if (includeTracking)
            {
                return Db.LeaveRequests;
            }

            return Db.LeaveRequests.AsNoTracking();
					}

		
		/// <summary>
        /// Returns a LeaveRequest entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual LeaveRequest GetLeaveRequest(System.Guid id)
		{
					return Db.LeaveRequests.FirstOrDefault(f => f.Id == id);
				}

		/// <summary>
        /// Adds a new LeaveRequest entity
        /// </summary>
        /// <param name="model">The LeaveRequest entity to add</param>
		public virtual void Add(LeaveRequest model)
		{
			Db.LeaveRequests.Add(model);
		}

		/// <summary>
        /// Deletes a LeaveRequest entity
        /// </summary>
        /// <param name="model">The LeaveRequest entity to delete</param>
		public virtual void Delete(LeaveRequest model)
		{
					Db.LeaveRequests.Remove(model);
				}

		#endregion

				#region LeaveRequestStatuses

		/// <summary>
        /// Returns a query for LeaveRequestStatuses
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<LeaveRequestStatus> GetLeaveRequestStatuses(bool includeTracking = false)
		{
						if (includeTracking)
            {
                return Db.LeaveRequestStatuses;
            }

            return Db.LeaveRequestStatuses.AsNoTracking();
					}

		
		/// <summary>
        /// Applies a search query, for LeaveRequestStatuses, to an existing query
        /// </summary>
		/// <param name="query">The query to apply the search to</param>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		protected virtual IQueryable<LeaveRequestStatus> SearchLeaveRequestStatusesQuery(IQueryable<LeaveRequestStatus> query, string searchTerm)
		{
			return query.Where(w => (w.Name != null && w.Name.Contains(searchTerm)));
		}

		/// <summary>
        /// Returns a search query for LeaveRequestStatuses
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		public virtual IQueryable<LeaveRequestStatus> SearchLeaveRequestStatuses(string searchTerm)
		{
			return SearchLeaveRequestStatusesQuery(GetLeaveRequestStatuses(), searchTerm);
		}

		
		/// <summary>
        /// Returns a LeaveRequestStatus entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual LeaveRequestStatus GetLeaveRequestStatus(byte id)
		{
					return Db.LeaveRequestStatuses.FirstOrDefault(f => f.Id == id);
				}

		/// <summary>
        /// Adds a new LeaveRequestStatus entity
        /// </summary>
        /// <param name="model">The LeaveRequestStatus entity to add</param>
		public virtual void Add(LeaveRequestStatus model)
		{
			Db.LeaveRequestStatuses.Add(model);
		}

		/// <summary>
        /// Deletes a LeaveRequestStatus entity
        /// </summary>
        /// <param name="model">The LeaveRequestStatus entity to delete</param>
		public virtual void Delete(LeaveRequestStatus model)
		{
					Db.LeaveRequestStatuses.Remove(model);
				}

		#endregion

				#region Users

		/// <summary>
        /// Returns a query for Users
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<User> GetUsers(bool includeTracking = false)
		{
						if (includeTracking)
            {
                return Db.Users;
            }

            return Db.Users.AsNoTracking();
					}

		
		/// <summary>
        /// Applies a search query, for Users, to an existing query
        /// </summary>
		/// <param name="query">The query to apply the search to</param>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		protected virtual IQueryable<User> SearchUsersQuery(IQueryable<User> query, string searchTerm)
		{
			return query.Where(w => (w.Username != null && w.Username.Contains(searchTerm)) || (w.Email != null && w.Email.Contains(searchTerm)) || (w.Password != null && w.Password.Contains(searchTerm)) || (w.PasswordSalt != null && w.PasswordSalt.Contains(searchTerm)));
		}

		/// <summary>
        /// Returns a search query for Users
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		public virtual IQueryable<User> SearchUsers(string searchTerm)
		{
			return SearchUsersQuery(GetUsers(), searchTerm);
		}

		
		/// <summary>
        /// Returns a User entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual User GetUser(System.Guid id)
		{
					return Db.Users.FirstOrDefault(f => f.Id == id);
				}

		/// <summary>
        /// Adds a new User entity
        /// </summary>
        /// <param name="model">The User entity to add</param>
		public virtual void Add(User model)
		{
			Db.Users.Add(model);
		}

		/// <summary>
        /// Deletes a User entity
        /// </summary>
        /// <param name="model">The User entity to delete</param>
		public virtual void Delete(User model)
		{
					Db.Users.Remove(model);
				}

		#endregion

			}

	public abstract class FakeDataRepositoryBase
	{
		/// <summary>
        /// Saves the current changes
        /// </summary>
		public void Save()
        {

        }

		/// <summary>
        /// Disposes the repository
        /// </summary>
        public void Dispose()
        {

        }

		/// <summary>
        /// Deletes all the data from the database. CAUTION: Only used for integration tests
        /// </summary>
        public void ResetDatabase()
		{

		}

				#region Employees

		protected List<Employee> Employees = new List<Employee>();

		/// <summary>
        /// Returns a query for Employees
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<Employee> GetEmployees(bool includeTracking = false)
		{
			return this.Employees.AsQueryable();
		}

		
		/// <summary>
        /// Returns a search query for Employees
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		public virtual IQueryable<Employee> SearchEmployees(string searchTerm)
		{
			return this.Employees.Where(w => (w.FirstName != null && w.FirstName.Contains(searchTerm)) || (w.LastName != null && w.LastName.Contains(searchTerm)) || (w.Email != null && w.Email.Contains(searchTerm)) || (w.ActivationCode != null && w.ActivationCode.Contains(searchTerm)) || (w.SkypeId != null && w.SkypeId.Contains(searchTerm)) || (w.FacebookId != null && w.FacebookId.Contains(searchTerm)) || (w.ServiceUrl != null && w.ServiceUrl.Contains(searchTerm)) || (w.LastUsedId != null && w.LastUsedId.Contains(searchTerm)) || (w.BotId != null && w.BotId.Contains(searchTerm))).AsQueryable();
		}

		
		/// <summary>
        /// Returns a Employee entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual Employee GetEmployee(int id)
		{
			return this.Employees.FirstOrDefault(f => f.Id == id);
		}

		/// <summary>
        /// Adds a new Employee entity
        /// </summary>
        /// <param name="model">The Employee entity to add</param>
		public virtual void Add(Employee model)
		{
			this.Employees.Add(model);
		}

		/// <summary>
        /// Deletes a Employee entity
        /// </summary>
        /// <param name="model">The Employee entity to delete</param>
		public virtual void Delete(Employee model)
		{
			this.Employees.Remove(model);
		}

		#endregion

				#region LeaveRequests

		protected List<LeaveRequest> LeaveRequests = new List<LeaveRequest>();

		/// <summary>
        /// Returns a query for LeaveRequests
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<LeaveRequest> GetLeaveRequests(bool includeTracking = false)
		{
			return this.LeaveRequests.AsQueryable();
		}

		
		/// <summary>
        /// Returns a LeaveRequest entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual LeaveRequest GetLeaveRequest(System.Guid id)
		{
			return this.LeaveRequests.FirstOrDefault(f => f.Id == id);
		}

		/// <summary>
        /// Adds a new LeaveRequest entity
        /// </summary>
        /// <param name="model">The LeaveRequest entity to add</param>
		public virtual void Add(LeaveRequest model)
		{
			this.LeaveRequests.Add(model);
		}

		/// <summary>
        /// Deletes a LeaveRequest entity
        /// </summary>
        /// <param name="model">The LeaveRequest entity to delete</param>
		public virtual void Delete(LeaveRequest model)
		{
			this.LeaveRequests.Remove(model);
		}

		#endregion

				#region LeaveRequestStatuses

		protected List<LeaveRequestStatus> LeaveRequestStatuses = new List<LeaveRequestStatus>();

		/// <summary>
        /// Returns a query for LeaveRequestStatuses
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<LeaveRequestStatus> GetLeaveRequestStatuses(bool includeTracking = false)
		{
			return this.LeaveRequestStatuses.AsQueryable();
		}

		
		/// <summary>
        /// Returns a search query for LeaveRequestStatuses
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		public virtual IQueryable<LeaveRequestStatus> SearchLeaveRequestStatuses(string searchTerm)
		{
			return this.LeaveRequestStatuses.Where(w => (w.Name != null && w.Name.Contains(searchTerm))).AsQueryable();
		}

		
		/// <summary>
        /// Returns a LeaveRequestStatus entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual LeaveRequestStatus GetLeaveRequestStatus(byte id)
		{
			return this.LeaveRequestStatuses.FirstOrDefault(f => f.Id == id);
		}

		/// <summary>
        /// Adds a new LeaveRequestStatus entity
        /// </summary>
        /// <param name="model">The LeaveRequestStatus entity to add</param>
		public virtual void Add(LeaveRequestStatus model)
		{
			this.LeaveRequestStatuses.Add(model);
		}

		/// <summary>
        /// Deletes a LeaveRequestStatus entity
        /// </summary>
        /// <param name="model">The LeaveRequestStatus entity to delete</param>
		public virtual void Delete(LeaveRequestStatus model)
		{
			this.LeaveRequestStatuses.Remove(model);
		}

		#endregion

				#region Users

		protected List<User> Users = new List<User>();

		/// <summary>
        /// Returns a query for Users
        /// </summary>
        /// <param name="includeTracking">Indicates if the returned entities must be tracked</param>
        /// <returns>The query</returns>
		public virtual IQueryable<User> GetUsers(bool includeTracking = false)
		{
			return this.Users.AsQueryable();
		}

		
		/// <summary>
        /// Returns a search query for Users
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <returns>The query</returns>
		public virtual IQueryable<User> SearchUsers(string searchTerm)
		{
			return this.Users.Where(w => (w.Username != null && w.Username.Contains(searchTerm)) || (w.Email != null && w.Email.Contains(searchTerm)) || (w.Password != null && w.Password.Contains(searchTerm)) || (w.PasswordSalt != null && w.PasswordSalt.Contains(searchTerm))).AsQueryable();
		}

		
		/// <summary>
        /// Returns a User entity
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <returns>The entity or null if it was not found</returns>
		public virtual User GetUser(System.Guid id)
		{
			return this.Users.FirstOrDefault(f => f.Id == id);
		}

		/// <summary>
        /// Adds a new User entity
        /// </summary>
        /// <param name="model">The User entity to add</param>
		public virtual void Add(User model)
		{
			this.Users.Add(model);
		}

		/// <summary>
        /// Deletes a User entity
        /// </summary>
        /// <param name="model">The User entity to delete</param>
		public virtual void Delete(User model)
		{
			this.Users.Remove(model);
		}

		#endregion

			}
}

