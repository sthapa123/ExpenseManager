namespace SebJones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ClaimVAT = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.ClaimComments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        Text = c.String(),
                        ClaimID_ClaimID = c.Int(),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Claims", t => t.ClaimID_ClaimID)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.ClaimID_ClaimID)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        ClaimID = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Status_ClaimStatusID = c.Int(),
                    })
                .PrimaryKey(t => t.ClaimID)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.ClaimStatus", t => t.Status_ClaimStatusID)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Status_ClaimStatusID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        ReceiptID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Category_CategoryID = c.Int(),
                        ClaimID_ClaimID = c.Int(),
                    })
                .PrimaryKey(t => t.ReceiptID)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryID)
                .ForeignKey("dbo.Claims", t => t.ClaimID_ClaimID)
                .Index(t => t.Category_CategoryID)
                .Index(t => t.ClaimID_ClaimID);
            
            CreateTable(
                "dbo.ClaimStatus",
                c => new
                    {
                        ClaimStatusID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ClaimStatusID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClaimComments", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Claims", "Status_ClaimStatusID", "dbo.ClaimStatus");
            DropForeignKey("dbo.Receipts", "ClaimID_ClaimID", "dbo.Claims");
            DropForeignKey("dbo.Receipts", "Category_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Claims", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClaimComments", "ClaimID_ClaimID", "dbo.Claims");
            DropIndex("dbo.ClaimComments", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Claims", new[] { "Status_ClaimStatusID" });
            DropIndex("dbo.Receipts", new[] { "ClaimID_ClaimID" });
            DropIndex("dbo.Receipts", new[] { "Category_CategoryID" });
            DropIndex("dbo.Claims", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ClaimComments", new[] { "ClaimID_ClaimID" });
            DropTable("dbo.ClaimStatus");
            DropTable("dbo.Receipts");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Claims");
            DropTable("dbo.ClaimComments");
            DropTable("dbo.Categories");
        }
    }
}
