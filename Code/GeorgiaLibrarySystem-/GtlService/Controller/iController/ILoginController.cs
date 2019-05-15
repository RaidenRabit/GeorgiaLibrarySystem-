using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GtlService.Controller.iController
{
    [ServiceContract]
    public interface ILoginController
    {
        [OperationContract]
        bool Login(int ssn, string password, int id);
    }
}
