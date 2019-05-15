using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtlService.DataAccess.IDataAccess
{
    public interface ILoginDa
    {
        bool Login(int ssn, string password);
    }
}
