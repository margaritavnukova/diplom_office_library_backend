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
                DateReturned = null
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
    }
}