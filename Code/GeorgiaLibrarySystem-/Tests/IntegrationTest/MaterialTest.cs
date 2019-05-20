using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Core;
using GTLService.Controller;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NUnit.Framework;

namespace Tests.IntegrationTest
{

    public class MaterialTest
    {

        private IMaterialService _materialService;
        
        [Test]
        //Code approach
        [TestCase("", "", 10, null, "0", "Code", 5)]
        [TestCase("horror book", "", 10, null, "0", "Code", 1)]
        [TestCase("horror book", "Pala", 10, null, "0", "Code", 1)]
        //[TestCase("", "", 6, null, "0", "Code", 6)]
        [TestCase("", "", 10, 1, "0", "Code", 3)]
        //[TestCase("", "", 10, null, "books", "Code", 7)]
        //Database approach
        /*[TestCase("", "", 10, null, "0", "Database", 5)]
        [TestCase("horror book", "", 10, null, "0", "Database", 1)]
        [TestCase("horror book", "Pala", 10, null, "0", "Database", 1)]
        [TestCase("", "", 6, null, "0", "Database", 6)]
        [TestCase("", "", 10, 1, "0", "Database", 3)]
        [TestCase("", "", 10, null, "books", "Database", 7)]*/
        public void GetMaterials(string materialTitle, string author, int numOfRecords, int isbn, string jobStatus,
            string approach, int resultCount)
        {
            //Arrange
            Setup(approach);
            
            //Act
            var result = _materialService.GetMaterials(materialTitle, author, numOfRecords, isbn, jobStatus);

            //Assert
            Assert.IsTrue(result.Count.Equals(resultCount));

        }

        [Test]
        //Code approach
        [TestCase(123456785, 1, "GTL", "books", "Code", true)]
        [TestCase(123456785, 0, "GTL", "books", "Code", true)]    
        [TestCase(123456785, 0, "Non-existent", "Non-existent", "Code", false)]
        [TestCase(0, 0, "GTL", "books", "Code", false)]   
        [TestCase(0, 1, "Non-existent", "books", "Code", false)]    
        [TestCase(0, 1, "GTL", "books", "Code", false)]  
        //Database approach
        [TestCase(123456785, 1, "GTL", "books", "Database", true)]
        [TestCase(123456785, 0, "GTL", "books", "Database", true)]    
        [TestCase(123456785, 0, "Non-existent", "Non-existent", "Database", false)]
        [TestCase(0, 0, "GTL", "books", "Database", false)]   
        [TestCase(0, 1, "Non-existent", "books", "Database", false)]    
        [TestCase(0, 1, "GTL", "books", "Database", false)]  
        public void CreateMaterial(int ssn, int isbn, string library, string typeName, string approach, bool passing)
        {
            //Arrange
            Setup(approach);
            string author = "TestAuthor", description = "TestDescription", title = "testTitle";
            int quantity = 2;

            //Act
            bool result =
                _materialService.CreateMaterial(ssn, isbn, library, author, description, title, typeName, quantity);

            //Assert
            Assert.IsTrue(result.Equals(passing));
        }
        
        [Test]
        //Code approach
        [TestCase(0, 0, "Code", false)] //invalid ssn and isbn
        [TestCase(123456785, 0, "Code", false)] //valid ssn, invalid isbn
        [TestCase(0, 1, "Code", false)] //invalid ssn, valid isbn
        [TestCase(123456785, 8, "Code", true)] //valid ssn and isbn
        //Database approach
        [TestCase(0, 0, "Database", false)] //invalid ssn and isbn
        [TestCase(123456785, 0, "Database", false)] //valid ssn, invalid isbn
        [TestCase(0, 1, "Database", false)] //invalid ssn, valid isbn
        [TestCase(123456785, 8, "Database", true)] //valid ssn and isbn
        public void DeleteMaterial(int ssn, int isbn, string approach, bool passing)
        {
            //Arrange
            Setup(approach);

            //Act
            bool result = _materialService.DeleteMaterial(ssn, isbn);

            //Assert
            Assert.IsTrue(result.Equals(passing));
        }

        [Test]
        //Code approach
        [TestCase(0,0, "Code", false)] //invalid ssn and id
        [TestCase(123456785,0, "Code", false)] //valid ssn, invalid id
        [TestCase(0,1, "Code", false)] //invalid ssn, valid id
        [TestCase(123456785,12, "Code", true)] //valid ssn and id
        //Db approach
        [TestCase(0,0, "Database", false)] //invalid ssn and id
        [TestCase(123456785,0, "Database", false)] //valid ssn, invalid id
        [TestCase(0,1, "Database", false)] //invalid ssn, valid id
        [TestCase(123456785,13, "Database", true)] //valid ssn and id
        public void DeleteCopy(int ssn, int copyId, string approach, bool passing)
        {
            //Arrange
            Setup(approach);

            //Act
            bool result = _materialService.DeleteCopy(ssn, copyId);

            //Assert
            Assert.IsTrue(result.Equals(passing));
        }
        
        private void Setup(string approach)
        {
            ResetDatabase();
            Context context = new Context();
            switch (approach)
            {
                case "Code":
                    MaterialDa_Code materialDa_Code = new MaterialDa_Code(context);
                    LibraryDa_Code libraryDa_Code = new LibraryDa_Code(context);
                    PersonDa_Code personDa_Code = new PersonDa_Code(context);
                    CopyDa_Code copyDa_Code = new CopyDa_Code(context);
                    LendingDa_Code lendingDa_Code = new LendingDa_Code(context);
                    MaterialDm_Code materialsDm_Code = new MaterialDm_Code(materialDa_Code, libraryDa_Code, personDa_Code, copyDa_Code, lendingDa_Code);
                    _materialService = new MaterialService(materialsDm_Code);
                    break;
                case "Database":
                    MaterialsDa_Database materialDa_Db = new MaterialsDa_Database(context);
                    MaterialsDm_Database materialsDm_Db = new MaterialsDm_Database(materialDa_Db);
                    _materialService = new MaterialService(materialsDm_Db);
                    break;
            }
        }
        
        private void ResetDatabase()
        {
            string connectionString =
                "data source=localhost;initial catalog=GTL;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith("SQLCreateQuery.sql"));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                SqlConnection conn = new SqlConnection(connectionString);
                Server server = new Server(new ServerConnection(conn));
                server.ConnectionContext.ExecuteNonQuery(result);
                conn.Close();
            }
        }
    }
}
