﻿using System.Web.Mvc;
using Core;
using GtlWebsite.LoginServiceReference;

namespace GtlWebsite.Controllers
{
    public class LoginController : Controller
    {
        LoginServiceClient _client = new LoginServiceClient();
        // GET: Login
        public ActionResult Index()
        {
            Session["SSN"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Person person)
        {
            //new InstanceContext(this)
            _client = new LoginServiceClient();
            if (_client.Login(person.SSN, person.Password))
            {
                Session["SSN"] = person.SSN;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}