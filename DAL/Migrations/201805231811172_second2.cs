namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DB_Lot", "DB_User_UserId1", "dbo.DB_User");
            DropIndex("dbo.DB_Lot", new[] { "DB_User_UserId1" });
            DropColumn("dbo.DB_Lot", "DB_User_UserId1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DB_Lot", "DB_User_UserId1", c => c.Int());
            CreateIndex("dbo.DB_Lot", "DB_User_UserId1");
            AddForeignKey("dbo.DB_Lot", "DB_User_UserId1", "dbo.DB_User", "UserId");
        }
    }
}
