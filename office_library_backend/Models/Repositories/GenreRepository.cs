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
            var query = from Genre in db.Genre_Dictionary.ToList() select Genre;
            var genres = query.ToList();

            List<GenresDto> genresDto = new List<GenresDto>();
            foreach (var genre in genres)
            {
                genresDto.Add(new GenresDto(genre));
            }

            return genresDto;
        }

        protected override string GetId(GenresDto dto)
        {
            return dto.Id;
        }

        //protected override Genre_Dictionary UpdateEntity(Genre_Dictionary entity, GenresDto dto)
        //{
        //    entity = dto.ConvertToModel(db); ;
        //    return entity;
        //}
    }
}