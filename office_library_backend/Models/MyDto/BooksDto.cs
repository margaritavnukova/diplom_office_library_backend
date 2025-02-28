using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI.WebControls;

namespace office_library_backend.Models.MyDto
{
    public class BooksDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Nullable<System.DateTime> Year { get; set; }

        public virtual List<UserBookHistory> UserBookHistory { get; set; } = new List<UserBookHistory>();
        public List<UsersDto> Readers { get; set; } = new List<UsersDto>();

        public BooksDto()
        {
            Id = -1;
            Author = String.Empty;
            Title = String.Empty;
            Genre = String.Empty;
            Year = null;
            Readers = null;
        }

        public BooksDto(Book book)
        {
            Id = book.Id;
            Author = book.Author1.Name;
            Title = book.Title;
            Genre = book.Genre_Dictionary.Name;
            Year = book.Year;

            var readers = book.UserBookHistory;

            if (readers != null)
                foreach (var readerHistory in readers)
                {
                    Readers.Add(new UsersDto(readerHistory.AspNetUsers));
                }
        }
    }
}