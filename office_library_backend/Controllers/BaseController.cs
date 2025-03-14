using Newtonsoft.Json;
using office_library_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using office_library_backend.Models.Repositories;
using office_library_backend.Models.MyDto;
using System.Net;

namespace office_library_backend.Controllers
{
    [RoutePrefix("api/{controller}")]
    public abstract class BaseController<TDto, TEntity> : ApiController where TEntity : class where TDto : BaseDto<TDto, TEntity>
    {
        protected Entities1 dbContext = new Entities1();
        protected readonly IBaseRepository<TDto, TEntity> repository;

        public BaseController(IBaseRepository<TDto, TEntity> repo)
        {
            repository = repo;
        }

        [HttpGet]
        //[ActionName("GetAll")]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var items = repository.GetAll();
            return Ok(items);
        }

        [HttpGet]
        //[ActionName("GetOne")]
        [Route("GetOne/{id}")]
        public IHttpActionResult Get([FromUri] string id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(item);
        }

        [HttpPost]
        //[ActionName("Post")]
        [Route("Post")]
        public HttpResponseMessage Post([FromBody] TDto dto)
        {
            if (dto == null)
                throw new Exception("Deserialize exception");

            TEntity item = dto.ConvertToModel(dbContext);
            item = repository.Add(item);
            dbContext.SaveChanges();
            var response = Request.CreateResponse(HttpStatusCode.Created, item);
            return response;
        }

        [HttpPut]
        //[ActionName("Put")]
        [Route("Put")]
        public void Put(int id, [FromBody] TDto dto)
        {
            if (!repository.Update(dto))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        //[ActionName("Delete")]
        [Route("Delete/{id}")]
        public void Delete([FromBody] string id)
        {
            repository.Remove(id);
        }
    }
}