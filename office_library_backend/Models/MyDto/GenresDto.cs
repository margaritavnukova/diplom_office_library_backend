using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class GenresDto : BaseDto<GenresDto, Genre_Dictionary>
    {
        public String Name { get; set; }
        public String Description { get; set; }

        public GenresDto(Genre_Dictionary genre) : base(genre)
        {
            Name = genre.Name;
            Description = genre.Description;
        }

        public override Genre_Dictionary ConvertToModel(Entities2 context)
        {
            return new Genre_Dictionary
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description
            };
        }

        protected override string GetId(Genre_Dictionary model)
        {
            return model.Id;
        }
    }
}