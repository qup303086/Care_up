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

            orders.Status = OrderType.已取消;
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
            DateTime endDate = (DateTime)orders.EndDate.AddDays(1);
            TimeSpan tsDate = endDate - startDate;

            Attendants attendants = db.Attendants.Find(orders.AttendantId);

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ModelState);

            }
            orders.Total = Convert.ToInt32(tsDate.Days) * attendants.Salary;

            orders.InitDate = DateTime.Now;
            orders.Status = OrderType.等待照服員確認訂單;
            db.Orders.Add(orders);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new { result = "訂單成立" });
        }



        [Route("MemberOrderStatus")]
        [HttpGet]
        public IHttpActionResult MemberOrderStatus(int id)//家屬id
        {
            int[] status = new int[5];
            List<Orders> orderID = db.Orders.Where(x => x.Elders.MemberId == id).ToList();


            var order1 = orderID.Where(x => x.Status == OrderType.等待照服員確認訂單).ToList();
            status[0] = order1.Count;


            var order2 = orderID.Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.已付款).ToList();
            status[1] = order2.Count;


            var order3 = orderID.Where(x => x.Elders.MemberId == id).Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.服務進行中).ToList();
            status[2] = order3.Count;


            var order4 = orderID.Where(x => x.Star == null)
                .Where(x => x.Status == OrderType.已完成 || x.Status == OrderType.已完成).ToList();
            status[3] = order4.Count;


            var order5 = orderID.Where(x => x.Star != null)
                 .Where(x => x.Status == OrderType.已取消 || x.Status == OrderType.已完成 || x.Status == OrderType.中斷 || x.Status == OrderType.待退款 || x.Status == OrderType.照服員拒接)
                 .ToList();
            status[4] = order5.Count;

            //db.SaveChanges();
            return Ok(new
            {
                count = status
            });
        }

        [Route("AttendantsOrderStatus")]
        [HttpGet]
        public IHttpActionResult AttendantsOrderStatus(int id) //照服員id
        {
            int[] status = new int[5];
            List<Orders> orderID= db.Orders.Where(x => x.AttendantId == id).ToList();

            var order1 = orderID.Where(x =>x.Status == OrderType.等待照服員確認訂單).ToList();

            status[0] = order1.Count;


            var orders2 = orderID.Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.已付款).ToList();
            status[1] = orders2.Count;


            var orders3 = orderID.Where(x => x.Status == OrderType.已付款 || x.Status == OrderType.服務進行中).ToList();
            status[2] = orders3.Count;


            var orders4 = orderID.Where(x => x.Status == OrderType.待評價).ToList();
            status[3] = orders4.Count;


            var orders5 = orderID
                .Where(x => x.Status == OrderType.已取消 || x.Status == OrderType.已完成 || x.Status == OrderType.中斷 || x.Status == OrderType.待退款 || x.Status == OrderType.照服員拒接)
                .ToList();
            status[4] = orders5.Count;

            //db.SaveChanges();
            return Ok(new
            {
                count = status
            });
        }



        [Route("MemberOrder01")]
        [HttpGet]
        public IHttpActionResult MemberGet10(int id)
        {
            List<Orders> order = db.Orders.Where(x => x.Elders.MemberId == id && x.Status == OrderType.等待照服員確認訂單).ToList();
            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "目前尚無未確認訂單"
                });
            }
            foreach (Orders item in order)
            {
                //訂單的開始日期前3天還不接==拒接
                if (DateTime.Compare(DateTime.Now, item.StartDate.AddDays(-3)) > 0)
                {
                    item.Status = OrderType.照服員拒接;
                }
            }

            db.SaveChanges();
            var orders = order.Select(x => new
            {
                x,
                startDate = x.StartDate.ToString("yyyy-MM-dd"),
                endDate = x.EndDate.ToString("yyyy-MM-dd"),
                OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
            });
            return Ok(new
            {
                orders,
                count = orders.Count()
            });

        }

        [Route("AttendantsOrder01")]
        [HttpGet]
        public IHttpActionResult AttendantsGet10(int id)
        {
            List<Orders> order = db.Orders.Where(x => x.AttendantId == id && x.Status == OrderType.等待照服員確認訂單).ToList();
            if (order.Count == 0)
            {
                return Ok(new
                {
                    message = "目前尚無未確認訂單"
                });
            }
            foreach (Orders item in order)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate.AddDays(-3)) > 0)
                {
                    item.Status = OrderType.照服員拒接;
                }
            }

            var orders = order.Select(x => new
            {
                x,
                startDate = x.StartDate.ToString("yyyy-MM-dd"),
                endDate = x.EndDate.ToString("yyyy-MM-dd"),
                OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
                
            });
            return Ok(new
            {
                orders,
                count =  orders.Count()
            });

        }



        [Route("MemberOrder02")]
        [HttpGet]
        public IHttpActionResult MemberGet11(int id)
        {
            List<Orders> ordes = db.Orders.Where(x => x.Elders.MemberId == id).Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.已付款).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無待處理訂單"
                });
            }
            List<Orders> order11 = ordes.Where(x => x.Status == OrderType.待付款).ToList();
            foreach (Orders item in order11)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = OrderType.未於訂單開始前付款;
                }
            }
            List<Orders> order12 = ordes.Where(x => x.Status == OrderType.已付款).ToList();
            foreach (Orders item in order12)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = OrderType.服務進行中;
                }
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.Elders.MemberId == id).Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.已付款).ToList();

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
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
                    OrderStatus =x.Status.ToString()
                });
                return Ok(new
                {
                    orders,
                   count= orders.Count()
                });
            }
        }
        [Route("AttendantsOrder02")]
        [HttpGet]
        public IHttpActionResult AttendantsGet11(int id)
        {
            List<Orders> ordes = db.Orders.Where(x => x.AttendantId == id).Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.已付款).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無待處理訂單"
                });
            }
            List<Orders> order11 = ordes.Where(x => x.Status == OrderType.待付款).ToList();
            foreach (Orders item in order11)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = OrderType.未於訂單開始前付款;
                }
            }
            List<Orders> order12 = ordes.Where(x => x.Status == OrderType.已付款).ToList();
            foreach (Orders item in order12)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = OrderType.服務進行中;
                }
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.AttendantId == id).Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.已付款).ToList();

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
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
                    OrderStatus = x.Status.ToString()
                });
                return Ok(new
                {
                    orders,
                    count = orders.Count()
                });
            }
        }



        [Route("MemberOrder03")]
        [HttpGet]
        public IHttpActionResult MemberGet22(int id)
        {
            List<Orders> ordes = db.Orders.Where(x => x.Elders.MemberId == id).Where(x => x.Status == OrderType.待付款 || x.Status == OrderType.服務進行中).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無進行中訂單"
                });
            }
            List<Orders> to22 = ordes.Where(x => x.Status == OrderType.已付款).ToList();
            foreach (Orders item in to22)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = OrderType.服務進行中;
                }
            }

            List<Orders> to13 = ordes.Where(x => x.Status == OrderType.服務進行中).ToList();
            foreach (Orders item in to13)
            {
                if (DateTime.Compare(DateTime.Now, item.EndDate) > 0)
                {
                    item.Status = OrderType.已完成;
                }
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.Elders.MemberId == id && x.Status == OrderType.服務進行中).ToList();
            if (order.Count == 0)
            {
                return Ok(new { message = "尚無進行中訂單" });
            }
            else
            {
                var orders = order.Select(x => new
                {
                    x,
                    startDate = x.StartDate.ToString("yyyy-MM-dd"),
                    endDate = x.EndDate.ToString("yyyy-MM-dd"),
                    OrderInitDate = x.InitDate?.ToString("yyyy-MM-dd"),
                    OrderStatus = Enum.Parse(typeof(OrderType), x.Status.ToString()).ToString(),
                    
                });
                return Ok(new
                {
                    orders,
                    count =  orders.Count()
                });
            }
        }
        [Route("AttendantsOrder03")]
        [HttpGet]
        public IHttpActionResult AttendantsGet22(int id)
        {
            List<Orders> ordes = db.Orders.Where(x => x.AttendantId == id).Where(x => x.Status == OrderType.已付款 || x.Status == OrderType.服務進行中).ToList();
            if (ordes.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無進行中訂單"
                });
            }
            List<Orders> to22 = ordes.Where(x => x.Status == OrderType.已付款).ToList();
            foreach (Orders item in to22)
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) > 0)
                {
                    item.Status = OrderType.服務進行中;
                }
            }

            List<Orders> to13 = ordes.Where(x => x.Status == OrderType.服務進行中).ToList();
            foreach (Orders item in to13)
            {
                if (DateTime.Compare(DateTime.Now, item.EndDate) > 0)
                {
                    item.Status = OrderType.已完成;
                }
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.AttendantId == id && x.Status == OrderType.服務進行中)
                .ToList();
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
                    OrderStatus = Enum.Parse(typeof(OrderType), x.Status.ToString()).ToString(),
             
                });
                return Ok(new
                {
                    orders,
                    count = orders.Count()
                });
            }
        }


        [Route("MemberOrder04")]
        [HttpGet]
        public IHttpActionResult MemberGet13(int id)
        {
            List<Orders> orders = db.Orders.Where(x => x.Elders.MemberId == id && x.Star == null)
                .Where(x => x.Status == OrderType.已完成 || x.Status == OrderType.待評價).ToList();
            if (orders.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無需要填寫評價的訂單"
                });
            }
            var order = orders.Select(x => new
            {
                x,
                initTime = x.InitDate.Value.ToString("yyyy-MM-dd"),
                startTime = x.StartDate.ToString("yyyy-MM-dd"),
                endTime = x.EndDate.ToString("yyyy-MM-dd"),
                status ="待評價"
            });
            return Ok(new
            {
                order,
                count = order.Count()
            });
        }
        [Route("AttendantsOrder04")]
        [HttpGet]
        public IHttpActionResult AttendantsGet13(int id)
        {
            List<Orders> orders = db.Orders.Where(x => x.AttendantId == id && x.Status == OrderType.待評價).ToList();
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
                initTime = x.InitDate.Value.ToString("yyyy-MM-dd"),
                startTime = x.StartDate.ToString("yyyy-MM-dd"),
                endTime = x.EndDate.ToString("yyyy-MM-dd"),
                status = "待匯款",
            });
            return Ok(new
            {
                order,
                count = order.Count()
            });
        }


        [Route("MemberOrder05")]
        [HttpGet]
        public IHttpActionResult MemberFinish(int id)
        {
            List<Orders> orders = db.Orders.Where(x => x.Elders.MemberId == id)
                .Where(x => x.Status == OrderType.已取消 || x.Status == OrderType.已完成 || x.Status == OrderType.中斷 || x.Status == OrderType.待退款 || x.Status == OrderType.照服員拒接)
                .ToList();
            if (orders.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無已完成訂單"
                });
            }

            var order = orders.Select(x => new
            {
                x,
                initTime = x.InitDate.Value.ToString("yyyy-MM-dd"),
                startTime = x.StartDate.ToString("yyyy-MM-dd"),
                endTime = x.EndDate.ToString("yyyy-MM-dd"),
                status = Enum.Parse(typeof(OrderType), x.Status.ToString()).ToString(),
                serviceTime = Utility.Servicetime(Enum.Parse(typeof(ServiceTime), x.Attendants.ServiceTime.ToString()).ToString()),
       
            });

            return Ok(new
            {
                order,
                count = order.Count()
            });
        }
        [Route("AttendantsOrder05")]
        [HttpGet]
        public IHttpActionResult AttendantsFinish(int id)
        {
            List<Orders> orders = db.Orders.Where(x => x.AttendantId == id)
                .Where(x => x.Status == OrderType.已取消 || x.Status == OrderType.已完成 || x.Status == OrderType.中斷 || x.Status == OrderType.待退款 || x.Status == OrderType.照服員拒接)
                .ToList();
            if (orders.Count == 0)
            {
                return Ok(new
                {
                    message = "尚無已完成訂單"
                });
            }

            var order = orders.Select(x => new
            {
                x,
                initTime = x.InitDate.Value.ToString("yyyy-MM-dd"),
                startTime = x.StartDate.ToString("yyyy-MM-dd"),
                endTime = x.EndDate.ToString("yyyy-MM-dd"),
                status = Enum.Parse(typeof(OrderType), x.Status.ToString()).ToString(),
                serviceTime = Utility.Servicetime(Enum.Parse(typeof(ServiceTime), x.Attendants.ServiceTime.ToString()).ToString()),
       

            });

            return Ok(new
            {
                order,
                count = order.Count()
            });
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
                initTime = order.InitDate.Value.ToString("yyyy-MM-dd"),
                AttendantsService = Utility.Service(order.Attendants.Service),
                AttendantsServiceTime = Utility.Servicetime(Enum.Parse(typeof(ServiceTime), order.Attendants.ServiceTime.ToString()).ToString()),
                date,
                EldersBody = Utility.EldersBody(order.Elders.Body),
                EldersEquipment = Utility.EldersEquipment(order.Elders.Equipment),
                EldersServiceItems = Utility.Service(order.Attendants.Service),
                OrderStatus = order.Status.ToString()
            });
        }

        [Route("OrderAccept")]
        [HttpPatch]
        public IHttpActionResult OrderAccept(int id)
        {
            Orders order = db.Orders.Find(id);
            order.Status = OrderType.待付款;
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
                order.Status = OrderType.照服員拒接;
                order.EditDate = DateTime.Now;
                db.SaveChanges();
                return Ok(new { message = "已拒絕此訂單" });
            }
            else
            {
                return Ok(new { message = "未填寫拒絕理由" });
            }
        }

        [Route("WriteLog")]
        [HttpPost]
        public IHttpActionResult WriteLog(CareRecords careRecords)
        {
            ModelState.Remove("Remark");
            if (!ModelState.IsValid)
            {
                return Ok(new
                {
                    message = "照護日製沒填喔"
                });
            }
            List<CareRecords> Records = db.CareRecords.Where(x => x.OrdersID == careRecords.OrdersID).ToList();
            if (Records.Count != 0)
            {
                foreach (var item in Records)
                {
                    if (item.InitDate.Value.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        return Ok(new
                        {
                            message = item.InitDate.Value.ToString("yyyy-MM-dd") + "的照護紀錄已經填過囉"
                        });
                    }
                }
            }
            careRecords.InitDate = DateTime.Now;
            db.CareRecords.Add(careRecords);

            string date = careRecords.InitDate.Value.ToString("yyyy-MM-dd");
            db.SaveChanges();
            return Ok(new
            {
                message = $"已新增{date}的照護紀錄"
            });
        }

        [Route("GetLog")]
        [HttpGet]
        public IHttpActionResult GetLog(int id)
        {
            List<CareRecords> careRecords = db.CareRecords.Where(x => x.OrdersID == id).ToList();

            if (careRecords.Count == 0)
            {
                return Ok(new
                {
                    messsage = "此訂單目前尚無照護紀錄"
                });
            }
            var records = careRecords.Select(x => new
            {
                date = x.InitDate.Value.ToString("yyyy-MM-dd"),
                mood = x.Mood,
                time = x.InitDate.Value.ToString("HH:mm"),
                remark = x.Remark
            });

            return Ok(records);
        }


        [HttpPatch]
        [Route("FillinComment")]
        public HttpResponseMessage FillinComment(FillinCommentView FillinComment)
        {
            ModelState.Remove("EditDate");
            if (FillinComment.Comment == null)
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