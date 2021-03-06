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
            List<Elders> elder = db.Elders.Where(x => x.MemberId == id).ToList();
            if (elder.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "無須照護人員列表" });
            }
            else
            {
                var elders = elder.Select(x => new
                {
                    x,
                    EldersBody = Utility.ArrayToString(Utility.EldersBody(x.Body)),
                    EldersEquipment = Utility.ArrayToString(Utility.EldersEquipment(x.Equipment)),
                    EldersServiceItems = Utility.ArrayToString(Utility.Service(x.ServiceItems))
                });

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    elders
                });
            }
        }

        // GET: api/Elders/5
        //  [ResponseType(typeof(Elders))]
        [Route("ElderDetails")]
        [HttpGet]
        public HttpResponseMessage ElderDetails(int id)
        {
            Elders elders = db.Elders.Where(x => x.Id == id).FirstOrDefault();
            if (elders == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "無此筆記錄" });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                elders,
                EldersBody = Utility.EldersBody(elders.Body),
                EldersEquipment = Utility.EldersEquipment(elders.Equipment),
                EldersServiceItems = Utility.Service(elders.ServiceItems)
            });
        }

        // PUT: api/Elders/5
        [ResponseType(typeof(void))]
        [Route("EditElder")]
        [HttpPut]
        public HttpResponseMessage PutElders(Elders elders)
        {
            ModelState.Remove("InitDate");
            if (!ModelState.IsValid)
            {

                return Request.CreateResponse(HttpStatusCode.OK, ModelState);

            }
            db.Entry(elders).State = EntityState.Modified;

            elders.EditDate = DateTime.Now;
            db.SaveChanges();

            //try //可不加
            //{
            //db.SaveChanges();
            //   
            //}
            //catch (Exception e)
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, new { result = e.ToString() });
            //}
            //Elders eldersEdit = db.Elders.Where(x => x.Id == elders.Id).FirstOrDefault();


            //eldersEdit.Name = elders.Name;
            //eldersEdit


            return Request.CreateResponse(HttpStatusCode.OK, new { result = "更新成功" });
        }

        // POST: api/Elders
        //[ResponseType(typeof(Elders))]
        [Route("AddElder")]
        [HttpPost]
        public HttpResponseMessage PostElders(Elders elders)
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ModelState);

            }


            elders.InitDate = DateTime.Now;
            db.Elders.Add(elders);
            db.SaveChanges();

            Elders elder = db.Elders.FirstOrDefault(x => x.MemberId == elders.MemberId && x.Name == elders.Name);

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                result = "建立成功",
                elder
            });
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