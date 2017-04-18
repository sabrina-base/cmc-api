using WashMyCar.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http;
using WashMyCar.API.Data;
using System.Data.Entity.Spatial;

namespace WashMyCar.API.Controllers
{
    public class UsersController : ApiController
    {
        private UserManager<User> _userManager;

        public UsersController()
        {
            var db = new WashMyCarDataContext();
            var store = new UserStore<User>(db);

            _userManager = new UserManager<User>(store);
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
                UserName = registration.EmailAddress
            };

            var customer = new Customer
            {
                Address = registration.Address,
                Cellphone = registration.Cellphone,
                EmailAddress = registration.EmailAddress,
                FirstName = registration.FirstName,
                LastName = registration.LastName
            };

            var geocoderService = new Geocoder.GeocodeService();

            var customerLocation = geocoderService.GeocodeLocation(registration.Address);

            customer.Location = DbGeography.FromText($"POINT({customerLocation.Longitude}, {customerLocation.Latitude})");

            user.Customer = customer;

            var result = _userManager.Create(user, registration.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Invalid user registration");
            }
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
                UserName = registration.EmailAddress
            };

            var detailer = new Detailer
            {
                Address = registration.Address,
                Cellphone = registration.Cellphone,
                EmailAddress = registration.EmailAddress,
                FirstName = registration.FirstName,
                LastName = registration.LastName
            };

            var geocoderService = new Geocoder.GeocodeService();

            var detailerLocation = geocoderService.GeocodeLocation(registration.Address);

            detailer.Location = DbGeography.FromText($"POINT({detailerLocation.Longitude}, {detailerLocation.Latitude})");

            user.Detailer = detailer;

            var result = _userManager.Create(user, registration.Password);

            if (result.Succeeded)
            {
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