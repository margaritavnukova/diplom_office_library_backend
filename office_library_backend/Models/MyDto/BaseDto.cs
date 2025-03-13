using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public abstract class BaseDto<TDto, TModel> where TDto : class where TModel : class 
    {
        public string Id { get; set; }

        public BaseDto() { }

        public BaseDto(TModel model)
        {
            Id = GetId(model);
        }

        public abstract TModel ConvertToModel(Entities1 context);
        protected abstract string GetId(TModel model);
    }
}