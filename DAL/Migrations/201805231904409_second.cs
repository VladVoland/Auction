namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DB_Lot", "DB_User_UserId", "dbo.DB_User");
            DropForeignKey("dbo.DB_User", "DB_Lot_LotId", "dbo.DB_Lot");
            DropIndex("dbo.DB_Lot", new[] { "DB_User_UserId" });
            DropIndex("dbo.DB_User", new[] { "DB_Lot_LotId" });
            DropColumn("dbo.DB_Lot", "DB_User_UserId");
            DropColumn("dbo.DB_User", "DB_Lot_LotId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DB_User", "DB_Lot_LotId", c => c.Int());
            AddColumn("dbo.DB_Lot", "DB_User_UserId", c => c.Int());
            CreateIndex("dbo.DB_User", "DB_Lot_LotId");
            CreateIndex("dbo.DB_Lot", "DB_User_UserId");
            AddForeignKey("dbo.DB_User", "DB_Lot_LotId", "dbo.DB_Lot", "LotId");
            AddForeignKey("dbo.DB_Lot", "DB_User_UserId", "dbo.DB_User", "UserId");
        }
    }
}
