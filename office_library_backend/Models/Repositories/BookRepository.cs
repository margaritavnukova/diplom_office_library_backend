using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;

namespace office_library_backend.Models.Repositories
{
    public class BookRepository : IBookRepository
    {
        private int _nextId = 1;
        private Entities1 db = new Entities1();

        public List<BooksDto> Initialize()
        {
            var query = from Book in db.Book.ToList() select Book;
            var books = query.ToList();

            List<BooksDto> booksDto = new List<BooksDto>();
            foreach (var book in books)
            {
                booksDto.Add(new BooksDto(book));
            }

            return booksDto;
        }

        public Book Add(Book book)
        {
            if (book == null)
                throw new ArgumentNullException();
            book.Id = _nextId++;
            db.Book.Add(book);
            db.SaveChanges();
            return book;
        }

        public BooksDto Get(int id)
        {
            var books = Initialize();
            return books.Find(p => p.Id == id);
        }

        public IEnumerable<BooksDto> GetAll()
        {
            var books = Initialize();
            return books;
        }

        public void Remove(int id)
        {
            var bookToDelete = db.Book.Where(b => b.Id == id).FirstOrDefault();

            db.Book.Remove(bookToDelete);
            db.SaveChanges();
        }

        public bool Update(BooksDto book)
        {
            var books = Initialize();
            if (book == null)
                throw new ArgumentNullException();
            int index = books.FindIndex(p => p.Id == book.Id);
            if (index == -1)
                return false;

            var bookToUpdate = db.Book.Where(p => p.Id == book.Id).FirstOrDefault();
            
            bookToUpdate = book.ConvertToBookModel(db);

            db.Entry(bookToUpdate).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}