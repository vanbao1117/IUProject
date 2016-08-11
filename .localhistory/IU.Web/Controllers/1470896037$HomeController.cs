using IU.Services;
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
            

            using (UserService service = new UserService())
            {
                string userName = HttpContext.User.Identity.Name;
                var user = service.GetUserInfo(userName);
                if (user != null)
                    return View(user);
                else
                    return View();
            }
            
        }



        public PartialViewResult StudentInClass(string classID)
        {
            return PartialView("~/Views/Admin/_Account.cshtml");
        }

        public PartialViewResult AdminAccount()
        {
            return PartialView("~/Views/Admin/_Account.cshtml");
        }

        public PartialViewResult AdminHome()
        {
            return PartialView("~/Views/Admin/_Home.cshtml");
        }

        public PartialViewResult AdminSchedule()
        {
            return PartialView("~/Views/Admin/_Schedule.cshtml");
        }
        public PartialViewResult AdminAttenance()
        {
            return PartialView("~/Views/Admin/_Attendance.cshtml");
        }

        public PartialViewResult Lecturer()
        {
            return PartialView("~/Views/Lecturer/_LecturerHome.cshtml");
        }

        public PartialViewResult TakeAttendance()
        {
            return PartialView("~/Views/Lecturer/_TakeAttendance.cshtml");
        }

        public PartialViewResult Dashboard()
        {
            return PartialView("~/Views/Dashboard/_Dashboard.cshtml");
        }

        public PartialViewResult Bis()
        {
            return PartialView("~/Views/Bis/_Bis.cshtml");
        }

        public PartialViewResult StuAttendance()
        {
            return PartialView("~/Views/StuAttendance/_StuAttendance.cshtml");
        }

        public PartialViewResult Schedule()
        {
            return PartialView("~/Views/Schedule/_Schedule.cshtml");
        }


        public PartialViewResult FeedBack()
        {
            return PartialView("~/Views/FeedBack/_FeedBack.cshtml");
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