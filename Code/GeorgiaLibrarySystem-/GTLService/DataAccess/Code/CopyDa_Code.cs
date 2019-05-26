using System.Collections.Generic;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class CopyDa_Code
    {
        public virtual Copy GetCopy(int id, Context context)
        {
            return context.Copies.Find(id);
        }
        
        public virtual bool CheckTypeName(string typeName, Context context)
        {
            return context.Copies.FirstOrDefault(x => x.TypeName.Equals(typeName)) != null;
        }
        
        public virtual bool CreateCopy(Copy copy, Context context)
        {
                context.Copies.Add(copy);
                return context.SaveChanges() > 0;
        }
        
        public virtual List<Copy> ReadCopies(string isbn, string typeName, Context context)
        {
            return context.Copies
                .Where(x => (isbn.Equals("0") || x.ISBN.Equals(isbn)) &&
                            (typeName.Contains("0") || x.TypeName.Equals(typeName)))
                .ToList();
        }
        
        public virtual bool DeleteCopy(Copy copy, Context context)
        {
            context.Copies.Remove(copy);
            return context.SaveChanges() > 0;
        }

        public virtual List<Copy> GetAvailableCopyId(string isbn, Context context)
        {
            return context.Copies.Where(x => x.ISBN.Equals(isbn)).ToList();
        }
    }
}