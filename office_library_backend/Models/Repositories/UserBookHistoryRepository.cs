using office_library_backend.Models.MyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.Repositories
{
    public class UserBookHistoryRepository : BaseRepository<UserBookHistoryDto, UserBookHistory>, IBaseRepository<UserBookHistoryDto, UserBookHistory>
    {
        public override List<UserBookHistoryDto> Initialize()
        {
            throw new NotImplementedException();
        }

        protected override string GetId(UserBookHistoryDto dto)
        {
            throw new NotImplementedException();
        }

        protected override UserBookHistory UpdateEntity(UserBookHistory entity, UserBookHistoryDto dto)
        {
            throw new NotImplementedException();
        }
    }
}