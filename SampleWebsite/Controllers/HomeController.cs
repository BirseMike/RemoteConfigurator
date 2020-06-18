using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SampleWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //WebConfigurationManager.AppSettings["dfdsf"] = "balal";
            var configer = new WebConfigurationLibrary.WebConfigurator();
            var appsettings = configer.GetAppSettings();
            var value = appsettings["MyWebSetting"];

            ViewBag.Message = $"WebSetting = {value}";


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}