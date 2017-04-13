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
    public class AppointmentServicesController : ApiController
    {
        private WashMyCarDataContext db = new WashMyCarDataContext();

        // PUT: api/AppointmentServices/5/5
        [ResponseType(typeof(void))]
        [Route("api/AppointmentServices/{appointmentId}/{serviceId}")]
        public IHttpActionResult PutAppointmentService(int appointmentId, int serviceId, AppointmentService appointmentService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (appointmentId != appointmentService.AppointmentId || serviceId != appointmentService.AppointmentId)
            {
                return BadRequest();
            }
          
            var dbAppointmentService = db.AppointmentServices.Find(appointmentId, serviceId);
            dbAppointmentService.ServiceId = appointmentService.ServiceId;
            dbAppointmentService.AppointmentId = appointmentService.AppointmentId;
            db.Entry(dbAppointmentService).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentServiceExists(appointmentId, serviceId))
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

        // POST: api/AppointmentServices
        [ResponseType(typeof(AppointmentService))]
        public IHttpActionResult PostAppointmentService(AppointmentService appointmentService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppointmentServices.Add(appointmentService);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AppointmentServiceExists(appointmentService.AppointmentId, appointmentService.ServiceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = appointmentService.AppointmentId }, new
            {
                appointmentService.AppointmentId,
                appointmentService.ServiceId
            });
        }

        // DELETE: api/AppointmentServices/5/5
        [ResponseType(typeof(AppointmentService))]
        [Route("api/AppointmentServices/{appointmentId}/{serviceId}")]
        public IHttpActionResult DeleteAppointmentService(int appointmentId, int serviceId)
        {
            AppointmentService appointmentService = db.AppointmentServices.Find(appointmentId, serviceId);
            if (appointmentService == null)
            {
                return NotFound();
            }

            db.AppointmentServices.Remove(appointmentService);
            db.SaveChanges();

            return Ok(new
            {
                appointmentService.AppointmentId,
                appointmentService.ServiceId
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

        private bool AppointmentServiceExists(int appointmentId, int serviceId)
        {
            return db.AppointmentServices.Count(e => e.AppointmentId == appointmentId && e.ServiceId == serviceId) > 0;
        }
    }
}