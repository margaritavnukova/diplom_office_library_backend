using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace office_library_backend.Models.MyDto
{
    [XmlRoot("Book", Namespace = "https://localhost:44319")]
    public class BooksDto
    {
        public int Id { get; set; }

        [XmlElement("Author")]
        public int Author { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("Genre")]
        public Nullable<int> Genre { get; set; }

        [XmlElement("Year")]
        public Nullable<System.DateTime> Year { get; set; }

        public List<UsersDto> Readers { get; set; } = new List<UsersDto>();
    }
}