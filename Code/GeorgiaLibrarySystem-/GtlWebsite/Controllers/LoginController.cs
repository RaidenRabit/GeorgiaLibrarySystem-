using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GtlWebsite.LoginReference;

namespace GtlWebsite.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int ssn, string password)
        {
            //new InstanceContext(this)
            LoginControllerClient client = new LoginControllerClient();
            if(client.Login(ssn, password))
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}