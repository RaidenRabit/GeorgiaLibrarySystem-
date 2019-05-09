using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GtlService.Model;

namespace GtlService.DataManagement
{
    class TestClass
    {
        /* Example */
        public static void Main(string[] args)
        {
            using (GTLEntities context = new GTLEntities())
            {
                Address a = new Address
                {
                    City = "Aalborg",
                    PostalCode = 9200,
                    Street = "Nibevej",
                    Number = 12
                };
                context.Addresses.Add(a);
                context.SaveChanges();
                Console.WriteLine("done");
            }
        }
    }
}
