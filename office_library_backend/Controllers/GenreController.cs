using office_library_backend.Models.MyDto;
using office_library_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using office_library_backend.Models.Repositories;

namespace office_library_backend.Controllers
{
    public class GenreController : BaseController<GenresDto, Genre_Dictionary>
    {
        public GenreController(IBaseRepository<GenresDto, Genre_Dictionary> repo) : base(repo)
        {  }

        protected override GenresDto ConvertToDto(Genre_Dictionary model)
        {
            return new GenresDto(model);
        }

        protected override Genre_Dictionary ConvertToModel(GenresDto dto)
        {
            return dto.ConvertToModel(dbContext);
        }
    }
}