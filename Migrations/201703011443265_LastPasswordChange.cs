namespace IJPReporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class LastPasswordChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastPasswordChange", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastPasswordChange");
        }
    }
}
