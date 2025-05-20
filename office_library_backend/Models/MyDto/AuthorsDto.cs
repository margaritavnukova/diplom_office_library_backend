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
        public string LifeTime { get; set; } = "";
        public string Country { get; set; } = "";
        public string PhotoBase64 { get; set; }

        public AuthorsDto() { }

        public AuthorsDto(Author author) : base(author)
        {
            Name = author.Name;
            LifeTime = author.LifeTime;
            Country = author.Country;
            PhotoBase64 = GetPhotoBase64(author);
        }

        public override Author ConvertToModel(Entities2 context)
        {
            // Если Id == -1, генерируем новый GUID
            var newId = this.Id == "-1" ? Guid.NewGuid().ToString() : this.Id;
            
            return new Author
            {
                Id = newId,
                Name = this.Name,
                LifeTime = this.LifeTime,
                Country = this.Country,
                Photo = this.PhotoBase64 != null
                    ? Encoding.UTF8.GetBytes(this.PhotoBase64)
                    : null
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