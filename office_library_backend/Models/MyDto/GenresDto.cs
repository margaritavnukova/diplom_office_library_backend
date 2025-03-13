using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class GenresDto : BaseDto<GenresDto, Genre_Dictionary>
    {
        String Name { get; set; }
        String Description { get; set; }

        public GenresDto(Genre_Dictionary genre) : base(genre)
        {
            Name = this.Name;
            Description = this.Description;
        }

        public override Genre_Dictionary ConvertToModel(Entities1 context)
        {
            var genre = new Genre_Dictionary
            {
                Id = Convert.ToInt32(this.Id),
                Name = this.Name,
                Description = this.Description
            };

            return genre;
        }

        protected override string GetId(Genre_Dictionary model)
        {
            return model.Id.ToString();
        }
    }
}