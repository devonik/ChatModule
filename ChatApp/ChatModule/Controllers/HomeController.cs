using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatModule.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowCrossSiteJson]
        public ActionResult GetChat(bool partial, int currentUserId)
        {
            var request = Request;
            ViewBag.Partial = partial;
            ViewBag.CurrentUserId = currentUserId;
            if (partial == true)
            {
                return PartialView("~/Views/Chatlogs/Index.cshtml");
            }
            else
            {
                return View("~/Views/Chatlogs/Index.cshtml");
            }
        }
    }
}