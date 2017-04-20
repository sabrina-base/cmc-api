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
using WashMyCar.API.Utility;

namespace WashMyCar.API.Controllers
{
    public class DetailersController : ApiController
    {
        private Data.WashMyCarDataContext db = new Data.WashMyCarDataContext();

        // GET: api/Detailers
        public IHttpActionResult GetDetailers()
        {
            var resultSet = db.Detailers.ToArray().Select(detailer => new
            {
                detailer.DetailerId,
                detailer.Rating,
                detailer.Address,
                detailer.EmailAddress,
                detailer.Cellphone,
                detailer.FirstName,
                detailer.LastName,
                Availability = detailer.DetailerAvailabilities.Select(da => new
                {
                    DayOfWeek = da.DayOfWeek.Weekday,
                    da.DayOfWeekId,
                    da.DetailerId,
                    da.Start,
                    da.End
                }),
                Services = detailer.DetailerServices.Select(ds => new
                {
                    ds.Service.Cost,
                    ds.Service.ServiceType
                }),
                Location = new
                {
                    detailer.Location.Latitude,
                    detailer.Location.Longitude
                }
            });

            return Ok(resultSet);
        }

        // GET: api/Detailers/5
        [ResponseType(typeof(Detailer))]
        public IHttpActionResult GetDetailer(int id)
        {
            
            Detailer detailer = db.Detailers.Find(id);
            if (detailer == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                detailer.DetailerId,
                detailer.Rating,
                detailer.Address,
                detailer.EmailAddress,
                detailer.Cellphone,
                Availability = detailer.DetailerAvailabilities.Select(da => new
                {
                    DayOfWeek = da.DayOfWeek.Weekday,
                    da.Start,
                    da.DayOfWeekId,
                    da.DetailerId,
                    da.End
                }),
                detailer.FirstName,
                detailer.LastName,
                Services = detailer.DetailerServices.Select(ds => new
                {
                    ds.Service.Cost,
                    ds.Service.ServiceType
                }),
                Location = new
                {
                    detailer.Location.Latitude,
                    detailer.Location.Longitude
                }
            });
        }

        // PUT: api/Detailers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDetailer(int id, Detailer detailer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != detailer.DetailerId)
            {
                return BadRequest();
            }

            var dbDetailer = db.Detailers.Find(id);
            dbDetailer.DetailerId = detailer.DetailerId;
            dbDetailer.FirstName = detailer.FirstName;
            dbDetailer.LastName = detailer.LastName;
            dbDetailer.Address = detailer.Address;
            dbDetailer.EmailAddress = detailer.EmailAddress;
            // Loops over the incoming availabilities
            foreach (var Availability in detailer.DetailerAvailabilities)
            {
                // For each on grab databases version of availability
                var dbAvailability = dbDetailer.DetailerAvailabilities.FirstOrDefault(avail => avail.DetailerId == Availability.DetailerId && avail.DayOfWeekId == Availability.DayOfWeekId);
                // Update databases version with new info
                dbAvailability.Start = Availability.Start;
                dbAvailability.End = Availability.End;

            }
            dbDetailer.Cellphone = detailer.Cellphone;
            dbDetailer.Location = LocationConverter.GeocodeAddress(dbDetailer.Address);

            db.Entry(dbDetailer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailerExists(id))
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

        // POST: api/Detailers
        [ResponseType(typeof(Detailer))]
        public IHttpActionResult PostDetailer(Detailer detailer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Detailers.Add(detailer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = detailer.DetailerId }, new
            {
                detailer.DetailerId,
                detailer.Rating,
                detailer.Address,
                detailer.EmailAddress,
                Availability = detailer.DetailerAvailabilities.Select(da => new
                {
                    DayOfWeek = da.DayOfWeek.Weekday,
                    da.Start,
                    da.DayOfWeekId,
                    da.DetailerId,
                    da.End
                }),
                detailer.Cellphone,
                detailer.FirstName,
                detailer.LastName,
                Services = detailer.DetailerServices.Select(ds => new
                {
                    ds.Service.Cost,
                    ds.Service.ServiceType
                }),
                Location = new
                {
                    detailer.Location.Latitude,
                    detailer.Location.Longitude
                }
            });
            
        }

        // DELETE: api/Detailers/5
        [ResponseType(typeof(Detailer))]
        public IHttpActionResult DeleteDetailer(int id)
        {
            Detailer detailer = db.Detailers.Find(id);
            if (detailer == null)
            {
                return NotFound();
            }

            db.Detailers.Remove(detailer);
            db.SaveChanges();

            return Ok(detailer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DetailerExists(int id)
        {
            return db.Detailers.Count(e => e.DetailerId == id) > 0;
        }
    }
}