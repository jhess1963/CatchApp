namespace CatchApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CatchApp_Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AbpFeatures", "EditionId", "dbo.AbpEditions");
            DropForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles");
            DropForeignKey("dbo.AbpUserLogins", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpUserRoles", "UserId", "dbo.AbpUsers");
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Club",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DailyOpenTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Byte(nullable: false),
                        OpenTime = c.DateTime(nullable: false),
                        CloseTime = c.DateTime(nullable: false),
                        ClubId = c.Int(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Club", t => t.ClubId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.ClubVisit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntryDate = c.DateTime(nullable: false),
                        LeavingDate = c.DateTime(),
                        HasLeft = c.Boolean(nullable: false),
                        ClubId = c.Int(nullable: false),
                        MemberId = c.Long(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Club", t => t.ClubId)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.ClubId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Surname = c.String(maxLength: 50),
                        IsMale = c.Boolean(nullable: false),
                        IsSingle = c.Boolean(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClubCategory",
                c => new
                    {
                        Club_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Club_Id, t.Category_Id })
                .ForeignKey("dbo.Club", t => t.Club_Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.Club_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Friendship",
                c => new
                    {
                        MemberId = c.Long(nullable: false),
                        FriendId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MemberId, t.FriendId })
                .ForeignKey("dbo.Member", t => t.MemberId)
                .ForeignKey("dbo.Member", t => t.FriendId)
                .Index(t => t.MemberId)
                .Index(t => t.FriendId);
            
            AddForeignKey("dbo.AbpFeatures", "EditionId", "dbo.AbpEditions", "Id");
            AddForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles", "Id");
            AddForeignKey("dbo.AbpUserLogins", "UserId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.AbpUserRoles", "UserId", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpUserRoles", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpUserLogins", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles");
            DropForeignKey("dbo.AbpFeatures", "EditionId", "dbo.AbpEditions");
            DropForeignKey("dbo.ClubVisit", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Friendship", "FriendId", "dbo.Member");
            DropForeignKey("dbo.Friendship", "MemberId", "dbo.Member");
            DropForeignKey("dbo.ClubVisit", "ClubId", "dbo.Club");
            DropForeignKey("dbo.DailyOpenTime", "ClubId", "dbo.Club");
            DropForeignKey("dbo.ClubCategory", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.ClubCategory", "Club_Id", "dbo.Club");
            DropIndex("dbo.Friendship", new[] { "FriendId" });
            DropIndex("dbo.Friendship", new[] { "MemberId" });
            DropIndex("dbo.ClubCategory", new[] { "Category_Id" });
            DropIndex("dbo.ClubCategory", new[] { "Club_Id" });
            DropIndex("dbo.ClubVisit", new[] { "MemberId" });
            DropIndex("dbo.ClubVisit", new[] { "ClubId" });
            DropIndex("dbo.DailyOpenTime", new[] { "ClubId" });
            DropTable("dbo.Friendship");
            DropTable("dbo.ClubCategory");
            DropTable("dbo.Member");
            DropTable("dbo.ClubVisit");
            DropTable("dbo.DailyOpenTime");
            DropTable("dbo.Club");
            DropTable("dbo.Category");
            AddForeignKey("dbo.AbpUserRoles", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AbpPermissions", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AbpUserLogins", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AbpPermissions", "RoleId", "dbo.AbpRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AbpFeatures", "EditionId", "dbo.AbpEditions", "Id", cascadeDelete: true);
        }
    }
}
