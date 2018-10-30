namespace CatchApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class Add_SpatialDataType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Club", "Location", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Club", "Location");
        }
    }
}
