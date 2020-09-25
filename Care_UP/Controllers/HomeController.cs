using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Care_UP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //return RedirectPermanent("http://careup.rocket-coding.com/index.html#/");//要測本地要註解掉

            return RedirectPermanent("http://careup.rocket-coding.com/index.html#/memberAdmin/order");
        }
    }
}
