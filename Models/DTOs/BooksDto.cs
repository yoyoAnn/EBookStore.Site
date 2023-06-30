using EBookStore.Site.Models.ViewsModel;
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

        public DateTime PublishDate { get; set; }

        public string Summary { get; set; }

        public string ISBN { get; set; }

        public string EISBN { get; set; }

        public int? Stock { get; set; }

        public bool Status { get; set; } = true;
        public decimal Price { get; set; }

        public float? Discount { get; set; }
    }

    public static class BookExt
    {
        public static BooksDto ToDto(this BooksVM vm)
        {
            return new BooksDto
            {
                Id = vm.Id,
                Name = vm.Name,
                PublishDate = vm.PublishDate,
                Summary = vm.Summary,
                ISBN = vm.ISBN,
                EISBN = vm.EISBN,
                Stock = vm.Stock,
                Status = vm.Status,
                Price = vm.Price,
                Discount = vm.Discount,
            };
        }
    }
}