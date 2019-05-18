using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Core;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataAccess.Code
{
    public class MaterialDa_Code
    {
        private readonly Context _context;
        public MaterialDa_Code(Context context)
        {
            this._context = context;
        }

        public List<Material> ReadMaterials(int isbn, string title, string author, int numOfRecords)
        {
            return _context.Materials
                .Where(x => (isbn == 0 || x.ISBN.Equals(isbn.ToString())) &&
                            x.Author.Contains(author) &&
                            x.Title.Contains(title)
                )
                .Take(numOfRecords)
                .ToList();
        }

        public List<Copy> ReadCopies(int isbn, string typeName)
        {
            var a = _context.Copies
                .Where(x => (isbn == 0 || x.ISBN.Equals(isbn.ToString())) &&
                    (typeName.Contains("0") || x.TypeName.Contains(typeName))
                )
                .ToList();
            return a;
        }

        public readAllMaterial ReadMaterials(int isbn)
        {
            return _context.readAllMaterials.Find(isbn);
        }

        public bool CheckMaterialIsbn(int isbn)
        {
            var material = _context.Materials.Find(isbn);
            return material != null;
        }

        public bool CheckCopyId(int id)
        {
            var copy = _context.Copies.Find(id);
            return copy != null;
        }

        public bool CheckTypeName(string typeName)
        {
            var copy = _context.Copies.FirstOrDefault(x => x.TypeName.Equals(typeName));
            return copy != null;
        }

        public bool CreateMaterial(int isbn, string author, string description, string title)
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

        public bool CreateCopy(int isbn, string libraryName, string typeName)
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

        public bool DeleteMaterial(int isbn)
        {
            try
            {
                _context.Materials.Remove(new Material {ISBN = isbn.ToString()});
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool DeleteCopy(int copyId)
        {
            try
            {
                _context.Copies.Remove(new Copy {CopyID = copyId});
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}