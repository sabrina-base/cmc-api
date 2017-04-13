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
    public class PaymentsController : ApiController
    {
        private WashMyCarDataContext db = new WashMyCarDataContext();

        // GET: api/Payments
        public IHttpActionResult GetPayments()
        {
            var resultSet = db.Payments.Select(payment => new
            {
                payment.PaymentId,
                payment.AppointmentId,
                payment.AmountReceived

            });
            return Ok(resultSet);  
        }

        // GET: api/Payments/5
        [ResponseType(typeof(Payment))]
        public IHttpActionResult GetPayment(int id)
        {
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                payment.PaymentId,
                payment.AppointmentId,
                payment.AmountReceived

            });
        }

        // PUT: api/Payments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPayment(int id, Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            var dbPayment = db.Payments.Find(id);

            dbPayment.AmountReceived = payment.AmountReceived;

            db.Entry(dbPayment).State = EntityState.Modified;
                try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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
        // POST: api/Payments
        [ResponseType(typeof(Payment))]
        public IHttpActionResult PostPayment(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Payments.Add(payment);
            db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = payment.PaymentId }, new
            {
                payment.PaymentId,
                payment.AppointmentId,
                payment.AmountReceived
            });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool PaymentExists(int id)
        {
            return db.Payments.Count(e => e.PaymentId == id) > 0;
        }
    }
};