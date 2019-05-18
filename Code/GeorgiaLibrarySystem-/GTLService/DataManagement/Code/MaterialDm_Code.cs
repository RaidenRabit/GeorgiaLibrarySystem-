using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.IDataAccess;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class MaterialDm_Code : IMaterialsDm
    {
        private readonly LibraryDa_Code _libraryDa;
        private readonly MaterialDa_Code _materialDa;
        private readonly IPersonDa _personDa;

        public MaterialDm_Code(MaterialDa_Code materialDa, LibraryDa_Code libraryDa, IPersonDa personDa)
        {
            this._materialDa = materialDa;
            this._libraryDa = libraryDa;
            this._personDa = personDa;
        }

        public List<readAllMaterial> ReadMaterials(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            throw new NotImplementedException();
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