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
    }
}