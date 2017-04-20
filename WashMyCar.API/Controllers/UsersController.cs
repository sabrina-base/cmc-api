using WashMyCar.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http;
using WashMyCar.API.Data;
using System.Data.Entity.Spatial;
using WashMyCar.API.Utility;

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
                CurrentUser.Customer.Address,
                CurrentUser.Customer.Cellphone,
                CurrentUser.Customer.EmailAddress
            });
        }

        // GET: api/detailerProfile
        // TODO: Copy GetCustomerProfile and modify to work for detailers

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
                UserName = registration.Username
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
                UserName = registration.Username
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