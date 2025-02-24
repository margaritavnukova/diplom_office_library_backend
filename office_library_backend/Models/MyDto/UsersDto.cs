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
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public List<BooksDto> Books { get; set; } = new List<BooksDto>();
    }
}