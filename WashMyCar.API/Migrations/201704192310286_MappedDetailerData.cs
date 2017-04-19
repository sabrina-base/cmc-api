namespace WashMyCar.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MappedDetailerData : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Services", name: "DetailerId", newName: "Detailer_DetailerId");
            RenameIndex(table: "dbo.Services", name: "IX_DetailerId", newName: "IX_Detailer_DetailerId");
            CreateTable(
                "dbo.DetailerServices",
                c => new
                    {
                        DetailerId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DetailerId, t.ServiceId })
                .ForeignKey("dbo.Detailers", t => t.DetailerId)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.DetailerId)
                .Index(t => t.ServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetailerServices", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.DetailerServices", "DetailerId", "dbo.Detailers");
            DropIndex("dbo.DetailerServices", new[] { "ServiceId" });
            DropIndex("dbo.DetailerServices", new[] { "DetailerId" });
            DropTable("dbo.DetailerServices");
            RenameIndex(table: "dbo.Services", name: "IX_Detailer_DetailerId", newName: "IX_DetailerId");
            RenameColumn(table: "dbo.Services", name: "Detailer_DetailerId", newName: "DetailerId");
        }
    }
}
