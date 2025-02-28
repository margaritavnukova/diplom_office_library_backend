using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class UsersDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

        public List<UserBookHistory> History { get; } = new List<UserBookHistory>();
        public List<BooksDto> Books { get; set; }

        public UsersDto() { }

        public UsersDto(AspNetUsers user)
        {
            Id = user.Id;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            UserName = user.UserName;
        }
    }
}