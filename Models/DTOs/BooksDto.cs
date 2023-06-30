using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.DTOs
{
    public class BooksDto
    {
        public int Id { get; set; }
    
        public string Name { get; set; }

        public DateTime? PublishDate { get; set; }

        public string Summary { get; set; }

        public string ISBN { get; set; }
       
        public string EISBN { get; set; }

        public int? Stock { get; set; }
     
        public decimal Price { get; set; }

        public float? Discount { get; set; }
    }
}