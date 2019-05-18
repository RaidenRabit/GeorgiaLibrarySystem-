using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Database
{
    public class MaterialsDa_Database 
    {
        private readonly Context _context;
        public MaterialsDa_Database(Context context)
        {
            this._context = context;
        }

        public virtual List<readAllMaterial> ReadMaterials(string materialTitle, string author,
            int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {

            return _context.readAllMaterials.AsNoTracking()
                .Where(x => (isbn == 0 || x.ISBN.Equals(isbn.ToString())) &&
                            x.Author.Contains(author) &&
                            x.Title.Contains(materialTitle) &&
                             (jobStatus.Contains("0") || x.TypeName.Contains(jobStatus))
                )
              .Take(numOfRecords)  
              .ToList();
        }

        public bool CreateMaterial(int ssn, int isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {
            var value = _context.CreateMaterials(ssn, isbn,library,author,description,title,typeName,quantity).First();
            return Convert.ToBoolean(value);
        }

        public bool DeleteMaterial(int ssn, int isbn)
        {
            var value = _context.DeleteMaterial(ssn, isbn).First();
            return Convert.ToBoolean(value);
        }

        public bool DeleteCopy(int ssn, int copyId)
        {
            var value = _context.DeleteCopy(ssn, copyId).First();
            return Convert.ToBoolean(value);
        }
    }
}