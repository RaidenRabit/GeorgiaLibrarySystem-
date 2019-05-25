using System.Linq;
using System.Data.Entity;
using Core;

namespace GTLService.DataAccess.Code
{
    public class MemberDa_Code
    {
        public virtual Member GetMember(int ssn, Context context)
        {
            return context.Members
                .Include(x => x.MemberType)
                .SingleOrDefault(x => x.SSN == ssn);
        }
    }
}