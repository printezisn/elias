namespace Elias.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BotFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "LastUsedId", c => c.String(maxLength: 250));
            AddColumn("dbo.Employees", "BotId", c => c.String(maxLength: 250));
        }

        public override void Down()
        {
            DropColumn("dbo.Employees", "BotId");
            DropColumn("dbo.Employees", "LastUsedId");
        }
    }
}
