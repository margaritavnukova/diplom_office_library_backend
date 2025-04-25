using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;

namespace office_library_backend.Models.Repositories
{
    public class BookRepository : BaseRepository<BooksDto, Book>, IBaseRepository<BooksDto, Book>
    {
        public override List<BooksDto> Initialize()
        {
            var query = from Book in db.Book.ToList() select Book;
            var books = query.ToList();

            List<BooksDto> booksDto = new List<BooksDto>();
            foreach (var book in books)
            {
                booksDto.Add(new BooksDto(book));
            }

            return booksDto;
        }

        protected override string GetId(BooksDto dto)
        {
            return dto.Id;
        }
    }
}