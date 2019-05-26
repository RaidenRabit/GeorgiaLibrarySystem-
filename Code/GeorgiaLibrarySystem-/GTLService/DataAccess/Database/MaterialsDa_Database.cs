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
            _context = context;
        }

        public virtual List<readAllMaterial> ReadMaterials(string materialTitle, string author,
            int numOfRecords = 10, string isbn = "0", string jobStatus = "0")
        {

            return _context.readAllMaterials.AsNoTracking()
                .Where(x => (isbn.Equals("0") || x.ISBN.Equals(isbn.ToString())) &&
                            x.Author.Contains(author) &&
                            x.Title.Contains(materialTitle) &&
                             (jobStatus.Contains("0") || x.TypeName.Equals(jobStatus))
                )
              .Take(numOfRecords)  
                .OrderByDescending(x => x.Available_Copies)
              .ToList();
        }

        public bool CreateMaterial(int ssn, string isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {
            var value = _context.CreateMaterials(ssn, Int32.Parse(isbn), library,author,description,title,typeName,quantity).First();
            return Convert.ToBoolean(value);
        }

        public bool DeleteMaterial(int ssn, string isbn)
        {
            var value = _context.DeleteMaterial(ssn, Int32.Parse(isbn)).First();
            return Convert.ToBoolean(value);
        }
    }
}