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
        public AuthorsDto Author { get; set; }
        public string Title { get; set; }
        public GenresDto Genre { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime? Year { get; set; }
        public bool IsTaken { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? PlannedReturnDate { get; set; } 
        public DateTime? ActualReturnDate { get; set; }   

        public int TakingCount { get; set; }
        public List<UsersDto> Readers { get; set; } = new List<UsersDto>();
        public UsersDto CurrentReader { get; set; }
        public string PhotoBase64 { get; set; }

        public BooksDto() { }

        public BooksDto(Book book) : base(book)
        {
            Id = book.Id;
            Author = new AuthorsDto(book.Author1);
            Title = book.Title;
            Genre = new GenresDto(book.Genre_Dictionary);
            IsDeleted = book.IsDeleted;
            Year = book.Year;

            // Получаем последнюю запись о взятии книги
            var lastHistoryRecord = book.UserBookHistory?
                .OrderByDescending(h => h.DateTaken)
                .FirstOrDefault();

            // Устанавливаем даты
            DateTaken = lastHistoryRecord?.DateTaken;
            PlannedReturnDate = lastHistoryRecord?.DateTaken?.AddDays(30); // Рассчитываем ожидаемую дату возврата
            ActualReturnDate = lastHistoryRecord?.DateReturned;

            // Книга считается взятой, если есть дата взятия и нет даты возврата
            IsTaken = lastHistoryRecord != null &&
                lastHistoryRecord.DateTaken != null &&
                lastHistoryRecord.DateReturned == null;

            // Общее количество взятий книги
            TakingCount = book.UserBookHistory?.Count ?? 0;

            // Заполняем список читателей
            if (book.UserBookHistory != null)
            {
                foreach (var readerHistory in book.UserBookHistory.OrderBy(h => h.DateTaken))
                {
                    if (readerHistory.AspNetUsers != null)
                    {
                        Readers.Add(new UsersDto(readerHistory.AspNetUsers));
                    }
                }
            }

            // Текущий читатель - последний, кто взял книгу и не вернул
            CurrentReader = IsTaken ? Readers.LastOrDefault() : null;

            PhotoBase64 = GetPhotoBase64(book);
        }

        public override Book ConvertToModel(Entities2 context)
        {
            // Если Id == -1, генерируем новый GUID
            var newId = this.Id == "-1" ? Guid.NewGuid().ToString() : this.Id;

            // Находим или создаём Author по имени
            var author = context.Author.FirstOrDefault(a => a.Id == this.Author.Id);
            if (author == null && !string.IsNullOrEmpty(this.Author.Name))
            {
                author = this.Author.ConvertToModel(context);
                context.Author.Add(author);
                context.SaveChanges();
            }

            // Находим или создаём Genre по имени
            var genre = context.Genre_Dictionary.FirstOrDefault(g => g.Id == this.Genre.Id);
            if (genre == null && !string.IsNullOrEmpty(this.Genre.Name))
            {
                genre = this.Genre.ConvertToModel(context);
                context.Genre_Dictionary.Add(genre);
                context.SaveChanges();
            }

            // Создаём объект Book
            var book = new Book
            {
                Id = newId,
                Title = this.Title,
                Author = author?.Id ?? "", // Добавлена проверка на null
                Author1 = author,
                Genre = genre?.Id ?? "",    // Добавлена проверка на null
                Genre_Dictionary = genre,
                IsDeleted = this.IsDeleted,
                Year = this.Year,
                Photo = !string.IsNullOrEmpty(PhotoBase64) ?
                       Convert.FromBase64String(PhotoBase64) :
                       null
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