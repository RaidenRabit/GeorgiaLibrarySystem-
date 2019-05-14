using System.Web.Mvc;
using GtlService.Model;
using GtlWebsite.LoginReference;

namespace GtlWebsite.Controllers
{
    public class LoginController : Controller
    {
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
            LoginControllerClient client = new LoginControllerClient();
            if(client.Login(person.SSN, person.Password))
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}