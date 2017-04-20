namespace WashMyCar.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MappedMoreInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "DenyDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "DenyDate");
        }
    }
}
