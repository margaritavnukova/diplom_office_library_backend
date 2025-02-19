using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;

namespace office_library_backend.Models.Repositories
{
    interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book Get(int id);
        Book Add(Book item);
        void Remove(int id);
        bool Update(BooksDto item);
    }
}