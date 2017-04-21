using WashMyCar.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http;
using WashMyCar.API.Data;
using System.Data.Entity.Spatial;
using WashMyCar.API.Utility;
using System.Linq;

namespace WashMyCar.API.Controllers
{
    public class UsersController : BaseApiController
    {
        private UserManager<User> _userManager;

        public UsersController()
        {
            var db = new WashMyCarDataContext();
            var store = new UserStore<User>(db);

            _userManager = new UserManager<User>(store);
        }

        // GET: api/customerProfile
        [Route("api/customerProfile")]
        public IHttpActionResult GetCustomerProfile()
        {
            if(CurrentUser.Customer == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                CurrentUser.Customer.CustomerId,
                CurrentUser.Customer.FirstName,
                CurrentUser.Customer.LastName,
                CurrentUser.Customer.Address,
                CurrentUser.Customer.EmailAddress,
                CurrentUser.Customer.Cellphone,
                Location = new
                {
                    CurrentUser.Customer.Location.Latitude,
                    CurrentUser.Customer.Location.Longitude
                },
                Appointments = CurrentUser.Customer.Appointments.Select(ca => new {
                    ca.AppointmentDate,
                    ca.DetailerId,
                    ca.Detailer.FirstName,
                    ca.Detailer.LastName,
                    ca.TotalCost,
                    ca.VehicleType.VehicleSize,
                    ca.Rating,
                    ca.VehicleTypeId
                })

            });
        }

        // GET: api/detailerProfile
        // TODO: Copy GetCustomerProfile and modify to work for detailers
        [Route("api/detailerProfile")]
        public IHttpActionResult GetDetailerProfile()
        {
            if (CurrentUser.Customer == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                CurrentUser.Detailer.DetailerId,
                CurrentUser.Detailer.Rating,
                CurrentUser.Detailer.Address,
                CurrentUser.Detailer.EmailAddress,
                CurrentUser.Detailer.Cellphone,
                Availability = CurrentUser.Detailer.DetailerAvailabilities.Select(da => new
                {
                    DayOfWeek = da.DayOfWeek.Weekday,
                    da.Start,
                    da.DayOfWeekId,
                    da.DetailerId,
                    da.End
                }),
                CurrentUser.Detailer.FirstName,
                CurrentUser.Detailer.LastName,
                Services = CurrentUser.Detailer.DetailerServices.Select(ds => new
                {
                    ds.Service.Cost,
                    ds.Service.ServiceType
                }),
                Location = new
                {
                    CurrentUser.Detailer.Location.Latitude,
                    CurrentUser.Detailer.Location.Longitude
                },
                Appointments = CurrentUser.Detailer.Appointments.Select(da => new
                {
                    da.Customer.FirstName,
                    da.Customer.LastName,
                    da.Customer.Address,
                    da.AppointmentDate,
                    da.Customer.Cellphone
                })
            });
        }

        // POST: api/users/register
        [AllowAnonymous]
        [Route("api/users/registerCustomer")]
        public IHttpActionResult RegisterCustomer(RegistrationModel registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = registration.Username,
                Email = registration.EmailAddress,
                PhoneNumber = registration.Cellphone
            };

            var customer = new Customer
            {
                Address = registration.Address,
                Cellphone = registration.Cellphone,
                EmailAddress = registration.EmailAddress,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Location = LocationConverter.GeocodeAddress(registration.Address)
            };
            
            user.Customer = customer;

            var result = _userManager.Create(user, registration.Password);

            if (result.Succeeded)
            {
                _userManager.AddToRole(user.Id, "Detailer");
                return Ok();
            }
            else
            {
                return BadRequest("Invalid user registration");
            }
        }

        // POST: api/users/register
        [AllowAnonymous]
        [Route("api/users/registerDetailer")]
        public IHttpActionResult RegisterDetailer(RegistrationModel registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = registration.Username,
                Email = registration.EmailAddress,
                PhoneNumber = registration.Cellphone
            };

            var detailer = new Detailer
            {
                Address = registration.Address,
                Cellphone = registration.Cellphone,
                EmailAddress = registration.EmailAddress,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Location = LocationConverter.GeocodeAddress(registration.Address)
            };
            
            user.Detailer = detailer;

            var result = _userManager.Create(user, registration.Password);

            if (result.Succeeded)
            {
                _userManager.AddToRole(user.Id, "Detailer");

                return Ok();
            }
            else
            {
                return BadRequest("Invalid user registration");
            }
        }

        protected override void Dispose(bool disposing)
        {
            _userManager.Dispose();
        }
    }
}