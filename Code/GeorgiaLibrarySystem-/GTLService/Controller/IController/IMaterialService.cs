using System.Collections.Generic;
using System.ServiceModel;
using Core;

namespace GTLService.Controller
{
	
	[ServiceContract]
	public interface IMaterialService
	{

        [OperationContract]
        List<readAllMaterial> GetMaterials(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0");
	}
}
