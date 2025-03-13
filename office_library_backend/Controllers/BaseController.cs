using Newtonsoft.Json;
using office_library_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using office_library_backend.Models.Repositories;

namespace office_library_backend.Controllers
{
    public abstract class BaseController<TDto, TEntity> : ApiController where TEntity : class where TDto : class
    {
        protected Entities1 dbContext = new Entities1();
        protected readonly IBaseRepository<TDto, TEntity> repository;

        public BaseController(IBaseRepository<TDto, TEntity> repo)
        {
            repository = repo;
        }

        [HttpGet]
        [Route("api/[controller]/GetAll")]
        public string GetAll()
        {
            var items = repository.GetAll();
            return JsonConvert.SerializeObject(items);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public string Get(string id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return JsonConvert.SerializeObject(item);
        }

        [HttpPost]
        [Route("api/[controller]/Post")]
        public HttpResponseMessage Post([FromBody] TDto dto)
        {
            if (dto == null)
                throw new Exception("Deserialize exception");

            TEntity item = ConvertToModel(dto);
            item = repository.Add(item);
            dbContext.SaveChanges();
            var response = Request.CreateResponse(HttpStatusCode.Created, item);
            return response;
        }

        [HttpPut]
        [Route("api/[controller]/Put/{id}")]
        public void Put(int id, [FromBody] TDto dto)
        {
            if (!repository.Update(dto))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/[controller]/Delete/{id}")]
        public void Delete(string id)
        {
            repository.Remove(id);
        }

        protected abstract TEntity ConvertToModel(TDto dto);
        protected abstract TDto ConvertToDto(TEntity model);
    }
}