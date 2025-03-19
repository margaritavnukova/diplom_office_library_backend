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
using System.Data.Entity;

namespace office_library_backend.Controllers
{
    [RoutePrefix("api/{controller}")]
    public abstract class BaseController<TDto, TEntity> : ApiController where TEntity : class where TDto : BaseDto<TDto, TEntity>
    {
        protected Entities2 dbContext = new Entities2();
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
            dbContext.Entry(item).State = EntityState.Detached; // Отсоединяем, если нужно
            item = repository.Add(item, dbContext); // Передаём контекст в метод Add
            dbContext.SaveChanges();
            var response = Request.CreateResponse(HttpStatusCode.Created, item);
            return response;
        }


        [HttpPut]
        //[ActionName("Put")]
        [Route("Put")]
        public void Put([FromBody] TDto dto)
         {
            if (!repository.Update(dto))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        //[ActionName("Delete")]
        [Route("Delete")]
        public void Delete([FromBody] string id)
        {
            if (int.TryParse(id, out int intId))
            {
                repository.Remove(intId);
            }
            else
                repository.Remove(id);
        }
    }
}