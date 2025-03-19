using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class AuthorsDto : BaseDto<AuthorsDto, Author>
    {
        public string Name { get; set; }
        public string LifeTime { get; set; }
        public string Country { get; set; }
        public string PhotoBase64 { get; set; }

        public AuthorsDto(Author author) : base(author)
        {
            Name = author.Name;
            LifeTime = author.LifeTime;
            Country = author.Country;
            PhotoBase64 = GetPhotoBase64(author);
        }

        public override Author ConvertToModel(Entities2 context)
        {
            return new Author
            {
                Id = this.Id,
                Name = this.Name,
                LifeTime = this.LifeTime,
                Country = this.Country,
                Photo = Encoding.UTF8.GetBytes(this.PhotoBase64),
            };
        }

        protected override string GetId(Author model)
        {
            return model.Id;
        }

        protected string GetPhotoBase64(Author model)
        {
            return model.Photo != null ? Convert.ToBase64String(model.Photo) : null;
        }
    }
}