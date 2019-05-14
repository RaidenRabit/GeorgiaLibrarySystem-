using System;
using GtlService.Model;

namespace GtlService.DataAccess
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
