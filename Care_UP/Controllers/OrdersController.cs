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
        public IHttpActionResult GetOrders(int id)
        {
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        [Route("EditOder")]
        [HttpPut]
        public HttpResponseMessage PutOrders(Orders orders)
        {

            db.Entry(orders).State = EntityState.Modified;
            orders.EditDate = DateTime.Now;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, new { result = "訂單取消成功" });

        }

        // POST: api/Orders
        [ResponseType(typeof(Orders))]
        [Route("AddOrder")]
        [HttpPost]
        public HttpResponseMessage PostOrders(Orders orders,Attendants attendants,Elders elders)
        {
            DateTime startDate = (DateTime)orders.StartDate;
            DateTime endDate = (DateTime) orders.EndDate;
            TimeSpan tsDate = startDate - endDate;
            orders.Total = tsDate.Days * attendants.Salary;


            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "訂單不完整" });

            }
            orders.InitDate = DateTime.Now;
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