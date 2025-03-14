using Newtonsoft.Json;
using office_library_backend.Models.MyDto;
using office_library_backend.Models.Repositories;
using office_library_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace office_library_backend.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseController<UsersDto, AspNetUsers>
    {
        public UserController() : base(new UserRepository()) { }

        [HttpGet]
        //[ActionName("GetByEmail")]
        [Route("GetByEmail/{email}")]
        public IHttpActionResult GetUserByEmail([FromUri] string email)
        {
            var editedEmail = email.Replace('-', '.');
            var user = repository.GetAll().Where(
                u => string.Equals(u.Email, editedEmail, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return Ok(user);
        }
    }
}