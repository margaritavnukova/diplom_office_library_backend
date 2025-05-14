using office_library_backend.Models;
using office_library_backend.Models.MyDto;
using office_library_backend.Models.Repositories;
using System;
using System.Data.Entity;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace office_library_backend.Controllers
{
    [RoutePrefix("api/UserBookHistory")]
    public class UserBookHistoryController : BaseController<UserBookHistoryDto, UserBookHistory>
    {
        public UserBookHistoryController() : base(new UserBookHistoryRepository()) { }

        [HttpPost]
        [Route("RegisterBookTaking")]
        public IHttpActionResult RegisterBookTaking([FromBody] BooksDto bookDto)
        {
            var readerDto = bookDto.CurrentReader;
            //проблема
            bookDto.IsTaken = !bookDto.PlannedReturnDate.HasValue ? false : true;

            if (bookDto == null || readerDto == null)
            {
                return BadRequest("Книга или читатель не дошли до сервера");
            }

            if (dbContext.AspNetUsers.FirstOrDefault(a => a.Id == readerDto.Id) == null)
            {
                return BadRequest("Читатель не найден!");
            }

            if (dbContext.Book.FirstOrDefault(a => a.Id == bookDto.Id) == null)
            {
                return BadRequest("Книга не найдена!");
            }

            if (bookDto.IsTaken)
            {
                return Conflict();
            }

            UserBookHistoryDto historyDto = new UserBookHistoryDto()
            {
                Book = bookDto,
                Reader = readerDto,
                DateTaken = DateTime.Today,
                PlannedReturnDate = DateTime.Today.AddDays(30),
            };

            try
            {
                var historyEntity = historyDto.ConvertToModel(dbContext);
                repository.Add(historyEntity, dbContext);
                dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("RegisterBookReturning")]
        public IHttpActionResult RegisterBookReturning([FromBody] BooksDto bookDto)
        {
            bookDto.IsTaken = !bookDto.PlannedReturnDate.HasValue ? false : true;

            if (bookDto == null /*|| readerDto == null*/)
            {
                return BadRequest("Книга или читатель не дошли до сервера");
            }

            //изменила юзер бук хистори потому что читатель постоянно не доходил до сервера и это просто неудобно
            //if (dbContext.AspNetUsers.FirstOrDefault(a => a.Id == readerDto.Id) == null)
            //{
            //    return BadRequest("Читатель не найден!");
            //}

            var book = dbContext.Book.FirstOrDefault(a => a.Id == bookDto.Id);

            if (book == null)
            {
                return BadRequest("Книга не найдена!");
            }
            else
            {
                var bookDtoFromDB = new BooksDto(book);
                bookDto.CurrentReader = bookDtoFromDB.IsTaken ? bookDtoFromDB.Readers.LastOrDefault() : null;
            }

            //Находим активную запись о взятии
            var activeLoan = dbContext.UserBookHistory
                .FirstOrDefaultAsync(h =>
                    h.BookId == bookDto.Id &&
                    h.UserId == bookDto.CurrentReader.Id &&
                    h.DateReturned == null);

            if (activeLoan == null)
                return BadRequest("Активный заказ не найден или книга уже возвращена");

            try
            {
                UserBookHistory historyEntity = dbContext.UserBookHistory.Where(h => h.UserId == bookDto.CurrentReader.Id).Where(h => h.BookId == bookDto.Id).FirstOrDefault();

                historyEntity.DateReturned = DateTime.Today;
                dbContext.Entry(historyEntity).State = EntityState.Modified;
                dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}