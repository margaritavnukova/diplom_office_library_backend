using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class UsersDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string PhotoBase64 { get; set; }

        public UsersDto() { }

        public UsersDto(AspNetUsers user)
        {
            Id = user.Id;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            UserName = user.UserName;

            var roles = user.AspNetRoles;
            if (roles != null)
                foreach (var role in roles)
                {
                    Role = role.Name;
                }

            

            PhotoBase64 = user.Photo != null
                ? Convert.ToBase64String(user.Photo)
                : null;
        }
    }
}