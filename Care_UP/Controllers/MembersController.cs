using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Care_UP.Models;
using Newtonsoft.Json;

namespace Care_UP.Controllers
{
    public class MembersController : ApiController
    {
        private Model1 db = new Model1();


        // POST: api/Members/
        [ResponseType(typeof(Members))]
        [Route("MemberRegister")]
        [HttpPost]
        public HttpResponseMessage PostMembers(Members members)
        {
            ModelState.Remove("PasswordSalt");
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "不完整" });
            }

            members.PasswordSalt = Utility.CreateSalt(); //產生密碼鹽
            members.Password = Utility.GenerateHashWithSalt(members.Password, members.PasswordSalt);//密碼+密碼鹽
            members.InitDate = DateTime.Now;
            db.Members.Add(members);

            db.SaveChanges();


            return Request.CreateResponse(HttpStatusCode.OK, new { result = "註冊成功" });
        }


        // POST: api/Members/
        [ResponseType(typeof(Members))]
        [Route("AttendantRegister")]
        [HttpPost]
        public HttpResponseMessage PostMembers(Attendants attendants)
        {
            ModelState.Remove("PasswordSalt");
            ModelState.Remove("Name");
            ModelState.Remove("Salary");
            ModelState.Remove("Account");
            ModelState.Remove("Service");
            ModelState.Remove("File");
            ModelState.Remove("ServiceTime");
            ModelState.Remove("Experience");
            ModelState.Remove("Status");
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "不完整" });
            }
            attendants.PasswordSalt = Utility.CreateSalt(); //產生密碼鹽
            attendants.Password = Utility.GenerateHashWithSalt(attendants.Password, attendants.PasswordSalt);//密碼+密碼鹽
            attendants.InitDate = DateTime.Now;
            db.Attendants.Add(attendants);

            db.SaveChanges();


            return Request.CreateResponse(HttpStatusCode.OK, new { result = "註冊成功" });
        }

        [Route("MemberLogin")]
        [HttpPost]
        [ResponseType(typeof(Members))]
        [AllowAnonymous]
        public HttpResponseMessage Login(Members login)
        {
            ModelState.Remove("PasswordSalt");
            if (ModelState.IsValid)
            {
                using (db)
                {
                    Members memberAccount = db.Members.FirstOrDefault(x => x.Email == login.Email);
                    if (memberAccount == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { message = "無此帳號" });
                    }
                    else
                    {
                        string psw = Utility.GenerateHashWithSalt(login.Password, memberAccount.PasswordSalt);
                        Members memeber = db.Members.FirstOrDefault(x => x.Email == memberAccount.Email && x.Password == psw);
                        if (memeber == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { message = "密碼錯誤" });
                        }
                        else
                        {
                            string newToken = new Token().GenerateToken(login.Id, login.Email);
                            return Request.CreateResponse(HttpStatusCode.OK, new
                            {
                                message = "登入成功",
                                token = newToken
                            });
                        }
                    }
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "帳密格式不符" });
            }
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