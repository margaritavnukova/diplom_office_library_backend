using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class UsersDto : BaseDto<UsersDto, AspNetUsers>
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string PhotoBase64 { get; set; }

        public UsersDto() { }

        public UsersDto(AspNetUsers user) : base(user)
        {
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            UserName = user.UserName;

            var roles = user.AspNetRoles;
            if (roles != null)
                foreach (var role in roles)
                {
                    Role = role.Name;
                }
        }

        public override AspNetUsers ConvertToModel(Entities1 context)
        {
            var user = new AspNetUsers
            {
                Id = this.Id.ToString(),
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                UserName = this.UserName,
                Photo = this.PhotoBase64 != null ? Convert.FromBase64String(this.PhotoBase64) : null
            };

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            userManager.AddToRole(user.Id, this.Role);

            return user;
        }

        protected override string GetId(AspNetUsers model)
        {
            return model.Id;
        }

        protected string GetPhotoBase64(AspNetUsers model)
        {
            return model.Photo != null ? Convert.ToBase64String(model.Photo) : null;
        }
    }
}