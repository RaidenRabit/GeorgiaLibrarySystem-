using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core;
using GtlWebsite.MaterialServiceReference;

namespace GtlWebsite.Controllers
{
    public class MaterialsController : Controller
    {
        private MaterialServiceClient _client;

        public MaterialsController()
        {
            _client = new MaterialServiceClient();
        }
        // GET: Materials
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetMaterialsAjax(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            var response = _client.GetMaterials(materialTitle,author,numOfRecords,isbn,jobStatus);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(response);
            return json;
        }

        public void Borrow()
        {

        }
    }
}