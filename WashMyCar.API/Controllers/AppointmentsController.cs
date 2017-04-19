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
    public class AppointmentsController : ApiController
    {
        private Data.WashMyCarDataContext db = new Data.WashMyCarDataContext();

        // GET: api/Appointments
        public IHttpActionResult GetAppointments()
		{
			var resultSet = db.Appointments.Select(appointment => new
			{
				appointment.AppointmentId,
				appointment.AppointmentDate,
				appointment.CustomerId,
				appointment.DetailerId,
				appointment.VehicleTypeId,
                appointment.VehicleType.VehicleSize,
                appointment.Customer.FirstName,
                appointment.Customer.LastName,
                appointment.Customer.Address,
                appointment.Customer.Location
			}); 
            return Ok(resultSet);
        }

        // GET: api/Appointments/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult GetAppointment(int Id)
        {
            Appointment appointment = db.Appointments.Find(Id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(new
			{
				appointment.AppointmentId,
				appointment.AppointmentDate,
				appointment.CustomerId,
				appointment.DetailerId,
				appointment.VehicleTypeId,
                appointment.Customer.FirstName,
                appointment.Customer.LastName,
                appointment.Customer.Address,
                appointment.Customer.Location,
                appointment.VehicleType.VehicleSize
            });
        }

        // PUT: api/Appointments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppointment(int Id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id != appointment.AppointmentId)
            {
                return BadRequest();
            }

			var dbAppointment = db.Appointments.Find(Id);
			dbAppointment.AppointmentId = appointment.AppointmentId;
			dbAppointment.AppointmentDate = appointment.AppointmentDate;
			dbAppointment.CustomerId = appointment.CustomerId;
			dbAppointment.DetailerId = appointment.DetailerId;
			dbAppointment.VehicleTypeId = appointment.VehicleTypeId;
            dbAppointment.VehicleType.VehicleSize = appointment.VehicleType.VehicleSize;
            dbAppointment.Customer.FirstName = appointment.Customer.FirstName;
            dbAppointment.Customer.LastName = appointment.Customer.LastName;
            dbAppointment.Customer.Address = appointment.Customer.Address;
            dbAppointment.Customer.Location = appointment.Customer.Location;

            db.Entry(dbAppointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(Id))
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

        // POST: api/Appointments
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult PostAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentId }, new

			{
				appointment.AppointmentId,
				appointment.AppointmentDate,
				appointment.CustomerId,
				appointment.DetailerId,
				appointment.VehicleTypeId,
                appointment.Customer.FirstName,
                appointment.Customer.LastName,
                appointment.Customer.Address,
                appointment.Customer.Location,
                appointment.VehicleType.VehicleSize

            });
        }

        // DELETE: api/Appointments/5
	
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentId == id) > 0;
        }
    }
}