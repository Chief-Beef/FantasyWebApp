namespace FantasyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayerGrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Grade", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "Grade");
        }
    }
}
