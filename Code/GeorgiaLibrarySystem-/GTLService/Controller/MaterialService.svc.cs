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

        public List<readAllMaterial> GetMaterials(string materialTitle, string author, int numOfRecords = 10, string isbn = "0", string jobStatus = "0")
        {
            return _materialsDm.ReadMaterials(materialTitle, author, numOfRecords, isbn, jobStatus);
        }

        public bool CreateMaterial(int ssn, string isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {
            return _materialsDm.CreateMaterial(ssn, isbn, library, author, description, title, typeName, quantity);
        }

        public bool DeleteMaterial(int ssn, string isbn)
        {
            return _materialsDm.DeleteMaterial(ssn, isbn);
        }

        public bool DeleteCopy(int ssn, int copyId)
        {
            return _materialsDm.DeleteCopy(ssn, copyId);
        }
    }
}
