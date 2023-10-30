namespace IJPReporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegionId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RegionId");
        }
    }
}
