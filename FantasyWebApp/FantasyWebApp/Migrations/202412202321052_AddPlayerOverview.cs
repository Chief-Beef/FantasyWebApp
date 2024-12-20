namespace FantasyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayerOverview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Overview", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "Overview");
        }
    }
}
