using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.UI.WebControls;
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
            List<Locations> locationses = db.Locations.Where(x => x.CityId == Id).ToList();

            List<Attendants> attendant = db.Attendants.Include(x => x.Locationses)
                .Where(x => x.Locationses.Where(y => y.CityId == Id).Count() > 0).ToList();

            List<Orders> allOrderses = db.Orders.ToList();
            
            var attendants = attendant.Where(x => x.Status == "02").Select(x => new
            {
                attendantId = x.Id,
                name = x.Name,
                salary = x.Salary,
                experience = x.Experience,
                photo = x.Photo,
                file = x.File,
                服務項目 = Utility.Service(x.Service),
                服務時段 = Utility.ServiceTime(x.ServiceTime),
                count = allOrderses.Where(z=>z.AttendantId==x.Id).Where(z=>z.Comment!=null&&z.Star!=null).Count(),
                star = Utility.Star(allOrderses.Where(y => y.AttendantId == x.Id).Select(y => y.Star).Average())
            }).ToList();

            return Ok(new
            {
                attendants,
                cities,
                locationses
            });
        }

        [System.Web.Http.Route("City")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult City(int Id)
        {
            List<Locations> locationses = db.Locations.Where(x => x.CityId == Id).ToList();

            List<Attendants> attendant = db.Attendants.Include(x => x.Locationses)
                .Where(x => x.Locationses.Where(y => y.CityId == Id).Count() > 0).ToList();

            if (attendant.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無照服員登記於此地區服務"
                });
            }

            List<Orders> allOrderses = db.Orders.ToList();

            var attendants = attendant.Where(x => x.Status == "02").Select(x => new
            {
                attendantId = x.Id,
                name = x.Name,
                salary = x.Salary,
                experience = x.Experience,
                photo = x.Photo,
                file = x.File,
                服務項目 = Utility.Service(x.Service),
                服務時段 = Utility.ServiceTime(x.ServiceTime),
<<<<<<< HEAD
                star = Utility.Star(allOrderses.Where(y => y.AttendantId == x.Id).Select(y => y.Star).Average())
            }).ToList();
=======
                count = allOrderses.Where(z => z.AttendantId == x.Id).Where(z => z.Comment != null && z.Star != null).Count(),
                star =Utility.Star(allOrderses.Where(y => y.AttendantId == x.Id).Select(y => y.Star).Average()) 
                }).ToList();
>>>>>>> master

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
            if (attendant.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無照服員登記於此地區服務"
                });
            }
            List<Orders> allOrderses = db.Orders.ToList();

            var attendants = attendant.Where(x=>x.Status=="02").Select(x => new
            {
                attendantId = x.Id,
                name = x.Name,
                salary = x.Salary,
                experience = x.Experience,
                photo = x.Photo,
                file = x.File,
                服務項目 = Utility.Service(x.Service),
                服務時段 = Utility.ServiceTime(x.ServiceTime),
                count = allOrderses.Where(z => z.AttendantId == x.Id).Where(z => z.Comment != null && z.Star != null).Count(),
                star = Utility.Star(allOrderses.Where(y => y.AttendantId == x.Id).Select(y => y.Star).Average())
            }).ToList();

            return Ok(new
            {
                attendants
            });

        }


        [ResponseType(typeof(Orders))]
        [HttpGet]
        [Route("SelectAttendant")]
        public HttpResponseMessage SelectAttendant(int id)//attendent.Id
        {
            Attendants attendantDetails = db.Attendants.Include(x => x.Locationses).Where(x => x.Id == id).FirstOrDefault();
            List<Orders> orders = db.Orders.Where(x => x.AttendantId == id).ToList();
            var area = attendantDetails.Locationses.GroupBy(x => x.Area).Select(x => new
            {
                x.Key,
                city = x.Where(y => y.Area == x.Key).Select(y => y.Cities),
                star= Utility.Star(orders.Select(y => y.Star).Average())
            });
            
            List<string> date = new List<string>();
            var orderDate = orders.Where(x => x.Status == OrderType.等待照服員確認訂單 ||x.Status == OrderType.待付款 ||x.Status == OrderType.已付款 ||x.Status == OrderType.服務進行中)
                .Select(x => new
                {
                    x.StartDate,
                    x.EndDate
                });
            if (orderDate.Count() != 0)
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

            var allcomment = orders.Where(x => x.Star != null && x.Comment != null).Select(comments => new
            {
                memeber =  comments.Elders.Members.Email.Substring(0,4)+"xxxxxxx",
                star = comments.Star,
                comment = comments.Comment
            }).ToList();

            var quiz = db.Questions.Include(x=>x.QuestionAnswers).Where(x=>x.AttendantId==id).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                attendantDetails,
                count = orders.Where(z => z.Comment != null && z.Star != null).Count(),
                服務項目 = Utility.Service(attendantDetails.Service),
                服務時段 = Utility.ServiceTime(attendantDetails.ServiceTime),
                已被預約的日期 = date,
                area,
                allcomment,
                quiz
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