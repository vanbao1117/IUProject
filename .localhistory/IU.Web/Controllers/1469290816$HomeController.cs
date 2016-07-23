using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IU.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Dashboard()
        {
            return PartialView("~/Views/Dashboard/_Dashboard.cshtml");
        }

        public PartialViewResult StuAttendance()
        {
            return PartialView("~/Views/StuAttendance/_StuAttendance.cshtml");
        }

        public PartialViewResult Schedule()
        {
            return PartialView("~/Views/Class/_Schedule.cshtml");
        }

        public PartialViewResult Input()
        {
            return PartialView("~/Views/Input/_Input.cshtml");
        }

        public PartialViewResult Category()
        {
            return PartialView("~/Views/Category/_Category.cshtml");
        }

        public PartialViewResult Exploit()
        {
            return PartialView("~/Views/Exploit/_Exploit.cshtml");
        }

        public PartialViewResult Search()
        {
            return PartialView("~/Views/Search/_Search.cshtml");
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
    }
}