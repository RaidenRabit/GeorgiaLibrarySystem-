﻿using System.Collections.Generic;
using Core;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class MaterialsDm_Database : IMaterialsDm
    {
        
        private readonly MaterialsDa_Database _materialDa;

        public MaterialsDm_Database(MaterialsDa_Database materialDa)
        {
            _materialDa = materialDa;
        }

        public List<readAllMaterial> ReadMaterials(string materialTitle, string author, int numOfRecords = 10, string isbn = "0", string jobStatus = "0")
        {
            return _materialDa.ReadMaterials(materialTitle, author, numOfRecords, isbn, jobStatus);
        }

        public bool CreateMaterial(int ssn, string isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {
            return _materialDa.CreateMaterial(ssn, isbn, library, author, description, title, typeName, quantity);
        }

        public bool DeleteMaterial(int ssn, string isbn)
        {
            return _materialDa.DeleteMaterial(ssn, isbn);
        }
    }
}