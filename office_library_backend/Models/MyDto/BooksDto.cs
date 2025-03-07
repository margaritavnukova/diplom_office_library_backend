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
        public string PhotoBase64 { get; set; }
        public bool IsTaken { get; set; }
        public System.DateTime? DateOfReturning { get; set; }
        public int TakingCount { get; set; }
        public List<UsersDto> Readers { get; set; } = new List<UsersDto>();

        public BooksDto() { }

        public BooksDto(Book book)
        {
            Id = book.Id;
            Author = book.Author1.Name;
            Title = book.Title;
            Genre = book.Genre_Dictionary.Name;
            Year = book.Year;

            PhotoBase64 = book.Photo != null
                ? Convert.ToBase64String(book.Photo)
                : null;

            DateOfReturning = book.UserBookHistory.Count != 0 
                ? book.UserBookHistory.OrderBy(b => b.DateTaken).FirstOrDefault().DateReturned
                : null;
            IsTaken = DateOfReturning != null;
            TakingCount = book.UserBookHistory.Count();

            var readers = book.UserBookHistory;
            if (readers != null)
                foreach (var readerHistory in readers)
                {
                    Readers.Add(new UsersDto(readerHistory.AspNetUsers));
                }
        }
    }
}