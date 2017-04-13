using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WashMyCar.API.Data;
using WashMyCar.API.Models;

namespace WashMyCar.API.Controllers
{
    public class CustomersController : ApiController
    {
        private WashMyCarDataContext db = new WashMyCarDataContext();

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
                customer.Latitude,
                customer.Longitude
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
                customer.Latitude,
                customer.Longitude
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
            dbCustomer.Latitude = customer.Latitude;
            dbCustomer.Longitude = customer.Longitude;

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

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, new
            {
                customer.CustomerId,
                customer.FirstName,
                customer.LastName,
                customer.Address,
                customer.EmailAddress,
                customer.Cellphone,
                customer.Latitude,
                customer.Longitude
            });
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