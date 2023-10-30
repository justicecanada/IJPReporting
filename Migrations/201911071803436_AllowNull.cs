namespace IJPReporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "ProgramId", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "CommunityId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CommunityId", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "ProgramId", c => c.Int(nullable: false));
        }
    }
}
