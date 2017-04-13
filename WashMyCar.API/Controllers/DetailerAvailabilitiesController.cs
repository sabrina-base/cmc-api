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
    public class DetailerAvailabilitiesController : ApiController
    {
        private WashMyCarDataContext db = new WashMyCarDataContext();

        // PUT: api/DetailerAvailabilities/5/5
        [ResponseType(typeof(void))]
		[Route("api/DetailerAvailability/{detailerId}/{serviceId}")]

		public IHttpActionResult PutDetailerAvailability(int detailerId, int dayOfWeekId, DetailerAvailability detailerAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (detailerId != detailerAvailability.DetailerId || dayOfWeekId != detailerAvailability.DayOfWeekId)
            {
                return BadRequest();
            }

			var dbDetailerAvailability = db.DetailerAvailabilities.Find(detailerId, dayOfWeekId);

			dbDetailerAvailability.Start = detailerAvailability.Start;
			dbDetailerAvailability.End = detailerAvailability.End;
			dbDetailerAvailability.Multiplier = detailerAvailability.Multiplier;

            db.Entry(dbDetailerAvailability).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailerAvailabilityExists(detailerId, dayOfWeekId))
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

        // POST: api/DetailerAvailabilities
        [ResponseType(typeof(DetailerAvailability))]
		[Route("api/DetailerAvailability/{detailerId}/{serviceId}")]

		public IHttpActionResult PostDetailerAvailability(DetailerAvailability detailerAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DetailerAvailabilities.Add(detailerAvailability);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DetailerAvailabilityExists(detailerAvailability.DetailerId, detailerAvailability.DayOfWeekId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = detailerAvailability.DetailerId }, new
			{
				detailerAvailability.DetailerId,
				detailerAvailability.DayOfWeekId,
				detailerAvailability.Detailer,
				detailerAvailability.DayOfWeek,
				detailerAvailability.Start,
				detailerAvailability.End,
				detailerAvailability.Multiplier
			});
        }

        // DELETE: api/DetailerAvailabilities/5/5
        [ResponseType(typeof(DetailerAvailability))]
		[Route("api/DetailerAvailability/{detailerId}/{serviceId}")]

		public IHttpActionResult DeleteDetailerAvailability(int detailerId, int dayOfWeekId)
        {
            DetailerAvailability detailerAvailability = db.DetailerAvailabilities.Find(detailerId, dayOfWeekId);
            if (detailerAvailability == null)
            {
                return NotFound();
            }

            db.DetailerAvailabilities.Remove(detailerAvailability);
            db.SaveChanges();

            return Ok(new
			{
				detailerAvailability.DetailerId,
				detailerAvailability.DayOfWeekId,
				detailerAvailability.Detailer,
				detailerAvailability.DayOfWeek,
				detailerAvailability.Start,
				detailerAvailability.End,
				detailerAvailability.Multiplier
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

        private bool DetailerAvailabilityExists(int detailerId, int dayOfWeekId)
        {
            return db.DetailerAvailabilities.Count(e => e.DetailerId == detailerId && e.DayOfWeekId == dayOfWeekId) > 0;
        }
    }
}