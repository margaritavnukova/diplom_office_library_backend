using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using office_library_backend.Models.MyModels;

namespace office_library_backend.Models.Repositories
{
    public class BookRepository : IBookRepository
    {
        private int _nextId = 1;
        //private Vnukova_ISRPOEntities1 db = new Vnukova_ISRPOEntities1();

        public List<Books> Initialize()
        {
            var query = from Book in db.Books.ToList() select Book;
            return query.ToList();
        }

        public Books Add(Books item)
        {
            //var cruises = Initialize();
            if (item == null)
                throw new ArgumentNullException();
            item.Id = _nextId++;
            db.Books.Add(item);
            db.SaveChanges();
            //cruises.Add(item);
            return item;
        }

        public Books Get(int id)
        {
            var cruises = Initialize();
            return cruises.Find(p => p.Id == id);
        }

        public IEnumerable<Books> GetAll()
        {
            var cruises = Initialize();
            return cruises;
        }

        public void Remove(int id)
        {
            var itemToDelete = db.Books.Where(p => p.Id == id).FirstOrDefault();

            db.Books.Remove(itemToDelete);
            //db.Entry(itemToDelete).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool Update(Books item)
        {
            var cruises = Initialize();
            if (item == null)
                throw new ArgumentNullException();
            int index = cruises.FindIndex(p => p.Id == item.Id);
            if (index == -1)
                return false;

            //cruises.RemoveAt(index);
            //cruises.Add(item);

            var itemToDelete = db.Books.Where(p => p.Id == item.Id).FirstOrDefault();
            itemToDelete.Name = item.Name;
            itemToDelete.destination = item.destination;
            db.Entry(itemToDelete).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}