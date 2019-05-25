using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Tests
{
    public static class DatabaseTesting
    {
        public static void ResetDatabase()
        {
            string connection =
                "data source=localhost;initial catalog=GTL;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            
            string script = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Database/InsertScript.sql"));
            
            using (SqlConnection conn = new SqlConnection(connection))
            {
                Server db = new Server(new ServerConnection(conn));
                db.ConnectionContext.ExecuteNonQuery(script);      
            }
        }
    }
}
