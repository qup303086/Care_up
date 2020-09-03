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
using Care_UP.Models;

namespace Care_UP.Controllers
{
    public class AttendantsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Attendants
        public IQueryable<Attendants> GetAttendants()
        {
            return db.Attendants;
        }

        // GET: api/Attendants/5
        [ResponseType(typeof(Attendants))]
        [Route("AttendantSearch")]
        [HttpGet]
        public HttpResponseMessage GetAttendants(int id)
        {
            Attendants attendants = db.Attendants.Find(id);
            if (attendants == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "無照服員資料" });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "有照服員資料" });
        }


        // PUT: api/Attendants/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAttendants(int id, Attendants attendants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attendants.Id)
            {
                return BadRequest();
            }

            db.Entry(attendants).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendantsExists(id))
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

        // POST: api/Attendants
        [ResponseType(typeof(Attendants))]
        public IHttpActionResult PostAttendants(Attendants attendants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attendants.Add(attendants);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = attendants.Id }, attendants);
        }

        // DELETE: api/Attendants/5
        [ResponseType(typeof(Attendants))]
        public IHttpActionResult DeleteAttendants(int id)
        {
            Attendants attendants = db.Attendants.Find(id);
            if (attendants == null)
            {
                return NotFound();
            }

            db.Attendants.Remove(attendants);
            db.SaveChanges();

            return Ok(attendants);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttendantsExists(int id)
        {
            return db.Attendants.Count(e => e.Id == id) > 0;
        }
    }
}