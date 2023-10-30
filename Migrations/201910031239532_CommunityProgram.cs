namespace IJPReporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommunityProgram : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProgramId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CommunityId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "RegionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RegionId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "CommunityId");
            DropColumn("dbo.AspNetUsers", "ProgramId");
        }
    }
}
