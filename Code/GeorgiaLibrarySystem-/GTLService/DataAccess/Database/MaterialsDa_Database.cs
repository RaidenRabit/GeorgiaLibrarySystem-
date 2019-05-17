using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using Core;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Database
{
    public class MaterialsDa_Database : IMaterialsDa
    {
        private readonly Context _context;
        public MaterialsDa_Database(Context context)
        {
            this._context = context;
        }

        public virtual List<readAllMaterial> ReadMaterials(string materialTitle, string author,
            int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            var a = _context.readAllMaterials
              /*  .Where(x => (isbn == 0 || x.ISBN.Equals(isbn.ToString())) &&
                            x.Author.Contains(author) &&
                            x.Title.Contains(materialTitle) &&
                             (jobStatus.Contains("0") || x.TypeName.Contains(jobStatus))
                )*/
              .Take(1000)  
              .ToList();
            return a;
        }
    }
}