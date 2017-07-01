namespace Elias.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class LeaveRequest_RequestDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveRequests", "RequestDate", c => c.DateTime(nullable: true));
            Sql("UPDATE [dbo].[LeaveRequests] SET [RequestDate] = GETDATE()");
            AlterColumn("dbo.LeaveRequests", "RequestDate", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.LeaveRequests", "RequestDate");
        }
    }
}
