using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace GTLService.DataAccess.IDataAccess
{
    public interface IMaterialsDa
    {
        List<readAllMaterial> ReadMaterials(string materialTitle, string author,
            int numOfRecords = 10, int isbn = 0, string jobStatus = "0");
    }
}
