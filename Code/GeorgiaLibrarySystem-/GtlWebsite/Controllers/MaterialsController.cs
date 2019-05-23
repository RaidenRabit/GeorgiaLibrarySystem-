using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GtlWebsite.CopyServiceReference;
using GtlWebsite.LoaningServiceReference;
using GtlWebsite.MaterialServiceReference;

namespace GtlWebsite.Controllers
{
    public class MaterialsController : Controller
    {
        private MaterialServiceClient _materialClient;
        private CopyServiceClient _copyClient;
        private LoaningServiceClient _loaningClient;

        public MaterialsController()
        {
            _materialClient = new MaterialServiceClient();
            _copyClient = new CopyServiceClient();
            _loaningClient = new LoaningServiceClient();
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
                    if(_loaningClient.LoanBook(Int32.Parse(Session["SSN"].ToString()), id))
                        return PartialView("_succesfullLoan", id);
                }

                copies--;
            }

            return PartialView("_failedLoan");
        }
    }
}