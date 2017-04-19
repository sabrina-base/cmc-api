using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using WashMyCar.API.Data;
using WashMyCar.API.Models;
using WashMyCar.API.Utility;

namespace WashMyCar.API.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WashMyCarDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WashMyCarDataContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            Random random = new Random();

            string[] addresses = new string[]
            {
                "101 W Broadway, San Diego CA 92101",
                "13030 Salmon River Road, San Diego CA 92129",
                "1930 Georgia Court, San Diego CA 92104",
                "3872 Jewell Street, San Diego CA 92109",
                "47 Southfield Road, Pocklington, York YO42 2XE"
            };

            if (context.Roles.Count() == 0)
            {
                context.Roles.Add(new IdentityRole
                {
                    Name = "Detailer"
                });

                context.Roles.Add(new IdentityRole
                {
                    Name = "Customer"
                });

                context.SaveChanges();
            }
            if (context.DayOfWeeks.Count() == 0)
            {
                string[] weekday = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                for (int i = 0; i < weekday.Length; i++)
                {
                    context.DayOfWeeks.Add(new Models.DayOfWeek
                    {
                        Weekday = weekday[i]
                    });
                }
                context.SaveChanges();
            }
            if (context.Services.Count() == 0)
            {
                Dictionary<string, decimal> services = new Dictionary<string, decimal>() {
                    { "Handwash", 39.99M},
                    { "Handwax", 69.99M},
                    { "Complete Interior", 129.99M},
                    { "Complete Exterior", 179.99M},
                    { "Steam Clean Interior", 119.99M},
                    { "Leather Treatment", 59.99M},
                    { "Deluxe Detail", 239.99M},
                    { "Light and Rim Restoration", 49.99M}
                };

                foreach (KeyValuePair<string, decimal> service in services)
                {
                    context.Services.Add(new Service
                    {
                        ServiceType = service.Key,
                        Cost = service.Value
                    });
                }
            }
            if (context.VehicleTypes.Count() == 0)
            {
                string[] vehicletype = new string[] { "Coupe", "Sedan", "SUV" };
                decimal[] multiplier = new decimal[] { 1, 1.2M, 1.3M };
                for (int i = 0; i < vehicletype.Length; i++)
                {
                    context.VehicleTypes.Add(new Models.VehicleType
                    {
                        VehicleSize = vehicletype[i],
                        Multiplier = multiplier[i]
                    });
                }
                context.SaveChanges();
            }
            if (context.Customers.Count() == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    string email = Faker.InternetFaker.Email();
                    string phone = Faker.PhoneFaker.Phone();

                    userManager.Create(new User
                    {
                        Email = email,
                        PhoneNumber = phone,
                        UserName = $"customer{i + 1}",
                        Customer = new Customer
                        {
                            Cellphone = phone,
                            EmailAddress = email,
                            FirstName = Faker.NameFaker.FirstName(),
                            LastName = Faker.NameFaker.LastName(),
                            Address = addresses[i],
                            Location = LocationConverter.GeocodeAddress(addresses[i])
                        }

                    }, "password123");
                }
            }
            if (context.Detailers.Count() == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    string email = Faker.InternetFaker.Email();
                    string phone = Faker.PhoneFaker.Phone();

                    var detailer = new Detailer
                    {
                        Cellphone = phone,
                        EmailAddress = email,
                        FirstName = Faker.NameFaker.FirstName(),
                        LastName = Faker.NameFaker.LastName(),
                        Address = addresses[i],
                        Location = LocationConverter.GeocodeAddress(addresses[i])
                    };

                    // availability
                    for (int j = 2; j < 7; j++)
                    {
                        detailer.DetailerAvailabilities.Add(new DetailerAvailability
                        {
                            DayOfWeekId = j,
                            Start = 9,
                            End = 5
                        });
                    }

                    // service(s)
                    var services = context.Services.ToList();
                    for (int j = 0; j < Faker.NumberFaker.Number(1, services.Count()); j++)
                    {
                        // grab a random service from db
                        var service = services[random.Next(services.Count())];
                        if (detailer.DetailerServices.All(x => x.ServiceId != service.ServiceId))
                        {
                            detailer.DetailerServices.Add(new DetailerService
                            {
                                Service = service,
                                Multiplier = (decimal)random.NextDouble()
                            });
                        }
                    }

                    userManager.Create(new User
                    {
                        Email = email,
                        PhoneNumber = phone,
                        UserName = $"detailer{i + 1}",
                        Detailer = detailer
                    }, "password123");
                }
            }
            if (context.Appointments.Count() == 0)
            {
                foreach (var detailer in context.Detailers)
                {
                    var randomNumber = Faker.NumberFaker.Number(1, context.Customers.Count());

                    for (int i = 0; i < randomNumber; i++)
                    {
                        var apptDate = DateTime.Now.AddDays(Faker.NumberFaker.Number(-14, 14));

                        var appt = new Appointment
                        {
                            AppointmentDate = apptDate,
                            CancelledDate = Faker.BooleanFaker.Boolean() ? (DateTime?)apptDate.AddDays(-3) : null,
                            ConfirmedDate = Faker.BooleanFaker.Boolean() ? (DateTime?)apptDate.AddDays(-3) : null,
                            CustomerId = Faker.NumberFaker.Number(1, context.Customers.Count()),
                            Detailer = detailer,
                            VehicleTypeId = Faker.NumberFaker.Number(1, context.VehicleTypes.Count()),
                            Rating = Faker.BooleanFaker.Boolean() ? (int?)Faker.NumberFaker.Number(1, 5) : null
                        };

                        // services
                        for (int j = 0; j < Faker.NumberFaker.Number(1, detailer.DetailerServices.Count); j++)
                        {
                            appt.AppointmentServices.Add(new AppointmentService
                            {
                                Service = detailer.DetailerServices.ElementAt(j).Service
                            });
                        }

                        context.Appointments.Add(appt);
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
