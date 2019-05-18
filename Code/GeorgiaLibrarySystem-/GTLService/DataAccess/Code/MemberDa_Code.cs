﻿using System.Linq;
using System.Data.Entity;
using Core;

namespace GTLService.DataAccess.Code
{
    public class MemberDa_Code
    {
        private readonly Context _context;
        public MemberDa_Code(Context context)
        {
            _context = context;
        }

        public virtual Member GetMember(int ssn)
        {
            return _context.Members
                .Include(x => x.MemberType)
                .SingleOrDefault(x => x.SSN == ssn);
        }
    }
}