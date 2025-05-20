using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public abstract class BaseDto<TDto, TModel> where TDto : class where TModel : class 
    {
        public string Id { get; set; } = "-1"; // Дефолтное значение

        public BaseDto() { }

        public BaseDto(TModel model)
        {
            Id = GetId(model);
        }

        public abstract TModel ConvertToModel(Entities2 context);
        protected abstract string GetId(TModel model);
    }
}