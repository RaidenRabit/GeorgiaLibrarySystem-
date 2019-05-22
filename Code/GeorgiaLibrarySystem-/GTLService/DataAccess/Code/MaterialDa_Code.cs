using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class MaterialDa_Code
    {
        private readonly Context _context;
        public MaterialDa_Code(Context context)
        {
            _context = context;
        }

        public virtual List<Material> ReadMaterials(string isbn, string title, string author, int numOfRecords)
        {
            return _context.Materials
                .Where(x => (isbn.Equals("0") || x.ISBN.Equals(isbn)) &&
                            x.Author.Contains(author) &&
                            x.Title.Contains(title)
                )
                .Take(numOfRecords)
                .ToList();
        }

        public virtual bool CheckMaterialIsbn(string isbn)
        {
            return _context.Materials.Find(isbn) != null;
        }

        public virtual bool CreateMaterial(string isbn, string author, string description, string title)
        {
            try
            {
                Material material = new Material
                    {ISBN = isbn, Author = author, Title = title, Description = description};
                _context.Materials.Add(material);
                return _context.SaveChanges() > 0;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public virtual bool DeleteMaterial(string isbn)
        {
            try
            {
                var material = _context.Materials.Single(o => o.ISBN.Equals(isbn));
                _context.Materials.Remove(material);
                return _context.SaveChanges() > 0;
            }
            catch (SqlException)
            {
                return false;
            }
        }

    }
}