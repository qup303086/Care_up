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

            var attendants = attendant.Select(x => new
            {
                x,
                服務項目 = Utility.Service(x.Service),
                服務時段 = Utility.ServiceTime(x.ServiceTime)
            });

            return Ok(new
            {
                attendants,
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

            var attendants = attendant.Select(x => new
            {
                x,
                服務項目 = Utility.Service(x.Service),
                服務時段 = Utility.ServiceTime(x.ServiceTime)
            });

            return Ok(new
            {
                attendants,
                locationses
            });
        }

        [Route("Location")]
        [HttpGet]
        public IHttpActionResult Location(int Id)
        {
            List<Attendants> attendant = db.Attendants.Include(x => x.Locationses)
                .Where(x => x.Locationses.Where(y => y.Id == Id).Count() > 0).ToList();
            if (attendant.Count==0)
            {
                return Ok(new
                {
                    message = "此地區尚無照護員"
                });
            }
            else
            {
                var attendants = attendant.Select(x => new
                {
                    x,
                    服務項目 = Utility.Service(x.Service),
                    服務時段 = Utility.ServiceTime(x.ServiceTime)

                });
                
                return Ok(new
                {
                    attendants
                });
            }
         
        }
        
        
        [ResponseType(typeof(Orders))]
        [HttpGet]
        [Route("GetAttendat")]
        public HttpResponseMessage GetAttendat(int id)//attendent.Id
        {
            Attendants attendantDetails = db.Attendants.Include(x => x.Locationses).Where(x => x.Id == id).FirstOrDefault();
            var area = attendantDetails.Locationses.GroupBy(x => x.Area).Select(x => new
            {
                x.Key,
                city = x.Where(y => y.Area == x.Key).Select(y => y.Cities)
            });

            List<Orders> orders = db.Orders.Where(x => x.AttendantId == id).ToList();
            var attendant = orders.Where(x => x.AttendantId == id).GroupBy(x => x.Id).Select(x => new
            {
                Id = x.Key,
                star = x.Where(y => y.Id == x.Key).Select(y => y.Star).Average()
            });
            double? sum = 0;
            int star = 0;
            if (attendant.Count() != 0)
            {
                foreach (var item in attendant)
                {
                    if (item.star != null)
                    {
                        sum += item.star;
                    }
                }
                star = Convert.ToInt32(sum / attendant.Count());
            }

            //DateTime Starttime = (DateTime)db.Orders.Where(x => x.AttendantId == id).Select(x => x.StartDate).Min();
            //DateTime Endtime = (DateTime)db.Orders.Where(x => x.AttendantId == id).Select(x => x.EndDate).Max();

            //TimeSpan Alldate = Endtime - Starttime;
            //List<string> date = new List<string>();
            //for (int i = 0; i <= Convert.ToInt32(Alldate.Days); i++)
            //{
            //    date.Add(Starttime.AddDays(i).ToString("yyyy-MM-dd"));
            //}

            List<string> date = new List<string>();
            var orderDate = orders.Where(x=>x.Status=="10"||x.Status=="11"||x.Status=="12"||x.Status=="22")
                .Select(x => new
            {
                x.StartDate,
                x.EndDate
            });
            if (orderDate.Count()!=0)
            {
                foreach (var item in orderDate)
                {
                    TimeSpan Alldate = item.EndDate - item.StartDate;
                    for (int i = 0; i <= Convert.ToInt32(Alldate.Days); i++)
                    {
                        date.Add(item.StartDate.AddDays(i).ToString("yyyy-MM-dd"));
                    }
                }
                
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                attendantDetails,
                服務項目 = Utility.Service(attendantDetails.Service),
                服務時段 = Utility.ServiceTime(attendantDetails.ServiceTime),
                已被預約的日期 = date,
                star,
                area
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