using System.Collections.Generic;
using System.ServiceModel;
using Core;

namespace GTLService.Controller.IController
{
	
	[ServiceContract]
	public interface IMaterialService
	{
        
        [OperationContract]
        List<readAllMaterial> GetMaterials(string materialTitle, string author, int numOfRecords = 10, string isbn = "0", string jobStatus = "0");

        [OperationContract]
        bool CreateMaterial(int ssn, string isbn, string library, string author, string description, string title, string typeName, int quantity);
        
        [OperationContract]
        bool DeleteMaterial(int ssn, string isbn);
	}
}
