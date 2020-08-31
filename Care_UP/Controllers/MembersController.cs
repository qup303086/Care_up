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
    [RoutePrefix("Login")]
    public class MembersController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Members
        public IQueryable<Members> GetMembers()
        {
            return db.Members;
        }

        // GET: api/Members/5
        [ResponseType(typeof(Members))]
        public IHttpActionResult GetMembers(int id)
        {
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return NotFound();
            }

            return Ok(members);
        }

        // PUT: api/Members/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMembers(int id, Members members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != members.Id)
            {
                return BadRequest();
            }

            db.Entry(members).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembersExists(id))
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

        // POST: api/Members
        [ResponseType(typeof(Members))]
        [Route("Register")]
        [HttpPost]
        public HttpResponseMessage PostMembers(Members members)
        {
            ModelState.Remove("PasswordSalt"); //不驗證
            
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK,new { result = "不完整" });
            }
            members.PasswordSalt = Utility.CreateSalt(); //產生密碼鹽
            members.Password = Utility.GenerateHashWithSalt(members.Password, members.PasswordSalt);//密碼+密碼鹽
            members.InitDate = DateTime.Now;
            db.Members.Add(members);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "註冊成功" }); 
        }

        // DELETE: api/Members/5
        [ResponseType(typeof(Members))]
        public IHttpActionResult DeleteMembers(int id)
        {
            Members members = db.Members.Find(id);
            if (members == null)
            {
                return NotFound();
            }

            db.Members.Remove(members);
            db.SaveChanges();

            return Ok(members);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MembersExists(int id)
        {
            return db.Members.Count(e => e.Id == id) > 0;
        }
    }
}