using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class BooksDto
    {
        public BooksDto()
        {
            Id = -1;
            Author = String.Empty;
            Title = String.Empty;
            Genre = String.Empty;
            Year = null;
            Readers = null;
        }

        public BooksDto(int id, string author, string title, string genre, DateTime? year, List<UsersDto> readers)
        {
            Id = id;
            Author = author;
            Title = title;
            Genre = genre;
            Year = year;
            Readers = readers;
        }

        public BooksDto(Book book)
        {
            Id = book.Id;
            Author = book.Author1.Name;
            Title = book.Title;
            Genre = book.Genre_Dictionary.Name;
            Year = book.Year;
            //Readers = book.UserBookHistory.Where(u => u.BookId == book.Id).FirstOrDefault();
        }

        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Nullable<System.DateTime> Year { get; set; }

        public List<UsersDto> Readers { get; set; } = new List<UsersDto>();
    }
}