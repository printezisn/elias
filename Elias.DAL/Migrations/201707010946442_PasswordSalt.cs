namespace Elias.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PasswordSalt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordSalt", c => c.String(nullable: false, maxLength: 500));
        }

        public override void Down()
        {
            DropColumn("dbo.Users", "PasswordSalt");
        }
    }
}
