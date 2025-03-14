using office_library_backend.Models;
using office_library_backend.Models.MyDto;
using office_library_backend.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace office_library_backend.Controllers
{
    public class AuthorController : BaseController<AuthorsDto, Author>
    {
        public AuthorController() : base(new AuthorRepository()) { }
    }
}