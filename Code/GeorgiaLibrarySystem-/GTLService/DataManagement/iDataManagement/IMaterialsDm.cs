using System.Collections.Generic;
using Core;

namespace GTLService.DataManagement.IDataManagement
{
    public interface IMaterialsDm
    {
        List<readAllMaterial> ReadMaterials(string materialTitle, string author,
            int numOfRecords = 10, string isbn = "0", string jobStatus = "0");

        bool CreateMaterial(int ssn, string isbn, string library, string author, string description, string title,
            string typeName, int quantity);

        bool DeleteMaterial(int ssn, string isbn);
    }
}
