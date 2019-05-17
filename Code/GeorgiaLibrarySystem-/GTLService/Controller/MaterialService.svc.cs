using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GTLService.Controller
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MaterialService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MaterialService.svc or MaterialService.svc.cs at the Solution Explorer and start debugging.
    public class MaterialService : IMaterialService
    {
        public void DoWork()
        {
        }

        public List<Materials> GetMaterials(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, int jobStatus = 0)
        {
            return true;
        }
    }
}
