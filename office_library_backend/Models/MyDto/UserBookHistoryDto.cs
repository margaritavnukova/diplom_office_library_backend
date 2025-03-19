using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class UserBookHistoryDto : BaseDto<UserBookHistoryDto, UserBookHistory>
    {
        //public string UserId { get; set; }
        //public string BookId { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? DateOfReturningFuture { get; set; }
        public DateTime? DateReturned { get; set; }

        public UsersDto Reader { get; set; }
        public BooksDto Book { get; set; }

        private static int amountOfDaysToReturnTheBook = 30;

        public UserBookHistoryDto() { }

        public UserBookHistoryDto(UserBookHistory history) : base(history)
        {
            DateTaken = history.DateTaken;
            DateReturned = history.DateReturned;

            var Date = DateReturned.HasValue 
                ? DateReturned.Value 
                : DateTime.MinValue;

            // Считаю дату возврата книги в будущем, если книгу еще не вернули
            DateOfReturningFuture = DateTaken.HasValue && Book.IsTaken
                ? DateTaken.Value.AddDays(amountOfDaysToReturnTheBook)
                : Date;

            Reader = new UsersDto(history.AspNetUsers);
            Book = new BooksDto(history.Book);
        }

        public override UserBookHistory ConvertToModel(Entities2 context)
        {
            var reader = context.AspNetUsers.FirstOrDefault(a => a.Id == this.Reader.Id);
            if (reader == null)
            {
                throw new Exception("Читатель не найден!");
            }

            var book = context.Book.FirstOrDefault(a => a.Id == this.Book.Id);
            if (book == null)
            {
                throw new Exception("Книга не найдена!");
            }

            return new UserBookHistory
            {
                DateTaken = this.DateTaken,
                DateReturned = this.DateReturned,
                UserId = reader.Id,
                AspNetUsers = reader,
                BookId = book.Id,
                Book = book
            };
        }

        protected override string GetId(UserBookHistory model)
        {
            return "";
        }
    }
}