using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Care_UP.Models;

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

            return Ok(order);
        }


        [Route("AttendantsGet10")]
        [HttpGet]
        public IHttpActionResult AttendantsGet10(int id)
        {
            var order = db.Orders.Where(x => x.AttendantId == id && x.Status == "10").ToList();
            return Ok(order);
        }

        [Route("CheckOrder")]
        [HttpGet]
        public IHttpActionResult CheckOrder(int id)
        {
            Orders order = db.Orders.Find(id);


            return Ok(new
            {
                order,
                AttendantsService =Utility.Service(order.Attendants.Service),
                AttendantsServiceTime=Utility.ServiceTime(order.Attendants.ServiceTime),
                EldersBody=Utility.EldersBody(order.Elders.Body),
                EldersEquipment =Utility.EldersEquipment(order.Elders.Equipment),
                EldersServiceItems = Utility.Service(order.Attendants.Service)
            });
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
                return Ok(new{ message = "已拒絕此訂單"});
            }
            else
            {
                return Ok(new{ message = "未填寫拒絕理由"});
            }
           
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