using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using office_library_backend.Models;
using office_library_backend.Models.MyDto;

namespace office_library_backend.Models.Repositories
{
    public abstract class BaseRepository<TDto, TEntity> where TDto : BaseDto<TDto, TEntity> where TEntity : class 
    {
        protected Entities2 db = new Entities2();

        public abstract List<TDto> Initialize();

        public TEntity Add(TEntity item, DbContext dbContext)
        {
            if (item == null)
                throw new ArgumentNullException();
            dbContext.Set<TEntity>().Add(item); // Используем переданный контекст
            dbContext.SaveChanges();
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

        public void Remove<TKey>(TKey id)
        {
            var itemToDelete = db.Set<TEntity>().Find(id);
            if (itemToDelete != null)
            {
                db.Set<TEntity>().Remove(itemToDelete);
                db.SaveChanges();
            }
        }

        public TDto Update(TDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException();

            var items = Initialize();
            int index = items.FindIndex(p => GetId(p) == GetId(dto));
            if (index == -1)
                throw new Exception();

            var itemToUpdate = db.Set<TEntity>().Find(GetId(dto));
            if (itemToUpdate == null)
                throw new Exception();

            itemToUpdate = UpdateEntity(itemToUpdate, dto);
            db.Entry(itemToUpdate).State = EntityState.Modified;
            db.SaveChanges();

            return dto;
        }

        protected abstract string GetId(TDto dto);
        protected virtual TEntity UpdateEntity(TEntity entity, TDto dto)
        {
            // Создаем временную модель из DTO
            var tempModel = dto.ConvertToModel(db);

            // Копируем все свойства из временной модели в существующую сущность
            db.Entry(entity).CurrentValues.SetValues(tempModel);

            return entity;
        }
    }
}