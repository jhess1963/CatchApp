namespace CatchApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CatchApp_Added_Images : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClubImage",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Club", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.MemberImage",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberImage", "Id", "dbo.Member");
            DropForeignKey("dbo.ClubImage", "Id", "dbo.Club");
            DropIndex("dbo.MemberImage", new[] { "Id" });
            DropIndex("dbo.ClubImage", new[] { "Id" });
            DropTable("dbo.MemberImage");
            DropTable("dbo.ClubImage");
        }
    }
}
