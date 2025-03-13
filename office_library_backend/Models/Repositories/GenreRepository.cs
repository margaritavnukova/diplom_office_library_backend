using office_library_backend.Models.MyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.Repositories
{
    public class GenreRepository : BaseRepository<GenresDto, Genre_Dictionary>, IBaseRepository<GenresDto, Genre_Dictionary>
    {
        public override List<GenresDto> Initialize()
        {
            throw new NotImplementedException();
        }

        protected override string GetId(GenresDto dto)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateEntity(Genre_Dictionary entity, GenresDto dto)
        {
            throw new NotImplementedException();
        }
    }
}