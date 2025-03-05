using office_library_backend.Models.MyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.Repositories
{
    interface IUserRepository
    {
        IEnumerable<UsersDto> GetAll();
        UsersDto Get(string id);
        AspNetUsers Add(AspNetUsers item);
        void Remove(string id);
        bool Update(UsersDto item);
    }
}