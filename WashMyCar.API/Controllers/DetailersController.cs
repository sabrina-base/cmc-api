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
    public class DetailersController : ApiController
    {
        private Data.WashMyCarDataContext db = new Data.WashMyCarDataContext();

        // GET: api/Detailers
        public IHttpActionResult GetDetailers()
        {
            var resultSet = db.Detailers.Select(detailer => new
            {
                detailer.DetailerId,
                detailer.Rating
            });

            return Ok(resultSet);
        }

        // GET: api/Detailers/5
        [Authorize, ResponseType(typeof(Detailer))]
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
                detailer.Rating
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
                detailer.Rating
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