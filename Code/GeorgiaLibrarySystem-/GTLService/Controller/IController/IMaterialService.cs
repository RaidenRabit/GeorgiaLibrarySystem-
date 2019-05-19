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

        [OperationContract]
        bool CreateMaterial(int ssn, int isbn, string library, string author, string description, string title, string typeName, int quantity);
        
        [OperationContract]
        bool DeleteMaterial(int ssn, int isbn);
        
        [OperationContract]
        bool DeleteCopy(int ssn, int copyId);
	}
}
