namespace CatchApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClubImage", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.ClubImage", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.ClubImage", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.ClubImage", "CreatorUserId", c => c.Long());
            AddColumn("dbo.MemberImage", "LastModificationTime", c => c.DateTime());
            AddColumn("dbo.MemberImage", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.MemberImage", "CreationTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.MemberImage", "CreatorUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberImage", "CreatorUserId");
            DropColumn("dbo.MemberImage", "CreationTime");
            DropColumn("dbo.MemberImage", "LastModifierUserId");
            DropColumn("dbo.MemberImage", "LastModificationTime");
            DropColumn("dbo.ClubImage", "CreatorUserId");
            DropColumn("dbo.ClubImage", "CreationTime");
            DropColumn("dbo.ClubImage", "LastModifierUserId");
            DropColumn("dbo.ClubImage", "LastModificationTime");
        }
    }
}
