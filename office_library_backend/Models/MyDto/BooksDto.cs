using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI.WebControls;

namespace office_library_backend.Models.MyDto
{
    public class BooksDto : BaseDto<BooksDto, Book>
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Nullable<System.DateTime> Year { get; set; }
        public bool IsTaken { get; set; }
        public System.DateTime? DateOfReturning { get; set; }
        public int TakingCount { get; set; }
        public List<UsersDto> Readers { get; set; } = new List<UsersDto>();
        public string PhotoBase64 { get; set; }

        public BooksDto() { }

        public BooksDto(Book book) : base(book)
        {
            Author = book.Author1.Name;
            Title = book.Title;
            Genre = book.Genre_Dictionary.Name;
            Year = book.Year;

            DateTime? DateOfTaking = book.UserBookHistory.Count != 0
                ? book.UserBookHistory.OrderBy(b => b.DateTaken).FirstOrDefault().DateTaken
                : null;

            DateTime DateOfTakingFuture = DateOfTaking.HasValue
                ? DateOfTaking.Value.AddDays(30)
                : DateTime.Today; //костыль

            DateOfReturning = book.UserBookHistory.Count != 0
                ? book.UserBookHistory.OrderBy(b => b.DateTaken).FirstOrDefault().DateReturned
                : DateOfTakingFuture;

            IsTaken = DateOfReturning != null;
            TakingCount = book.UserBookHistory.Count();

            var readers = book.UserBookHistory;
            if (readers != null)
                foreach (var readerHistory in readers)
                {
                    Readers.Add(new UsersDto(readerHistory.AspNetUsers));
                }
        }

        public override Book ConvertToModel(Entities1 context)
        {
            // Находим или создаём Author по имени
            var author = context.Author.FirstOrDefault(a => a.Name == this.Author);
            if (author == null)
            {
                author = new Author { Name = this.Author };
                context.Author.Add(author);
                context.SaveChanges(); // Сохраняем, чтобы получить Id
            }

            // Находим или создаём Genre по имени
            var genre = context.Genre_Dictionary.FirstOrDefault(g => g.Name == this.Genre);
            if (genre == null)
            {
                genre = new Genre_Dictionary { Name = this.Genre };
                context.Genre_Dictionary.Add(genre);
                context.SaveChanges(); // Сохраняем, чтобы получить Id
            }

            // Создаём объект Book
            var book = new Book
            {
                Id = Convert.ToInt32(this.Id),
                Title = this.Title,
                Author = author.Id,
                Author1 = author,
                Genre = genre.Id,
                Genre_Dictionary = genre,
                Year = this.Year,
            };

            return book;
        }

        protected override string GetId(Book model)
        {
            return model.Id.ToString();
        }

        protected string GetPhotoBase64(Book model)
        {
            return model.Photo != null ? Convert.ToBase64String(model.Photo) : null;
        }
    }
}