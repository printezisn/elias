namespace Elias.DAL.Migrations
{
    using Entities;
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

            string passwordSalt;
            string password = PasswordHelper.HashPassword("1234", out passwordSalt);

            context.Users.AddOrUpdate(
                new User() { Username = "admin", Email = "admin@softcom-int.com", Password = password, PasswordSalt = passwordSalt }
            );
        }
    }
}
