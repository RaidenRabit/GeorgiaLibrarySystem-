using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public virtual List<Copy> ReadCopies(int isbn, string typeName)
        {
            var a = _context.Copies
                .Where(x => (isbn == 0 || x.ISBN.Equals(isbn.ToString())) &&
                    (typeName.Contains("0") || x.TypeName.Equals(typeName))
                )
                .ToList();
            return a;
        }

        public virtual readAllMaterial ReadMaterials(int isbn)
        {
            return _context.readAllMaterials.Find(isbn);
        }

        public virtual bool CheckMaterialIsbn(int isbn)
        {
            var material = _context.Materials.Find(isbn);
            return material != null;
        }

        public virtual bool CheckCopyId(int id)
        {
            var copy = _context.Copies.Find(id);
            return copy != null;
        }

        public virtual bool CheckTypeName(string typeName)
        {
            var copy = _context.Copies.FirstOrDefault(x => x.TypeName.Equals(typeName));
            return copy != null;
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
    }
}