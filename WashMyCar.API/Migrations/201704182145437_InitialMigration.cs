namespace WashMyCar.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        VehicleTypeId = c.Int(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DetailerId = c.Int(nullable: false),
                        ConfirmedDate = c.DateTime(),
                        CancelledDate = c.DateTime(),
                        Rating = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Detailers", t => t.DetailerId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeId, cascadeDelete: true)
                .Index(t => t.VehicleTypeId)
                .Index(t => t.CustomerId)
                .Index(t => t.DetailerId);
            
            CreateTable(
                "dbo.AppointmentServices",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppointmentId, t.ServiceId })
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .Index(t => t.AppointmentId)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        DetailerId = c.Int(),
                        ServiceType = c.String(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ServiceId)
                .ForeignKey("dbo.Detailers", t => t.DetailerId)
                .Index(t => t.DetailerId);
            
            CreateTable(
                "dbo.Detailers",
                c => new
                    {
                        DetailerId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        Cellphone = c.String(),
                        Address = c.String(),
                        Location = c.Geography(),
                    })
                .PrimaryKey(t => t.DetailerId);
            
            CreateTable(
                "dbo.DetailerAvailabilities",
                c => new
                    {
                        DetailerId = c.Int(nullable: false),
                        DayOfWeekId = c.Int(nullable: false),
                        Start = c.Single(),
                        End = c.Single(),
                        Multiplier = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.DetailerId, t.DayOfWeekId })
                .ForeignKey("dbo.DayOfWeeks", t => t.DayOfWeekId, cascadeDelete: true)
                .ForeignKey("dbo.Detailers", t => t.DetailerId, cascadeDelete: true)
                .Index(t => t.DetailerId)
                .Index(t => t.DayOfWeekId);
            
            CreateTable(
                "dbo.DayOfWeeks",
                c => new
                    {
                        DayOfWeekId = c.Int(nullable: false, identity: true),
                        Weekday = c.String(),
                    })
                .PrimaryKey(t => t.DayOfWeekId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        CustomerId = c.Int(),
                        DetailerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Detailers", t => t.DetailerId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.CustomerId)
                .Index(t => t.DetailerId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        Cellphone = c.String(),
                        Address = c.String(),
                        Location = c.Geography(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        VehicleTypeId = c.Int(nullable: false, identity: true),
                        VehicleSize = c.String(),
                        Multiplier = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.VehicleTypeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Appointments", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.AppointmentServices", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DetailerId", "dbo.Detailers");
            DropForeignKey("dbo.AspNetUsers", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Appointments", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Services", "DetailerId", "dbo.Detailers");
            DropForeignKey("dbo.DetailerAvailabilities", "DetailerId", "dbo.Detailers");
            DropForeignKey("dbo.DetailerAvailabilities", "DayOfWeekId", "dbo.DayOfWeeks");
            DropForeignKey("dbo.Appointments", "DetailerId", "dbo.Detailers");
            DropForeignKey("dbo.AppointmentServices", "ServiceId", "dbo.Services");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "DetailerId" });
            DropIndex("dbo.AspNetUsers", new[] { "CustomerId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.DetailerAvailabilities", new[] { "DayOfWeekId" });
            DropIndex("dbo.DetailerAvailabilities", new[] { "DetailerId" });
            DropIndex("dbo.Services", new[] { "DetailerId" });
            DropIndex("dbo.AppointmentServices", new[] { "ServiceId" });
            DropIndex("dbo.AppointmentServices", new[] { "AppointmentId" });
            DropIndex("dbo.Appointments", new[] { "DetailerId" });
            DropIndex("dbo.Appointments", new[] { "CustomerId" });
            DropIndex("dbo.Appointments", new[] { "VehicleTypeId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Customers");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.DayOfWeeks");
            DropTable("dbo.DetailerAvailabilities");
            DropTable("dbo.Detailers");
            DropTable("dbo.Services");
            DropTable("dbo.AppointmentServices");
            DropTable("dbo.Appointments");
        }
    }
}
