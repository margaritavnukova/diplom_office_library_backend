﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using office_library_backend.Models.Repositories;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;

namespace office_library_backend.Controllers
{
    public class BookController : ApiController
    {
        //static string IP = "44319";

        static readonly IBookRepository repository = new BookRepository();

        public IEnumerable<BooksDto> GetAllBooks()
        {
            return repository.GetAll().Select(b => new BooksDto()
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                Year = b.Year,
                //Ships = p.BooksToShips.Select(s => s.ShipID.Value.ToString()).ToList(),
            });
        }


        public Book GetBook(int id)
        {
            Book item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        public IEnumerable<Book> GetBooksByTitle(string t)
        {
            return repository.GetAll().Where(
                b => string.Equals(b.Title, t, StringComparison.OrdinalIgnoreCase));
        }

        public HttpResponseMessage PostBook(Book book)
        {
            if (book.Title != null)
                book = repository.Add(book);
            var response = Request.CreateResponse<Book>(HttpStatusCode.Created, book);

            string uri = Url.Link("DefaultApi", new { id = book.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutBooks(int id, BooksDto book)
        {
            book.Id = id;
            if (!repository.Update(book))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteBook(int id)
        {
            repository.Remove(id);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net;
//using System.Web;
//using System.Web.Http;
//using System.Web.Mvc;
//using pr2A.Models;
//using System.Threading.Tasks;

//namespace pr2A.Controllers
//{

//    public class CruisesController : ApiController
//    {
//        //static string IP = "57660";

//        static readonly ICruiseRepository repository = new CruiseRepository();

//        public IEnumerable<CruisesDto> GetAllCruises()
//        {
//            return repository.GetAll().Select(p => new CruisesDto()
//            {
//                Id = p.Id,
//                Name = p.Name,
//                destination = p.destination,
//                //Ships = p.CruisesToShips.Select(s => s.ShipID.Value.ToString()).ToList(),
//            });
//        }


//        public Cruises GetCruise(int id)
//        {
//            Cruises item = repository.Get(id);
//            if (item == null)
//            {
//                throw new HttpResponseException(HttpStatusCode.NotFound);
//            }
//            return item;
//        }

//        public IEnumerable<Cruises> GetCruisesByDestination(string d)
//        {
//            return repository.GetAll().Where(
//                p => string.Equals(p.destination, d, StringComparison.OrdinalIgnoreCase));
//        }

//        public HttpResponseMessage PostCruise(Cruises book)
//        {
//            if (book.Name != null)
//                book = repository.Add(book);
//            var response = Request.CreateResponse<Cruises>(HttpStatusCode.Created, book);

//            string uri = Url.Link("DefaultApi", new { id = book.Id });
//            response.Headers.Location = new Uri(uri);
//            return response;
//        }

//        public void PutCruises(int id, Cruises book)
//        {
//            book.Id = id;
//            if (!repository.Update(book))
//            {
//                throw new HttpResponseException(HttpStatusCode.NotFound);
//            }

//        }

//        public void DeleteCruise(int id)
//        {
//            repository.Remove(id);
//        }
//    }
//}