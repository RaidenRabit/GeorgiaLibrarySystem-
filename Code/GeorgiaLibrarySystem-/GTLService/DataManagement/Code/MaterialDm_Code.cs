using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class MaterialDm_Code : IMaterialsDm
    {
        private readonly LibraryDa_Code _libraryDa;
        private readonly MaterialDa_Code _materialDa;
        private readonly LibrarianDa_Code _librarianDa;
        private readonly CopyDa_Code _copyDa;
        private readonly LoaningDa_Code _loaningDa;
        private readonly Context _context;

        public MaterialDm_Code(MaterialDa_Code materialDa, LibraryDa_Code libraryDa, LibrarianDa_Code librarianDa, CopyDa_Code copyDa,
            LoaningDa_Code loaningDa, Context context)
        {
            _materialDa = materialDa;
            _libraryDa = libraryDa;
            _librarianDa = librarianDa;
            _copyDa = copyDa;
            _loaningDa = loaningDa;
            _context = context;
        }
        
        public List<readAllMaterial> ReadMaterials(string materialTitle, string author, int numOfRecords = 10, string isbn = "0", string jobStatus = "0")
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var materials = _materialDa.ReadMaterials(isbn, materialTitle, author, numOfRecords, _context);
                    var copies = _copyDa.ReadCopies(isbn, jobStatus, _context);

                    List<readAllMaterial> allMaterials = new List<readAllMaterial>();
                    readAllMaterial readAllMaterial;

                    foreach (var c in copies)
                    {
                        bool unique = true;
                        foreach (var allMaterial in allMaterials)
                        {
                            if (allMaterial.ISBN == c.ISBN && allMaterial.TypeName == c.TypeName &&
                                allMaterial.Location == c.LibraryName)
                                unique = false;
                        }

                        if (unique)
                        {
                            foreach (var m in materials)
                            {
                                if (m.ISBN.Equals(c.ISBN))
                                {
                                    readAllMaterial = new readAllMaterial
                                    {
                                        ISBN = c.ISBN, TypeName = c.TypeName, Location = c.LibraryName,
                                        Description = m.Description,
                                        Author = m.Author, Title = m.Author
                                    };
                                    allMaterials.Add(readAllMaterial);
                                }
                            }

                        }

                    }

                    if (allMaterials.Count > numOfRecords)
                    {
                        for (int i = allMaterials.Count - 1; i >= numOfRecords; i--)
                        {
                            allMaterials.RemoveAt(i);
                        }
                    }

                    allMaterials = CountAvailableCopies(allMaterials, copies);
                    dbContextTransaction.Commit();
                    return allMaterials;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }
        }
        
        private List<readAllMaterial> CountAvailableCopies(List<readAllMaterial> allMaterials, List<Copy> copies)
        {
            foreach (var readAllMaterial in allMaterials)
            {
                int count = 0;
                foreach (var copy in copies)
                {
                    if (readAllMaterial.ISBN.Equals(copy.ISBN) && readAllMaterial.Location.Equals(copy.LibraryName)
                                                               && readAllMaterial.TypeName.Equals(copy.TypeName)
                                                               && (_loaningDa.GetLoan(copy.CopyID, _context) == null))
                        count++;
                }

                readAllMaterial.Available_Copies = count;
            }
            allMaterials = allMaterials.OrderByDescending(x => x.Available_Copies).ToList();

            return allMaterials;
        }

        public bool CreateMaterial(int ssn, string isbn, string library, string author, string description, string title, string typeName,
            int quantity)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    if (_librarianDa.CheckLibrarianSsn(ssn, _context) && _libraryDa.CheckLibraryName(library, _context) && _copyDa.CheckTypeName(typeName, _context))
                    {
                        if (!_materialDa.CheckMaterialIsbn(isbn, _context))
                            _materialDa.CreateMaterial( new Material {ISBN = isbn, Author = author, Title = title, Description = description}, _context);
                        for (int i = 0; i < quantity; i++)
                        {
                            _copyDa.CreateCopy(new Copy {ISBN = isbn, LibraryName = library, TypeName = typeName}, _context);
                        }

                        dbContextTransaction.Commit();
                        return true;
                    }
                    dbContextTransaction.Rollback();
                    return false;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }

        public bool DeleteMaterial(int ssn, string isbn)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    if (_librarianDa.CheckLibrarianSsn(ssn, _context) && _materialDa.CheckMaterialIsbn(isbn, _context))
                    {
                        var result = _materialDa.DeleteMaterial(isbn, _context);
                        dbContextTransaction.Commit();
                        return result;
                    }

                    dbContextTransaction.Rollback();
                    return false;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }
    }
}