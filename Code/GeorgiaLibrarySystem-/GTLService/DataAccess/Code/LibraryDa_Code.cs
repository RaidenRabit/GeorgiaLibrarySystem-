﻿using Core;

namespace GTLService.DataAccess.Code
{
    public class LibraryDa_Code
    {
        public virtual bool CheckLibraryName(string libraryName, Context context)
        {
            return context.Libraries.Find(libraryName) != null;
        }
    }
}