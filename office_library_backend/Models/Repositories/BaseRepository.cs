using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;

namespace office_library_backend.Models.Repositories
{
    public abstract class BaseRepository<TDto, TEntity> where TDto : class where TEntity : class 
    {
        protected Entities1 db = new Entities1();

        public abstract List<TDto> Initialize();

        public TEntity Add(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException();
            db.Set<TEntity>().Add(item);
            db.SaveChanges();
            return item;
        }

        public TDto Get(string id)
        {
            var items = Initialize();
            return items.Find(p => GetId(p) == id);
        }

        public IEnumerable<TDto> GetAll()
        {
            return Initialize();
        }

        public void Remove(string id)
        {
            var itemToDelete = db.Set<TEntity>().Find(id);
            if (itemToDelete != null)
            {
                db.Set<TEntity>().Remove(itemToDelete);
                db.SaveChanges();
            }
        }

        public bool Update(TDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            var items = Initialize();
            int index = items.FindIndex(p => GetId(p) == GetId(dto));
            if (index == -1)
                return false;

            var itemToUpdate = db.Set<TEntity>().Find(GetId(dto));
            if (itemToUpdate == null)
                return false;

            UpdateEntity(itemToUpdate, dto);
            db.Entry(itemToUpdate).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        protected abstract string GetId(TDto dto);
        protected abstract void UpdateEntity(TEntity entity, TDto dto);
    }
}