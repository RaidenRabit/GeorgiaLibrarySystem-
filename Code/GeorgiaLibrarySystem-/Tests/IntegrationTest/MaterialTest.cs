using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NUnit.Framework;

namespace Tests.IntegrationTest
{

    public class MaterialTest
    {
        public void GetMaterialsCode(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {

        }

        public void CreateMaterialCode(int ssn, int isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {

        }

        public void DeleteMaterialCode(int ssn, int isbn)
        {

        }


        public void DeleteCopyCode(int ssn, int copyId)
        {

        }
        
        public void GetMaterialsDb(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {

        }

        public void CreateMaterialDb(int ssn, int isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {

        }

        public void DeleteMaterialDb(int ssn, int isbn)
        {

        }

        [Test]
        public void DeleteCopyDb(int ssn, int copyId)
        {
            Assert.IsTrue(true);
        }

        [SetUp]
        public void Setup()
        {
            string connectionString =
                "metadata=res://*/GTLModel.csdl|res://*/GTLModel.ssdl|res://*/GTLModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=localhost;initial catalog=GTL;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
            
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"..\..\" + "SQLCreateQuery.sql")));
            string path = directory.ToString();

            string script = File.ReadAllText(@"C:\Users\abirt\source\repos\RaidenRabit\GeorgiaLibrarySystem-\Code\Database\SQLCreateQuery.sql");
            SqlConnection conn = new SqlConnection(connectionString);
            Server server = new Server(new ServerConnection(conn));
            server.ConnectionContext.ExecuteNonQuery(script);
            conn.Close();
        }
    }
}
