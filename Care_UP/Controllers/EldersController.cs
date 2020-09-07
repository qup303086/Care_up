﻿using System;
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
    public class EldersController : ApiController
    {
        private Model1 db = new Model1();

        //GET: api/Elders
       [Route("GetElders")]
       [HttpGet]
       public HttpResponseMessage GetElders(int id)
       {
           List<Elders> elders = db.Elders.Where(x=>x.MemberId==id).ToList();
           if (elders.Count==0)
           {
               return Request.CreateResponse(HttpStatusCode.OK, new { result = "無欲照護的人員資料" });
           }

           return Request.CreateResponse(HttpStatusCode.OK, elders);
       }

        // GET: api/Elders/5
        //  [ResponseType(typeof(Elders))]
        [Route("ElderDetails")]
        public HttpResponseMessage ElderDetails(int id)
        {
            Elders elders = db.Elders.Where(x => x.Id == id).FirstOrDefault();
            if (elders == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "無此ID" });
            }

            return Request.CreateResponse(HttpStatusCode.OK, elders);
        }

        // PUT: api/Elders/5
        [ResponseType(typeof(void))]
        [Route("EditElder")]
        [HttpPut]
        public HttpResponseMessage PutElders(int id, Elders elders)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "沒有值" });
            }

            if (id != elders.Id)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ID不符合" });
            }

            Elders elders_Edit = db.Elders.Where(x => x.Id == elders.Id).FirstOrDefault();
            elders_Edit.Name = elders.Name;

            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "更新成功" });
        }

        // POST: api/Elders
        //[ResponseType(typeof(Elders))]
        [Route("AddElder")]
        [HttpPost]
        public HttpResponseMessage PostElders([FromBody] Elders elders)
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "不完整" });

            }

            db.Elders.Add(elders);
            db.SaveChanges();


            return Request.CreateResponse(HttpStatusCode.OK, new { result = "註冊成功" });
        }

        // DELETE: api/Elders/5
        [ResponseType(typeof(Elders))]
        [Route("DeleteElder")]
        [HttpDelete]
        public HttpResponseMessage DeleteElders(int id)
        {

            Elders elders = db.Elders.Find(id);
            if (elders == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "刪除失敗" });

            }

            db.Elders.Remove(elders);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "刪除成功" });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EldersExists(int id)
        {
            return db.Elders.Count(e => e.Id == id) > 0;
        }
    }
}