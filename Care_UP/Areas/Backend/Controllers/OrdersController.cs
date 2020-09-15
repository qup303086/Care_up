using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Care_UP.Models;
using MvcPaging;

namespace Care_UP.Areas.Backend.Controllers
{
    public class OrdersController : Controller
    {
        private Model1 db = new Model1();

        private const int DefaultPagerSize = 5;

        // GET: admin/Members
        public ActionResult Index(int? page) //int 接頁數第?頁
        {
            int user_page = page.HasValue ? page.Value - 1 : 0;
          
            DateTime? date_start = Session["date_start"] != null ? (DateTime?)Session["date_start"] : null;
            DateTime? date_end = Session["date_end"] != null ? (DateTime?)Session["date_end"] : null;

            var result = db.Orders.Where(x=>x.Status=="13"||x.Status=="02"); 
        
            if (date_start.HasValue && date_end.HasValue) //起始 結束都要有值(結束時間一定要多加一天)
            {
                date_end = date_end.Value.AddDays(1); //系統預設起始時間是從0:00 開始算EX:搜尋8/1-8/3 搜尋結果會是8/1 0:00 - 8/3 0:00
                result = result.Where(x => x.InitDate >= date_start && x.InitDate <= date_end);
            }
            return View(result.ToList().ToPagedList(user_page, DefaultPagerSize));
        }

        [HttpPost]
        public ActionResult Index(DateTime? date_start, DateTime? date_end)
        {
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

        // GET: Backend/Orders/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Backend/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
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
