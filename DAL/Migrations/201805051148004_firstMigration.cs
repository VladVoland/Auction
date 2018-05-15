namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DB_Lot", "Step", c => c.Int());
            AlterColumn("dbo.DB_Lot", "StartDate", c => c.DateTime());
            AlterColumn("dbo.DB_Lot", "EndDate", c => c.DateTime());
            AlterColumn("dbo.DB_Lot", "Winner", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DB_Lot", "Winner", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.DB_Lot", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DB_Lot", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DB_Lot", "Step", c => c.Int(nullable: false));
        }
    }
}
