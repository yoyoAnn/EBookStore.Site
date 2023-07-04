using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.BooksViewsModel
{
    public class BooksDapperVM
    {
        public int Id { get; set; }

        [Display(Name = "書名")]
        public string Name { get; set; }

        [Display(Name = "出版商")]
        public int PublisherId { get; set; }

        [Display(Name = "作者")]
        public string Author { get; set; }

        [Display(Name = "書本分類")]
        public int CategoryId { get; set; }

        [Display(Name = "出版日期")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "內容摘要")]
        public string Summary { get; set; }

        [StringLength(13)]
        public string ISBN { get; set; }

        [StringLength(13)]
        public string EISBN { get; set; }

        [Display(Name = "庫存")]
        public int Stock { get; set; }

        [Display(Name = "上架狀況")]
        public bool Status { get; set; } = true;

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "折扣")]
        public float? Discount { get; set; } = 1;
    }
}
