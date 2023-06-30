using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewsModel
{
    public class BooksVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime PublishDate { get; set; }

        public string Summary { get; set; }

        public string ISBN { get; set; }

        public string EISBN { get; set; }

        public int Stock { get; set; }

        public decimal Price{ get; set; }

        public float Discount { get; set; }
    }
}