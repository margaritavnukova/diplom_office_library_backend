using office_library_backend.Models.MyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.Repositories
{
    public class AuthorRepository : BaseRepository<AuthorsDto, Author>, IBaseRepository<AuthorsDto, Author>
    {
        public override List<AuthorsDto> Initialize()
        {
            var query = from Author in db.Author.ToList() select Author;
            var authors = query.ToList();

            List<AuthorsDto> authorsDto = new List<AuthorsDto>();
            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorsDto(author));
            }

            return authorsDto;
        }

        protected override string GetId(AuthorsDto dto)
        {
            return dto.Id;
        }

        protected override void UpdateEntity(Author entity, AuthorsDto dto)
        {
            entity = dto.ConvertToModel(db);
        }
    }
}