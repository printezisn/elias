namespace Elias.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 250),
                    LastName = c.String(nullable: false, maxLength: 250),
                    Email = c.String(nullable: false, maxLength: 250),
                    LeaveDays = c.Int(nullable: false),
                    SkypeId = c.String(nullable: false, maxLength: 250),
                    FacebookId = c.String(maxLength: 250),
                    ServiceUrl = c.String(maxLength: 1024),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LeaveRequests",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    EmployeeId = c.Int(nullable: false),
                    FromDate = c.DateTime(nullable: false),
                    ToDate = c.DateTime(nullable: false),
                    TotalDays = c.Int(nullable: false),
                    StatusId = c.Byte(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.LeaveRequestStatuses", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.StatusId);

            CreateTable(
                "dbo.LeaveRequestStatuses",
                c => new
                {
                    Id = c.Byte(nullable: false),
                    Name = c.String(nullable: false, maxLength: 20),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Username = c.String(nullable: false, maxLength: 250),
                    Email = c.String(nullable: false, maxLength: 250),
                    Password = c.String(nullable: false, maxLength: 500),
                    SessionId = c.Guid(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.LeaveRequests", "StatusId", "dbo.LeaveRequestStatuses");
            DropForeignKey("dbo.LeaveRequests", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.LeaveRequests", new[] { "StatusId" });
            DropIndex("dbo.LeaveRequests", new[] { "EmployeeId" });
            DropTable("dbo.Users");
            DropTable("dbo.LeaveRequestStatuses");
            DropTable("dbo.LeaveRequests");
            DropTable("dbo.Employees");
        }
    }
}
