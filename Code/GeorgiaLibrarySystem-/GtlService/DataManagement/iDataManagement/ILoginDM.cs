using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GtlService.DataAccess;

namespace GtlService.DataManagement.iDataManagement
{
    public interface ILoginDm
    {
        bool Login(int ssn, string password);
    }
}
