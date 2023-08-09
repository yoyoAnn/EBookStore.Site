using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace EBookStore.Site.Models.BooksViewsModel
{
    public class BooksDapperVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "書名")]
        public string Name { get; set; }

        [Display(Name = "書本圖片")]
        public string BookImage { get; set; }

        public int PublisherId { get; set; }

        [Display(Name = "出版商")]
        public string PublisherName { get; set; }

        [Display(Name = "作者")]
        public string Author { get; set; }

        public int CategoryId{ get; set; }

        [Display(Name = "書本分類")]
        public string CategoryName { get; set; }


        [Column(TypeName = "date")]
        [Display(Name = "出版日期")]
        public DateTime PublishDate { get; set ; }

        public string PublishDatetxt => PublishDate.ToString("yyyy/MM/dd");


        [Display(Name = "內容摘要")]
        public string Summary { get; set; }

        [StringLength(13)]
        public string ISBN { get; set; }

        [StringLength(13)]
        public string EISBN { get; set; }


        [Required]
        [Display(Name = "庫存")]
        public int Stock { get; set; }

        [Display(Name = "上架狀況")]
        public bool Status { get; set; } = true;


        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "折扣")]
        public int Discount { get; set; } = 1;

    }
}
