namespace Elias.DAL.Migrations
{
    using Entities;
    using Enums;
    using Shared.Helpers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Elias.DAL.EliasEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EliasEntities context)
        {
            context.LeaveRequestStatuses.AddOrUpdate(
                new LeaveRequestStatus() { Id = 1, Name = "Pending" },
                new LeaveRequestStatus() { Id = 2, Name = "Accepted" },
                new LeaveRequestStatus() { Id = 3, Name = "Rejected" }
            );

            context.Employees.AddOrUpdate(
                r => r.SkypeId,
                new Employee() { FirstName = "Panagiotis", LastName = "Bakos", Email = "panbac88@hotmail.com", LeaveDays = 22, SkypeId = "default-user" }
            );

            if (!context.LeaveRequests.Any())
            {
                var employee = context.Employees.FirstOrDefault();

                var leaveRequest = new LeaveRequest();
                leaveRequest.Id = Guid.NewGuid();
                leaveRequest.FromDate = new DateTime(DateTime.UtcNow.Year, 1, 1);
                leaveRequest.ToDate = leaveRequest.FromDate.AddDays(5);
                leaveRequest.StatusId = (byte)LeaveRequestStatusEnum.Pending;
                leaveRequest.ComputeTotalDays();
                employee.LeaveRequests.Add(leaveRequest);

                leaveRequest = new LeaveRequest();
                leaveRequest.Id = Guid.NewGuid();
                leaveRequest.FromDate = new DateTime(DateTime.UtcNow.Year, 3, 1);
                leaveRequest.ToDate = leaveRequest.FromDate.AddDays(5);
                leaveRequest.StatusId = (byte)LeaveRequestStatusEnum.Pending;
                leaveRequest.ComputeTotalDays();
                employee.LeaveRequests.Add(leaveRequest);
            }

            string passwordSalt;
            string password = PasswordHelper.HashPassword("1234", out passwordSalt);

            context.Users.AddOrUpdate(
                new User() { Username = "admin", Email = "admin@softcom-int.com", Password = password, PasswordSalt = passwordSalt }
            );
        }
    }
}
