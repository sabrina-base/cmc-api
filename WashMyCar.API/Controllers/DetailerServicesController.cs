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
    public class DetailerServicesController : ApiController
    {
        private WashMyCarDataContext db = new WashMyCarDataContext();

        // PUT: api/DetailerServices/5/5
        [ResponseType(typeof(void))]
        [Route("api/detailerServices/{detailerId}/{serviceId}")]
        public IHttpActionResult PutDetailerService(int detailerId, int serviceId, DetailerService detailerService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (detailerId != detailerService.DetailerId || serviceId != detailerService.ServiceId)
            {
                return BadRequest();
            }

            var dbDetailerService = db.DetailerServices.Find(detailerId, serviceId);
            dbDetailerService.ServiceId = detailerService.ServiceId;
            dbDetailerService.DetailerId = detailerService.DetailerId;

            db.Entry(dbDetailerService).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetailerServiceExist(detailerId, serviceId))
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

        // POST: api/DetailerServices
        [ResponseType(typeof(DetailerService))]
        public IHttpActionResult PostDetailerService(DetailerService detailerService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DetailerServices.Add(detailerService);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DetailerServiceExist(detailerService.DetailerId, detailerService.ServiceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = detailerService.DetailerId }, new
            {
                detailerService.DetailerId,
                detailerService.ServiceId
            });
        }

        // DELETE: api/DetailerServices/5
        [ResponseType(typeof(DetailerService))]
        public IHttpActionResult DeleteDetailerService(int id)
        {
            DetailerService detailerService = db.DetailerServices.Find(id);
            if (detailerService == null)
            {
                return NotFound();
            }

            db.DetailerServices.Remove(detailerService);
            db.SaveChanges();

            return Ok(new
            {
                detailerService.DetailerId,
                detailerService.ServiceId
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

        private bool DetailerServiceExist(int detailerId, int serviceId)
        {
            return db.DetailerServices.Count(e => e.DetailerId == detailerId && e.ServiceId == serviceId) > 0;
        }
    }
}