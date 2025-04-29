using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class UserBookHistoryDto : BaseDto<UserBookHistoryDto, UserBookHistory>
    {
        public DateTime? DateTaken { get; set; }
        public DateTime? PlannedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public UsersDto Reader { get; set; }
        public BooksDto Book { get; set; }

        // Константа должна быть static readonly
        private static readonly int AmountOfDaysToReturnTheBook = 30;

        public UserBookHistoryDto() { }

        public UserBookHistoryDto(UserBookHistory history) : base(history)
        {
            if (history == null)
                throw new ArgumentNullException(nameof(history));

            DateTaken = history.DateTaken;
            ActualReturnDate = history.DateReturned;

            // Упрощен расчет PlannedReturnDate
            PlannedReturnDate = history.DateTaken?.AddDays(AmountOfDaysToReturnTheBook);

            if (history.AspNetUsers != null)
                Reader = new UsersDto(history.AspNetUsers);

            if (history.Book != null)
                Book = new BooksDto(history.Book);
        }

        public override UserBookHistory ConvertToModel(Entities2 context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (Reader == null)
                throw new InvalidOperationException("Reader is required");

            if (Book == null)
                throw new InvalidOperationException("Book is required");

            var reader = context.AspNetUsers.FirstOrDefault(a => a.Id == this.Reader.Id);
            if (reader == null)
                throw new Exception($"Читатель с ID {this.Reader.Id} не найден!");

            var book = context.Book.FirstOrDefault(a => a.Id == this.Book.Id);
            if (book == null)
                throw new Exception($"Книга с ID {this.Book.Id} не найдена!");

            return new UserBookHistory
            {
                DateTaken = this.DateTaken,
                DateReturned = this.ActualReturnDate, // Исправлено с DateReturned на ActualReturnDate
                UserId = reader.Id,
                AspNetUsers = reader,
                BookId = book.Id,
                Book = book
            };
        }

        protected override string GetId(UserBookHistory model)
        {
            return model?.Id.ToString() ?? string.Empty;
        }

        // Добавлен метод для проверки просрочки
        public bool IsOverdue =>
            !ActualReturnDate.HasValue &&
            PlannedReturnDate.HasValue &&
            PlannedReturnDate.Value < DateTime.Now;
    }
}