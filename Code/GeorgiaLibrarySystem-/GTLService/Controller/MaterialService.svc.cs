using System.Collections.Generic;
using Core;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.Controller
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialsDm _materialsDm;
        public MaterialService(IMaterialsDm materialsDm)
        {
            _materialsDm = materialsDm;
        }

        public List<readAllMaterial> GetMaterials(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            var a = _materialsDm.ReadMaterials(materialTitle, author, numOfRecords, isbn, jobStatus);
            return a;
        }
    }
}
