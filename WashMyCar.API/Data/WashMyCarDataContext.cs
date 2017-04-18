using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WashMyCar.API.Models;
using WashMyCar.API.Migrations;
using System.Data.Entity.SqlServer;

namespace WashMyCar.API.Data
{
    public class WashMyCarDataContext : IdentityDbContext<User>
    {
        public WashMyCarDataContext() : base("WashMyCar")
        {
            SqlProviderServices.SqlServerTypesAssemblyName =
                "Microsoft.SqlServer.Types, Version=13.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";

            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<WashMyCarDataContext, Configuration>()
            );
        }

        public IDbSet<Appointment> Appointments { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Models.DayOfWeek> DayOfWeeks { get; set; }
        public IDbSet<Detailer> Detailers { get; set; }
        public IDbSet<DetailerAvailability> DetailerAvailabilities { get; set; }
        public IDbSet<Service> Services { get; set; }
        public IDbSet<VehicleType> VehicleTypes { get; set; }
        public IDbSet<AppointmentService> AppointmentServices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // customer has many sales
            modelBuilder.Entity<Appointment>()
              .HasMany(appointment => appointment.AppointmentServices)
              .WithRequired(service => service.Appointment)
              .HasForeignKey(service => service.AppointmentId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(customer => customer.Appointments)
                .WithRequired(appointment => appointment.Customer)
                .HasForeignKey(appointment => appointment.CustomerId);

            modelBuilder.Entity<Models.DayOfWeek>()
                .HasMany(dayOfWeek => dayOfWeek.DetailersAvailability)
                .WithRequired(detailersAvailability => detailersAvailability.DayOfWeek)
                .HasForeignKey(detailersAvailability => detailersAvailability.DayOfWeekId);

            modelBuilder.Entity<Detailer>()
                .HasMany(detailer => detailer.DetailerAvailabilities)
                .WithRequired(detailersAvailability => detailersAvailability.Detailer)
                .HasForeignKey(detailersAvailability => detailersAvailability.DetailerId);

            modelBuilder.Entity<Detailer>()
                .HasMany(detailer => detailer.Services)
                .WithOptional(service => service.Detailer)
                .HasForeignKey(service => service.DetailerId);

            modelBuilder.Entity<Detailer>()
                .HasMany(detailer => detailer.Appointments)
                .WithRequired(appointment => appointment.Detailer)
                .HasForeignKey(appointment=> appointment.DetailerId);

            modelBuilder.Entity<VehicleType>()
                .HasMany(vehicleType => vehicleType.Appointments)
                .WithRequired(appointment => appointment.VehicleType)
                .HasForeignKey(appointment => appointment.VehicleTypeId);

            // 1-to-1: User -> Detailer
            modelBuilder.Entity<User>()
               .HasOptional(user => user.Detailer)
               .WithOptionalDependent(detailer => detailer.User)
               .Map(m => m.MapKey("DetailerId"));

            // 1-to-1: User -> Customer
            modelBuilder.Entity<User>()
                .HasOptional(user => user.Customer)
                .WithOptionalDependent(customer => customer.User)
                .Map(m => m.MapKey("CustomerId"));

        // Configure the compound keys
        modelBuilder.Entity<AppointmentService>()
                        .HasKey(a => new { a.AppointmentId, a.ServiceId });

            modelBuilder.Entity<DetailerAvailability>()
                        .HasKey(a => new { a.DetailerId, a.DayOfWeekId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
