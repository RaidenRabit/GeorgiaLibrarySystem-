using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtlService.DataManagement.iDataManagement
{
    public interface ILoginDM
    {
        bool Login(int ssn, string password);
    }
}
