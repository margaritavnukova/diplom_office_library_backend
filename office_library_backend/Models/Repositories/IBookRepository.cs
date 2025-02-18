using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using office_library_backend.Models.MyModels;

namespace office_library_backend.Models.Repositories
{
    interface IBookRepository
    {
        IEnumerable<Books> GetAll();
        Books Get(int id);
        Books Add(Books item);
        void Remove(int id);
        bool Update(Books item);
    }
}