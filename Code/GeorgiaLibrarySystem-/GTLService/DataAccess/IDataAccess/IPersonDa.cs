using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTLService.DataAccess.IDataAccess
{
    public interface IPersonDa
    {
        bool CheckLibrarianSsn(int ssn);

        bool CheckMemberSsn(int ssn);
    }
}
