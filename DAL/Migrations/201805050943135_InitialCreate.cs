namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DB_Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.DB_Lot",
                c => new
                    {
                        LotId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Specification = c.String(nullable: false, maxLength: 1000),
                        Bet = c.Int(nullable: false),
                        Step = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Winner = c.String(nullable: false, maxLength: 200),
                        Category_CategoryId = c.Int(nullable: false),
                        Owner_UserId = c.Int(nullable: false),
                        Subcategory_SubcategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LotId)
                .ForeignKey("dbo.DB_Category", t => t.Category_CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.DB_User", t => t.Owner_UserId, cascadeDelete: true)
                .ForeignKey("dbo.DB_Subcategory", t => t.Subcategory_SubcategoryId)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.Owner_UserId)
                .Index(t => t.Subcategory_SubcategoryId);
            
            CreateTable(
                "dbo.DB_User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Syrname = c.String(nullable: false, maxLength: 30),
                        Patronymic = c.String(nullable: false, maxLength: 30),
                        Login = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 30),
                        Status = c.String(nullable: false, maxLength: 10),
                        PhoneNumber = c.Int(nullable: false),
                        Passport = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.DB_Subcategory",
                c => new
                    {
                        SubcategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Category_CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubcategoryId)
                .ForeignKey("dbo.DB_Category", t => t.Category_CategoryId, cascadeDelete: true)
                .Index(t => t.Category_CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DB_Lot", "Subcategory_SubcategoryId", "dbo.DB_Subcategory");
            DropForeignKey("dbo.DB_Subcategory", "Category_CategoryId", "dbo.DB_Category");
            DropForeignKey("dbo.DB_Lot", "Owner_UserId", "dbo.DB_User");
            DropForeignKey("dbo.DB_Lot", "Category_CategoryId", "dbo.DB_Category");
            DropIndex("dbo.DB_Subcategory", new[] { "Category_CategoryId" });
            DropIndex("dbo.DB_Lot", new[] { "Subcategory_SubcategoryId" });
            DropIndex("dbo.DB_Lot", new[] { "Owner_UserId" });
            DropIndex("dbo.DB_Lot", new[] { "Category_CategoryId" });
            DropTable("dbo.DB_Subcategory");
            DropTable("dbo.DB_User");
            DropTable("dbo.DB_Lot");
            DropTable("dbo.DB_Category");
        }
    }
}
