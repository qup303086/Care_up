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
    public class ShowSearchController : ApiController
    {
        private Model1 db = new Model1();

        [Route("SearchAttendant")]
        [HttpGet]
        public IHttpActionResult SearchAttendant()
        {
            int Id = 15;
            List<Cities> cities = db.Cities.ToList();
            List<Attendants> attendant = db.Attendants.Include(x => x.Locationses)
                .Where(x => x.Locationses.Where(y => y.CityId == Id).Count() > 0).ToList();

            return Ok(new
            {
                attendant,
                cities
            });
        }

        [System.Web.Http.Route("City")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult City(int Id)
        {
            List<Locations> locationses = db.Locations.Where(x=>x.CityId==Id).ToList();
            List<Attendants> attendant = db.Attendants.Include(x => x.Locationses)
                .Where(x => x.Locationses.Where(y => y.CityId == Id).Count() > 0).ToList();

            return Ok(new
            {
                attendant,
                locationses
            });
        }

        [Route("Location")]
        [HttpGet]
        public IHttpActionResult Location(int Id)
        {
            List<Attendants> attendant = db.Attendants.Include(x => x.Locationses)
                .Where(x => x.Locationses.Where(y => y.Id == Id).Count() > 0).ToList();

            return Ok(new
            {
                attendant
            });
        }


        [ResponseType(typeof(Orders))]
        [HttpGet]
        [Route("GetAttendantsFormSearch")]
        public HttpResponseMessage GetOrders(int id)//attendent.Id
        {
            List<Orders> orders = db.Orders.Where(x => x.AttendantId == id).ToList();

            DateTime Starttime = (DateTime)db.Orders.Select(x => x.StartDate).Min();
            DateTime Endtime = (DateTime)db.Orders.Select(x => x.EndDate).Max();

            TimeSpan alldate = Endtime - Starttime;
            List<string> date = new List<string>();
            for (int i = 0; i <= Convert.ToInt32(alldate.Days); i++)
            {
                date.Add(Starttime.AddDays(i).ToString("yyyy-MM-dd"));
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                日期 = date
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

        private bool AttendantsExists(int id)
        {
            return db.Attendants.Count(e => e.Id == id) > 0;
        }
    }
}