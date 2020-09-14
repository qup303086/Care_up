using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Care_UP.Models;
using Microsoft.Ajax.Utilities;

namespace Care_UP.Controllers
{
    public class OrdersController : ApiController
    {
        private Model1 db = new Model1();


        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        [Route("CancelOrder")]
        [HttpPatch]
        public HttpResponseMessage PatchOrders(int Id)
        {

            Orders orders = db.Orders.Find(Id);

            orders.Status = "01";
            orders.EditDate = DateTime.Now;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "訂單取消成功" });
        }

        // POST: api/Orders
        [ResponseType(typeof(Orders))]
        [Route("AddOrder")]
        [HttpPost]
        public HttpResponseMessage PostOrders(Orders orders)
        {

            ModelState.Remove("Status");
            DateTime startDate = (DateTime)orders.StartDate;
            DateTime endDate = (DateTime)orders.EndDate;
            TimeSpan tsDate = endDate - startDate;

            Attendants attendants = db.Attendants.Find(orders.AttendantId);

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ModelState);

            }
            orders.Total = Convert.ToInt32(tsDate.Days) * attendants.Salary;

            orders.InitDate = DateTime.Now;
            orders.Status = "10";
            db.Orders.Add(orders);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "訂單成立" });
        }



        [Route("MemberGet10")]
        [HttpGet]
        public IHttpActionResult MemberGet10(int id)
        {
            var order = db.Orders.Where(x => x.Elders.MemberId == id && x.Status == "10").ToList();
            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "目前尚無未確認訂單"
                });
            }
            
            var orders = order.Select(x => new
            {
                x,
                startDate = x.StartDate.ToString("yyyy-MM-dd"),
                endDate = x.EndDate.ToString("yyyy-MM-dd"),
                OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd")
            });
            return Ok(orders);

        }

        [Route("AttendantsOrder01")]
        [HttpGet]
        public IHttpActionResult AttendantsGet10(int id)
        {
            var order = db.Orders.Where(x => x.AttendantId == id && x.Status == "10").ToList();
            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "目前尚無未確認訂單"
                });
            }

            var orders = order.Select(x => new
            {
                x,
                startDate = x.StartDate.ToString("yyyy-MM-dd"),
                endDate = x.EndDate.ToString("yyyy-MM-dd"),
                OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd")
            });
            return Ok(orders);

        }



        [Route("MemberOrder02")]
        [HttpGet]
        public IHttpActionResult MemberGet11(int id)
        {
            List<Orders> ordes = db.Orders.Where(x => x.Elders.MemberId == id).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無待處理訂單"
                });
            }
            List<Orders> order11 = ordes.Where(x => x.Status == "11").ToList();
            foreach (Orders item in order11)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = "19";
                }
            }
            List<Orders> order12 = ordes.Where(x => x.Status == "12").ToList();
            foreach (Orders item in order12)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = "22";
                }
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.Elders.MemberId == id).Where(x => x.Status == "11" || x.Status == "12").ToList();

            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無待處理訂單"
                });
            }
            else
            {
                var orders = order.Select(x => new
                {
<<<<<<< HEAD

                    startDate = x.StartDate.ToString("yyyy-MM-dd"),
                    endDate =x.EndDate.ToString("yyyy-MM-dd"),
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd")
=======
                    x,
                    startDate = x.StartDate.ToString("yyyy-MM-dd"),
                    endDate = x.EndDate.ToString("yyyy-MM-dd")
>>>>>>> featureping

                });
                return Ok(orders);
            }
        }
        [Route("AttendantsOrder02")]
        [HttpGet]
        public IHttpActionResult AttendantsGet11(int id)
        {
            List<Orders> ordes = db.Orders.Where(x => x.AttendantId == id).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無待處理訂單"
                });
            }
            List<Orders> order11 = ordes.Where(x => x.Status == "11").ToList();
            foreach (Orders item in order11)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = "19";
                }
            }
            List<Orders> order12 = ordes.Where(x => x.Status == "12").ToList();
            foreach (Orders item in order12)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = "22";
                }
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.AttendantId == id).Where(x => x.Status == "11" || x.Status == "12").ToList();

            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無待處理訂單"
                });
            }
            else
            {
                var orders = order.Select(x => new
                {
                    x,
                    startDate = x.StartDate.ToString("yyyy-MM-dd"),
                    endDate = x.EndDate.ToString("yyyy-MM-dd"),
<<<<<<< HEAD
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
                    OrderStatus = Utility.OrderStatus(x.Status)
=======
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd")
>>>>>>> featureping
                });
                return Ok(orders);
            }
        }

<<<<<<< HEAD


        [Route("MemberOrder03")]
=======
      
        [Route("MemberOrder02")]
>>>>>>> featureping
        [HttpGet]
        public IHttpActionResult MemberGet22(int id)
        {
<<<<<<< HEAD
            List<Orders> ordes = db.Orders.Where(x => x.Elders.MemberId == id).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無進行中訂單"
                });
            }
            List<Orders> to22 = ordes.Where(x => x.Status == "12").ToList();
            foreach (Orders item in to22)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = "22";
                }
            }

            List<Orders> to13 = ordes.Where(x => x.Status == "22").ToList();
            foreach (Orders item in to13)
            {
                if (DateTime.Compare(DateTime.Now, item.EndDate) > 0)
                {
                    item.Status = "13";
=======
            List<Orders> ordes = db.Orders.Where(x => x.Elders.MemberId== id && x.Status == "12").ToList();
            foreach (Orders item in ordes)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = "21";
>>>>>>> featureping
                }
            }
            db.SaveChanges();

<<<<<<< HEAD
            var order = db.Orders.Where(x => x.Elders.MemberId == id && x.Status == "22").ToList();
=======
            var order = db.Orders.Where(x => x.Elders.MemberId == id).Where(x=>x.Status == "11"||x.Status=="12").ToList();
>>>>>>> featureping

            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無進行中訂單"
                });
            }
            else
            {
                var orders = order.Select(x => new
                {
                    x,
                    startDate = x.StartDate.ToString("yyyy-MM-dd"),
                    endDate = x.EndDate.ToString("yyyy-MM-dd"),
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
                    OrderStatus = Utility.OrderStatus(x.Status)
                });
                return Ok(orders);
            }
        }
        [Route("AttendantsOrder03")]
        [HttpGet]
        public IHttpActionResult AttendantsGet22(int id)
        {
<<<<<<< HEAD
            List<Orders> ordes = db.Orders.Where(x => x.AttendantId == id).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無進行中訂單"
                });
            }
            List<Orders> to22 = ordes.Where(x => x.Status == "12").ToList();
            foreach (Orders item in to22)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = "22";
                }
            }

            List<Orders> to13 = ordes.Where(x => x.Status == "22").ToList();
            foreach (Orders item in to13)
            {
                if (DateTime.Compare(DateTime.Now, item.EndDate) > 0)
                {
                    item.Status = "13";
=======
            List<Orders> ordes = db.Orders.Where(x => x.AttendantId == id && x.Status == "12").ToList();
            foreach (Orders item in ordes)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate)>0)
                {
                    item.Status = "21";
>>>>>>> featureping
                }
            }
            db.SaveChanges();

<<<<<<< HEAD
            var order = db.Orders.Where(x => x.AttendantId == id && x.Status == "22")
                .ToList();
=======
            var order = db.Orders.Where(x => x.AttendantId == id).Where(x => x.Status == "11" || x.Status == "12").ToList();
>>>>>>> featureping

            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無進行中訂單"
                });
            }
            else
            {
                var orders = order.Select(x => new
                {
                    x,
                    startDate = x.StartDate.ToString("yyyy-MM-dd"),
                    endDate = x.EndDate.ToString("yyyy-MM-dd"),
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
                    OrderStatus = Utility.OrderStatus(x.Status)
                });
                return Ok(orders);
            }
        }


        [Route("MemberOrder04")]
        [HttpGet]
        public IHttpActionResult MemberGet13(int id)
        {
            List<Orders> orders = db.Orders.Where(x => x.Elders.MemberId == id && x.Comment == null)
                .Where(x => x.Status == "13" || x.Status == "02").ToList();
            if (orders.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無需要填寫評價的訂單"
                });
            }
            return Ok(orders);
        }
        [Route("AttendantsOrder04")]
        [HttpGet]
        public IHttpActionResult AttendantsGet13(int id)
        {
            List<Orders> orders = db.Orders.Where(x => x.AttendantId == id && x.Status == "13").ToList();
            if (orders.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無未匯款訂單"
                });
            }

            var order = orders.Select(x => new
            {
                x,
                status = Utility.Attendant04Status(x.Status)
            });
            return Ok(order);
        }





        [Route("CheckOrder")]
        [HttpGet]
        public IHttpActionResult CheckOrder(int id)
        {
            Orders order = db.Orders.Find(id);

            DateTime Starttime = (DateTime)db.Orders.Select(x => x.StartDate).Min();
            DateTime Endtime = (DateTime)db.Orders.Select(x => x.EndDate).Max();

            TimeSpan Alldate = Endtime - Starttime;
            List<string> date = new List<string>();
            for (int i = 0; i <= Convert.ToInt32(Alldate.Days); i++)
            {
                date.Add(Starttime.AddDays(i).ToString("yyyy-MM-dd"));
            }

            return Ok(new
            {
                order, 
                AttendantsService = Utility.Service(order.Attendants.Service),
                AttendantsServiceTime = Utility.ServiceTime(order.Attendants.ServiceTime),
                date,
                EldersBody = Utility.EldersBody(order.Elders.Body),
                EldersEquipment = Utility.EldersEquipment(order.Elders.Equipment),
                EldersServiceItems = Utility.Service(order.Attendants.Service)
            });
        }

        [Route("OrderAccept")]
        [HttpPatch]
        public IHttpActionResult OrderAccept(int id)
        {
            Orders order = db.Orders.Find(id);
            order.Status = "11";
            order.EditDate = DateTime.Now;
            db.SaveChanges();
            return Ok(new { message = "已接受此訂單" });
        }

        [Route("OrderReject")]
        [HttpPatch]
        public IHttpActionResult OrderReject(OrderReject orderReject)
        {
            if (!string.IsNullOrWhiteSpace(orderReject.Cancel))
            {
                Orders order = db.Orders.Find(orderReject.Id);
                order.Cancel = orderReject.Cancel;
                order.Status = "01";
                order.EditDate = DateTime.Now;
                db.SaveChanges();
                return Ok(new { message = "已拒絕此訂單" });
            }
            else
            {
                return Ok(new { message = "未填寫拒絕理由" });
            }
        }


        [HttpPatch]
        [Route("FillinComment")]
        public HttpResponseMessage FillinComment(FillinCommentView FillinComment)
        {
            ModelState.Remove("EditDate");
            if (FillinComment.Comment==null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "評價未填寫" });
            }
            if (FillinComment.Star == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "未給星星" });
            }
            Orders orders = db.Orders.Find(FillinComment.Id);
            orders.Comment = FillinComment.Comment;
            orders.Star = FillinComment.Star;
            orders.Status = "02";
            orders.EditDate = DateTime.Now;
            db.Entry(orders).State = EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, new { message = "評價填寫完畢" });

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdersExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}