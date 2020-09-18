using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Care_UP.Models;
using Microsoft.Ajax.Utilities;
using MvcPaging;

namespace Care_UP.Areas.Backend.Controllers
{
    public class OrdersController : Controller
    {
        private Model1 db = new Model1();

        private const int DefaultPagerSize = 10;

        // GET: admin/Members
        public ActionResult Index(int? page) //int 接頁數第?頁
        {

            var result = db.Orders.OrderByDescending(x=>x.InitDate).AsQueryable();
            int user_page = page.HasValue ? page.Value - 1 : 0;
            OrderType? order = Session["order"] != null ? (OrderType?)Session["order"] : null;
            string keyword = Session["keyword"] != null ? Session["keyword"].ToString() : null;
            DateTime? date_start = Session["date_start"] != null ? (DateTime?)Session["date_start"] : null;
            DateTime? date_end = Session["date_end"] != null ? (DateTime?)Session["date_end"] : null;

            DateTime? starttime = Session["starttime"] != null ? (DateTime?)Session["starttime"] : null;
            DateTime? endtime = Session["endtime"] != null ? (DateTime?)Session["endtime"] : null;

            //Where(x=>x.Status=="13"||x.Status=="02"); 
            if (order.HasValue)
            {
                result = result.Where(x => x.Status == order);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                result = result.Where(x => x.Elders.Name.Contains(keyword)||x.Attendants.Email.Contains(keyword));//Contains =SQL like %%
            }

            if (date_start.HasValue && date_end.HasValue) 
            {
                date_end = date_end.Value.AddDays(1); 
                result = result.Where(x => x.InitDate >= date_start && x.InitDate <= date_end);
            }
            if (starttime.HasValue && endtime.HasValue) 
            {
                endtime = endtime.Value.AddDays(1); 
                result = result.Where(x => x.StartDate >= starttime && x.EndDate <= endtime);
            }

            //var status = statusview;
            //var pay = payview;
            //if (status=="照服員待收款"||status=="服務進行中")
            //{

            //}
            return View(result.ToPagedList(user_page, DefaultPagerSize));
        }

        [HttpPost]
        public ActionResult Index(OrderType? order,string keyword, DateTime? date_start, DateTime? date_end)
        {
            Session["keyword"] = keyword;
            Session["order"] = order;
            Session["date_start"] = date_start;
            Session["date_end"] = date_end;
            return RedirectToAction("Index");
        }

        // GET: Backend/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }




        // GET: Backend/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.AttendantId = new SelectList(db.Attendants, "Id", "Email", orders.AttendantId);
            ViewBag.ElderId = new SelectList(db.Elders, "Id", "Name", orders.ElderId);
            return View(orders);
        }

        // POST: Backend/Orders/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ElderId,AttendantId,date_start,date_end,StopDate,Total,Comment,Star,Cancel,InitDate,EditDate,Status,StartDate,EndDate")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AttendantId = new SelectList(db.Attendants, "Id", "Email", orders.AttendantId);
            ViewBag.ElderId = new SelectList(db.Elders, "Id", "Name", orders.ElderId);
            return View(orders);
        }

        // GET: Backend/Orders/Finish/5
        public ActionResult Finish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Backend/Orders/Finish/5
        [HttpPost, ActionName("Finish")]
        [ValidateAntiForgeryToken]
        public ActionResult FinishConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Entry(orders).State = EntityState.Modified;
            orders.Status = OrderType.已完成;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
