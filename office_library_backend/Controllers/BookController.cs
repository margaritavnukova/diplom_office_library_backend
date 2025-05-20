using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using office_library_backend.Models.Repositories;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace office_library_backend.Controllers
{
    [RoutePrefix("api/Book")]
    public class BookController : BaseController<BooksDto, Book>
    {
        public BookController() : base(new BookRepository()) { }

        [HttpGet]
        //[ActionName("GetByReader")]
        [Route("GetByReader/{email}")]
        public IHttpActionResult GetBooksByReader([FromUri] string email)
        {
            var editedEmail = email.Replace('-', '.');
            var books = repository.GetAll().Where(
                b => b.Readers.Any(
                    reader => reader.Email == editedEmail)
                );
            return Ok(books);
        }

        [HttpDelete]
        //[ActionName("Delete")]
        [Route("Delete")]
        override public IHttpActionResult Delete([FromBody] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID не может быть пустым");
            }

            var book = dbContext.Book.FirstOrDefault(a => a.Id == id);
            if (book == null)
            {
                return BadRequest("Книга не найдена!");
            }

            // Получаем последнюю запись о взятии книги
            var lastHistoryRecord = book.UserBookHistory?
                .OrderByDescending(h => h.DateTaken)
                .FirstOrDefault();

            bool hasHistory = lastHistoryRecord != null;

            // Книга считается взятой, если есть дата взятия и нет даты возврата
            bool isTaken = lastHistoryRecord != null &&
                lastHistoryRecord.DateTaken != null &&
                lastHistoryRecord.DateReturned == null;

            if (hasHistory)
            {
                // Мягкое удаление (если есть история)
                book.IsDeleted = true;
                dbContext.SaveChanges();

                return Ok("Книга перемещена в архив (есть история выдачи)");
            }
            else if (!isTaken)
            {
                // Полное удаление (нет истории)
                repository.Remove(id);
                return Ok("Книга полностью удалена (без истории)");
            }
            else
            {
                return Conflict();
            }
        }

        [HttpGet]
        [Route("Search")]
        public IHttpActionResult SearchBooks([FromBody] string title)
        {
            try
            {
                // Если строка поиска пустая - возвращаем все книги
                if (string.IsNullOrWhiteSpace(title))
                {
                    var allBooks = dbContext.Book.ToList();
                    return Ok(allBooks);
                }

                // Ищем книги, содержащие указанную строку в названии (без учета регистра)
                var foundBooks = dbContext.Book
                    .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                    .ToList();

                return Ok(foundBooks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}