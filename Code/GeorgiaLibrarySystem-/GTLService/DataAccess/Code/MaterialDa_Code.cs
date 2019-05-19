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
            this._context = context;
        }

        public virtual List<Material> ReadMaterials(int isbn, string title, string author, int numOfRecords)
        {
            return _context.Materials
                .Where(x => (isbn == 0 || x.ISBN.Equals(isbn.ToString())) &&
                            x.Author.Contains(author) &&
                            x.Title.Contains(title)
                )
                .Take(numOfRecords)
                .ToList();
        }

        public virtual Material ReadMaterials(int isbn)
        {
            return _context.Materials.Find(isbn.ToString());
        }

        public virtual bool CheckMaterialIsbn(int isbn)
        {
            var material = _context.Materials.Find(isbn.ToString());
            return material != null;
        }

        public virtual bool CreateMaterial(int isbn, string author, string description, string title)
        {
            try
            {
                Material material = new Material
                    {ISBN = isbn.ToString(), Author = author, Title = title, Description = description};
                _context.Materials.Add(material);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public virtual bool DeleteMaterial(int isbn)
        {
            try
            {
                var material = _context.Materials.Single(o => o.ISBN.Equals(isbn.ToString()));
                _context.Materials.Remove(material);
                _context.SaveChanges();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

    }
}