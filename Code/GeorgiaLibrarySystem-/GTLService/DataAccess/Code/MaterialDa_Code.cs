using System.Collections.Generic;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class MaterialDa_Code
    {
        public virtual List<Material> ReadMaterials(string isbn, string title, string author, int numOfRecords, Context context)
        {
            return context.Materials
                .Where(x => (isbn.Equals("0") || x.ISBN.Equals(isbn)) &&
                            x.Author.Contains(author) &&
                            x.Title.Contains(title)
                )
                .Take(numOfRecords)
                .ToList();
        }

        public virtual bool CheckMaterialIsbn(string isbn, Context context)
        {
            return context.Materials.Find(isbn) != null;
        }

        public virtual bool CreateMaterial(Material material, Context context)
        {
            context.Materials.Add(material);
            return context.SaveChanges() > 0;
        }

        //todo fix?
        public virtual bool DeleteMaterial(string isbn, Context context)
        {
            context.Materials.Remove(context.Materials.Single(o => o.ISBN.Equals(isbn)));
            return context.SaveChanges() > 0;
        }

    }
}