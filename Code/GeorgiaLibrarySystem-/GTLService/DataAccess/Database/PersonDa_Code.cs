﻿using Core;

namespace GTLService.DataAccess.Database
{
    public class PersonDa_Code
    {
        private readonly Context _context;
        public PersonDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool CheckLibrarianSsn(int ssn)
        {
            var person = _context.Librarians.Find(ssn);
            return person != null;
        }

        public virtual bool CheckMemberSsn(int ssn)
        {
            var person = _context.Members.Find(ssn);
            return person != null;
        }
    }
}