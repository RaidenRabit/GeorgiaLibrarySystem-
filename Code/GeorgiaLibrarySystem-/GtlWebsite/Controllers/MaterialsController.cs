using System.Threading.Tasks;
using System.Web.Mvc;
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
        public async Task<string> GetMaterialsAjax(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, int jobStatus = 0)
        {
            var response = await _client.GetMaterials(materialTitle,author,numOfRecords,isbn,jobStatus);
            return await response.Content.ReadAsStringAsync();
        }

        public void Borrow()
        {

        }
    }
}