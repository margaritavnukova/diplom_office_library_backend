using office_library_backend.Models.MyDto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.Repositories
{
    public class UserRepository : BaseRepository< UsersDto, AspNetUsers>, IBaseRepository<UsersDto, AspNetUsers>
    {
        public override List<UsersDto> Initialize()
        {
            var query = from AspNetUsers in db.AspNetUsers.ToList() select AspNetUsers;
            var users = query.ToList();

            List<UsersDto> usersDto = new List<UsersDto>();
            foreach (var user in users)
            {
                usersDto.Add(new UsersDto(user));
            }

            return usersDto;
        }

        protected override string GetId(UsersDto dto)
        {
            return dto.Id; 
        }

        protected override void UpdateEntity(AspNetUsers entity, UsersDto dto)
        {
            entity = dto.ConvertToModel(db);
        }
    }
}