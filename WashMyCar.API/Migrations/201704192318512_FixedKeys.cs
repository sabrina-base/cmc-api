namespace WashMyCar.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DetailerServices", "DetailerId", "dbo.Detailers");
            AddForeignKey("dbo.DetailerServices", "DetailerId", "dbo.Detailers", "DetailerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetailerServices", "DetailerId", "dbo.Detailers");
            AddForeignKey("dbo.DetailerServices", "DetailerId", "dbo.Detailers", "DetailerId");
        }
    }
}
