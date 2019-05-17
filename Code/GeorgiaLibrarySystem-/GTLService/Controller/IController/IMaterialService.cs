using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GTLService.Controller
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMaterialService" in both code and config file together.
	[ServiceContract]
	public interface IMaterialService
	{
		[OperationContract]
		void DoWork();

        [OperationContract]
        bool GetMaterials(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, int jobStatus = 0);
	}
}
