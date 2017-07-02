namespace Elias.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ActivationCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "ActivationCode", c => c.String(maxLength: 6));
            AddColumn("dbo.Employees", "CodeExpirationDateTime", c => c.DateTime());
            AlterColumn("dbo.Employees", "SkypeId", c => c.String(maxLength: 250));
        }

        public override void Down()
        {
            Sql("DELETE FROM [dbo].[Employees] WHERE [SkypeId] IS NULL");

            AlterColumn("dbo.Employees", "SkypeId", c => c.String(nullable: false, maxLength: 250));
            DropColumn("dbo.Employees", "CodeExpirationDateTime");
            DropColumn("dbo.Employees", "ActivationCode");
        }
    }
}
