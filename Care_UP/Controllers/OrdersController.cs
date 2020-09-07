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

        // GET: api/Orders
        public IQueryable<Orders> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Orders))]
        [HttpGet]
        [Route("getdate")]
        public HttpResponseMessage GetOrders(int id)//attendent.Id
        {
            List<Orders> orders = db.Orders.Where(x => x.AttendantId == id).ToList();
           
            DateTime Starttime = (DateTime)db.Orders.Select(x=>x.StartDate).Min();
            DateTime Endtime= (DateTime)db.Orders.Select(x => x.EndDate).Max();

            TimeSpan alldate = Endtime-Starttime;
            List<string> date = new List<string>();
            for (int i = 0;i<=Convert.ToInt32(alldate.Days);i++)
            {
                date.Add(Starttime.AddDays(i).ToString("yyyy-MM-dd"));
            }
            

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
               日期 =  date
            });
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        [Route("CancelOrder")]
        [HttpPut]
        public HttpResponseMessage PutOrders(Orders orders)
        {
          

            db.Entry(orders).State = EntityState.Modified;
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
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "訂單不完整" });

            }
            orders.Total = Convert.ToInt32(tsDate.Days) * attendants.Salary;

            orders.InitDate = DateTime.Now;
            orders.Status = "10";
            db.Orders.Add(orders);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "訂單成立" });
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Orders))]
        public IHttpActionResult DeleteOrders(int id)
        {
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return NotFound();
            }

            db.Orders.Remove(orders);
            db.SaveChanges();

            return Ok(orders);
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