using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WashMyCar.API.Data;
using WashMyCar.API.Models;
using WashMyCar.API.Utility;

namespace WashMyCar.API.Controllers
{
    public class CustomersController : ApiController
    {
        private Data.WashMyCarDataContext db = new Data.WashMyCarDataContext();

        // GET: api/Customers
        public IHttpActionResult GetCustomers()
        {
            var resultSet = db.Customers.Select(customer => new
            {
                customer.CustomerId,
                customer.FirstName,
                customer.LastName,
                customer.Address,
                customer.EmailAddress,
                customer.Cellphone,
                Location = new
                {
                    customer.Location.Latitude,
                    customer.Location.Longitude
                },
                // add appoints here.
                // add securite routes
            });
            return Ok(resultSet);
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                customer.CustomerId,
                customer.FirstName,
                customer.LastName,
                customer.Address,
                customer.EmailAddress,
                customer.Cellphone,
                Location = new
                {
                    customer.Location.Latitude,
                    customer.Location.Longitude
                },
                Appointments = customer.Appointments.Select(ca => new {
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

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            var dbCustomer = db.Customers.Find(id);
            dbCustomer.CustomerId = customer.CustomerId;
            dbCustomer.FirstName = customer.FirstName;
            dbCustomer.LastName = customer.LastName;
            dbCustomer.Address = customer.Address;
            dbCustomer.EmailAddress = customer.EmailAddress;
            dbCustomer.Cellphone = customer.Cellphone;
            dbCustomer.Location = LocationConverter.GeocodeAddress(dbCustomer.Address);
            dbCustomer.Appointments = customer.Appointments;

            db.Entry(dbCustomer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

     
        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }
    }
}