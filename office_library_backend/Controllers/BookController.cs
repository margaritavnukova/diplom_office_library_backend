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
    //[Authorize(Roles = "Admin")]
    public class BookController : ApiController
    {
        Entities1 dbContext = new Entities1();
        static readonly IBookRepository repository = new BookRepository();

        [HttpGet]
        [Route("api/Book/GetAll")]
        public string GetAllBooks()
        {
            var books = repository.GetAll().Select(book => new BooksDto()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                Year = book.Year,
                PhotoBase64 = book.PhotoBase64,
                IsTaken = book.IsTaken,
                DateOfReturning = book.DateOfReturning,
                TakingCount = book.TakingCount,
                Readers = book.Readers,
            });

            return JsonConvert.SerializeObject(books);
        }

        [HttpGet]
        [Route("api/Book/{id}")]
        public string GetBook([FromUri] int id)
        {
            BooksDto item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return JsonConvert.SerializeObject(item);
        }

        [HttpGet]
        [Route("api/Book/GetByReader/{email}")]
        public string GetBooksByReader([FromUri] string email)
        {
            var editedEmail = email.Replace('-', '.');
            var books = repository.GetAll().Where(
                b => b.Readers.Any(
                    reader => reader.Email == editedEmail)
                );
            return JsonConvert.SerializeObject(books);
        }

        [HttpPost]
        [Route("api/Book/Post")]
        public HttpResponseMessage PostBook([FromBody] BooksDto bookDto)
        {
            if (bookDto == null)
                throw new Exception("Deserialize exception"); 

            Book book = bookDto.ConvertToBookModel(dbContext);
            book.Id = 0;
            if (book.Title != null)
                book = repository.Add(book);
            dbContext.SaveChanges();
            var response = Request.CreateResponse<Book>(HttpStatusCode.Created, book);

            //string uri = Url.Link("DefaultApi", new { id = book.Id });
            //response.Headers.Location = new Uri(uri);

            return response;
        }

        [HttpPut]
        [Route("api/Book/Put/{id}")]
        public void PutBooks([FromUri] int id, [FromBody] BooksDto bookDto)
        {
            bookDto.Id = id;
            if (!repository.Update(bookDto))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/Book/Delete/{id}")]
        public void DeleteBook(int id)
        {
            repository.Remove(id);
        }
    }
}