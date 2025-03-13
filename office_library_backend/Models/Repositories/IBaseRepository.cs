using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.Repositories
{
    public interface IBaseRepository<TDto, TEntity>
    {
        IEnumerable<TDto> GetAll();
        TDto Get(string id);
        TEntity Add(TEntity item);
        void Remove(string id);
        bool Update(TDto item);
    }
}