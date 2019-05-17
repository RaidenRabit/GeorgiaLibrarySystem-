using System.Linq;
using System.Data.Entity;
using Core;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Code
{
    public class MemberDa_Code: IMemberDa
    {
        private readonly Context _context;
        public MemberDa_Code(Context context)
        {
            _context = context;
        }

        public Member GetMember(int ssn)
        {
            return _context.Members
                .Include(x => x.MemberType)
                .SingleOrDefault(x => x.SSN == ssn);
        }
    }
}