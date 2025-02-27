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

        public Book Add(Book item)
        {
            if (item == null)
                throw new ArgumentNullException();
            item.Id = _nextId++;
            db.Book.Add(item);
            db.SaveChanges();
            return item;
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
            var itemToDelete = db.Book.Where(b => b.Id == id).FirstOrDefault();

            db.Book.Remove(itemToDelete);
            db.SaveChanges();
        }

        public bool Update(BooksDto item)
        {
            var books = Initialize();
            if (item == null)
                throw new ArgumentNullException();
            int index = books.FindIndex(p => p.Id == item.Id);
            if (index == -1)
                return false;

            var itemToDelete = db.Book.Where(p => p.Id == item.Id).FirstOrDefault();
            itemToDelete.Title = item.Title;
            db.Entry(itemToDelete).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}