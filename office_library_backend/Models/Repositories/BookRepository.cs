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
        private OfficeLibraryDataEntities db = new OfficeLibraryDataEntities();

        public List<Book> Initialize()
        {
            var query = from Book in db.Book.ToList() select Book;
            return query.ToList();
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

        public Book Get(int id)
        {
            var books = Initialize();
            return books.Find(p => p.Id == id);
        }

        public IEnumerable<Book> GetAll()
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
            itemToDelete.Author = item.Author;
            db.Entry(itemToDelete).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}