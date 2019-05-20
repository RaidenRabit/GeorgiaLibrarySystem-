﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class CopyDa_Code
    {
        private readonly Context _context;
        public CopyDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool CheckCopyId(int id)
        {
            return _context.Copies.Find(id) != null;
        }

        public virtual bool CheckTypeName(string typeName)
        {
            return _context.Copies.FirstOrDefault(x => x.TypeName.Equals(typeName)) != null;
        }
        
        public virtual bool CreateCopy(int isbn, string libraryName, string typeName)
        {
            try
            {
                Copy copy = new Copy {ISBN = isbn.ToString(), LibraryName = libraryName, TypeName = typeName};
                _context.Copies.Add(copy);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        
        public virtual List<Copy> ReadCopies(int isbn, string typeName)
        {
            var a = _context.Copies
                .Where(x => (isbn == 0 || x.ISBN.Equals(isbn.ToString())) &&
                            (typeName.Contains("0") || x.TypeName.Equals(typeName))
                )
                .ToList();
            return a;
        }
        
        public virtual bool DeleteCopy(int copyId)
        {
            try
            {
                var copy = _context.Copies.Single(o => o.CopyID == copyId);
                _context.Copies.Remove(copy);
                _context.SaveChanges();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public virtual List<Copy> GetAvailableCopyId(int isbn)
        {
            try
            {
                return _context.Copies.Where(x => x.ISBN.Equals(isbn.ToString())).ToList();
            }
            catch (SqlException)
            {
                return null;
            }
        }
    }
}