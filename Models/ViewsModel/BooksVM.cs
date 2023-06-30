using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewsModel
{
    public class BooksVM
    {
        public int Id { get; set; }

        [Display(Name = "書名")]
        [Required]
        public string Name { get; set; }

        public DateTime? PublishDate { get; set; }

        public string? Summary { get; set; }

        [Display(Name = "ISBN")]
        [Required]
        public string ISBN { get; set; }
        [Display(Name = "EISBN")]
        [Required]
        public string EISBN { get; set; }

        public int? Stock { get; set; }

        [Display(Name = "價格")]
        [Required]
        public decimal Price{ get; set; }

        public float? Discount { get; set; }
    }
}