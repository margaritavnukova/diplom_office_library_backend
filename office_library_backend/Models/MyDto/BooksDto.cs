using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace office_library_backend.Models.MyDto
{
    public class BooksDto
    {
        public int Id { get; set; }
        public int Author { get; set; }
        public string Title { get; set; }
        public Nullable<int> Genre { get; set; }
        public Nullable<System.DateTime> Year { get; set; }
    }
}