using System.Collections.Generic;
using Core;

namespace GTLService.DataAccess.IDataAccess
{
    public interface IMaterialsDa
    {
        List<readAllMaterial> ReadMaterials(string materialTitle, string author,
            int numOfRecords = 10, int isbn = 0, string jobStatus = "0");

        bool CreateMaterial(int ssn, int isbn, string library, string author, string description, string title,
            string typeName, int quantity);

        bool DeleteMaterial(int ssn, int isbn);

        bool DeleteCopy(int ssn, int copyId);
    }
}
