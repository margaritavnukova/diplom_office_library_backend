using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace office_library_backend.Models.MyDto
{
    public class BooksDto : BaseDto<BooksDto, Book>
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime? Year { get; set; }
        public bool IsTaken { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? DateReturned { get; set; }
        public int TakingCount { get; set; }
        public List<UsersDto> Readers { get; set; } = new List<UsersDto>();
        public UsersDto CurrentReader { get; set; }
        public string PhotoBase64 { get; set; }


        public BooksDto() { }

        public BooksDto(Book book) : base(book)
        {
            Id = book.Id;

            Author = book.Author1.Name;
            Title = book.Title;
            Genre = book.Genre_Dictionary.Name;
            Year = book.Year;

            // Получаем последнюю запись о взятии книги (сортировка по убыванию)
            var lastHistoryRecord = book.UserBookHistory
                .OrderByDescending(h => h.DateTaken)
                .FirstOrDefault();

            // Устанавливаем даты
            DateTaken = lastHistoryRecord?.DateTaken;
            DateReturned = lastHistoryRecord?.DateReturned;

            // Книга считается взятой, если есть дата взятия и нет даты возврата
            IsTaken = lastHistoryRecord != null && lastHistoryRecord.DateTaken != null
                      && lastHistoryRecord.DateReturned == null;

            // Общее количество взятий книги
            TakingCount = book.UserBookHistory.Count;

            var readers = book.UserBookHistory.OrderBy(h => h.DateTaken);
            if (readers != null)
                foreach (var readerHistory in readers)
                {
                    Readers.Add(new UsersDto(readerHistory.AspNetUsers));
                }

            CurrentReader = IsTaken
                ? Readers.Last()
                : null;

            PhotoBase64 = GetPhotoBase64(book);
        }

        public override Book ConvertToModel(Entities2 context)
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
                Id = this.Id,
                Title = this.Title,
                Author = author.Id,
                Author1 = author,
                Genre = genre.Id,
                Genre_Dictionary = genre,
                Year = this.Year,
                Photo = PhotoBase64 != null ? Encoding.UTF8.GetBytes(PhotoBase64) : null
            };

            return book;
        }

        protected override string GetId(Book model)
        {
            return model.Id;
        }

        protected string GetPhotoBase64(Book model)
        {
            return model.Photo != null ? Convert.ToBase64String(model.Photo) : null;
        }
    }
}