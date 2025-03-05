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
using System.Web.Mvc;

namespace office_library_backend.Controllers
{
    public class UserController : ApiController
    {
        Entities1 dbContext = new Entities1();
        static readonly IUserRepository repository = new UserRepository();

        [System.Web.Http.HttpGet]
        public string GetAllUsers()
        {
            var users = repository.GetAll().Select(user => new UsersDto()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            });

            return JsonConvert.SerializeObject(users);
        }


        //public UsersDto GetUser(string id)
        //{
        //    UsersDto item = repository.Get(id);
        //    if (item == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return item;
        //}

        // https://localhost:44319/api/User/GetUserByEmail?email=test1@mail.com
        public string GetUserByEmail(string email)
        {
            var user = repository.GetAll().Where(
                u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return JsonConvert.SerializeObject(user);
        }

        public HttpResponseMessage PostUser(AspNetUsers user)
        {
            if (user.PhoneNumber != null)
                user = repository.Add(user);
            var response = Request.CreateResponse<AspNetUsers>(HttpStatusCode.Created, user);

            string uri = Url.Link("DefaultApi", new { id = user.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutUsers(string id, UsersDto user)
        {
            user.Id = id;
            if (!repository.Update(user))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteUser(string id)
        {
            repository.Remove(id);
        }
    }
}