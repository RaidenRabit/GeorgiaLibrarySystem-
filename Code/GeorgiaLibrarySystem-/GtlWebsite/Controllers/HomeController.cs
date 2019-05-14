using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GtlWebsite.ExampleReference;

namespace GtlWebsite.Controllers
{
    public class HomeController : Controller
    {
        public string ContactService(int a)
        {
            Service1Client b = new Service1Client();
            return b.GetData(a);
        }

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
    }
}