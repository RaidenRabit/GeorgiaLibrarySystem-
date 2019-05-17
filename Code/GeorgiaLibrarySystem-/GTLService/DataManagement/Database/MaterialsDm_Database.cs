using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core;
using GTLService.DataAccess.IDataAccess;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class MaterialsDm_Database : IMaterialsDm
    {
        
        private readonly IMaterialsDa _materialDa;

        public MaterialsDm_Database(IMaterialsDa materialDa)
        {
            this._materialDa = materialDa;
        }

        public List<readAllMaterial> ReadMaterials(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            return _materialDa.ReadMaterials(materialTitle, author, numOfRecords, isbn, jobStatus);
        }
    }
}