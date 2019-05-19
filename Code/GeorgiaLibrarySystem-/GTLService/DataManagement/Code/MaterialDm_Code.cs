using System.Collections.Generic;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class MaterialDm_Code : IMaterialsDm
    {
        private readonly LibraryDa_Code _libraryDa;
        private readonly MaterialDa_Code _materialDa;
        private readonly PersonDa_Code _personDa;

        public MaterialDm_Code(MaterialDa_Code materialDa, LibraryDa_Code libraryDa, PersonDa_Code personDa)
        {
            this._materialDa = materialDa;
            this._libraryDa = libraryDa;
            this._personDa = personDa;
        }

        public List<readAllMaterial> ReadMaterials(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            var materials = _materialDa.ReadMaterials(isbn, materialTitle, author, numOfRecords);
            var copies = _materialDa.ReadCopies(isbn, jobStatus);

            List<readAllMaterial> allMaterials = new List<readAllMaterial>();
            readAllMaterial readAllMaterial;

            foreach (var c in copies)
            {
                bool unique = true;
                foreach (var allMaterial in allMaterials)
                {
                    if (allMaterial.ISBN == c.ISBN && allMaterial.TypeName == c.TypeName &&
                        allMaterial.Location == c.LibraryName)
                        unique = false;
                }

                if (unique)
                {
                    foreach (var m in materials)
                    {
                        if (m.ISBN.Equals(c.ISBN))
                        {
                            readAllMaterial = new readAllMaterial
                            {
                                ISBN = c.ISBN, TypeName = c.TypeName, Location = c.LibraryName,
                                Description = m.Description,
                                Author = m.Author, Title = m.Author
                            };
                            allMaterials.Add(readAllMaterial);
                        }
                    }
                   
                }

            }
            if(allMaterials.Count > numOfRecords)
                for (int i = allMaterials.Count - 1; i >= numOfRecords; i--)
                {
                    allMaterials.RemoveAt(i);
                }

            allMaterials = CountAvailableCopies(allMaterials, copies);
            return allMaterials;
        }

        private List<readAllMaterial> CountAvailableCopies(List<readAllMaterial> allMaterials, List<Copy> copies)
        {
            foreach (var readAllMaterial in allMaterials)
            {
                int count = 0;
                foreach (var copy in copies)
                {
                    if (readAllMaterial.ISBN.Equals(copy.ISBN) && readAllMaterial.Location.Equals(copy.LibraryName)
                                                               && readAllMaterial.TypeName.Equals(copy.TypeName))
                        count++;
                }

                readAllMaterial.Available_Copies = count;
            }

            return allMaterials;
        }

        public bool CreateMaterial(int ssn, int isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {
            if(_personDa.CheckLibrarianSsn(ssn))
            {
                if (_materialDa.CheckMaterialIsbn(isbn))
                {
                    var material = _materialDa.ReadMaterials(isbn);
                    for (int i = 0; i < quantity; i++)
                    {
                        _materialDa.CreateCopy(isbn, material.Location, material.TypeName);
                    }

                    return true;
                }
                else
                {
                    if (_libraryDa.CheckLibraryName(library) && _materialDa.CheckTypeName(typeName))
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            _materialDa.CreateCopy(isbn, library, typeName);
                        }

                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        public bool DeleteMaterial(int ssn, int isbn)
        {
            if (_personDa.CheckLibrarianSsn(ssn) && _materialDa.CheckMaterialIsbn(isbn))
            {
                _materialDa.DeleteMaterial(isbn);
                return true;
            }

            return false;
        }

        public bool DeleteCopy(int ssn, int copyId)
        {
            if (_personDa.CheckLibrarianSsn(ssn) && _materialDa.CheckCopyId(copyId))
            {
                _materialDa.DeleteCopy(copyId);
                return true;
            }

            return false;
        }
    }
}