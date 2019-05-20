using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GtlWebsite.CopyServiceReference;
using GtlWebsite.LendingServiceReference;
using GtlWebsite.MaterialServiceReference;

namespace GtlWebsite.Controllers
{
    public class MaterialsController : Controller
    {
        private MaterialServiceClient _materialClient;
        private CopyServiceClient _copyClient;
        private LendingServiceClient _lendingClient;

        public MaterialsController()
        {
            _materialClient = new MaterialServiceClient();
            _copyClient = new CopyServiceClient();
            _lendingClient = new LendingServiceClient();
        }
        // GET: Materials
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetMaterialsAjax(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            var response = _materialClient.GetMaterials(materialTitle,author,numOfRecords,isbn,jobStatus);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(response);
            return json;
        }

        [HttpGet]
        public ActionResult Borrow(string isbn, string availableCopies)
        {
            int id, copies = Int32.Parse(availableCopies);
            while (copies >= 0)
            {
                id = _copyClient.GetAvailableCopyId(Int32.Parse(isbn));
                if (id != 0)
                {
                    if(_lendingClient.LendBook(Int32.Parse(Session["SSN"].ToString()), id))
                        return PartialView("_succesfullBorrow", id);
                }

                copies--;
            }

            return PartialView("_failedBorrow");
        }
    }
}