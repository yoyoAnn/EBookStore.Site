using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewsModel
{
    public class BooksVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "書名")]
        public string Name { get; set; }

        [Display(Name = "書本分類")]
        public int CategoryId { get; set; }

        [Display(Name = "出版商")]
        public int PublisherId { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "出版日期")]
        public DateTime PublishDate { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "內容摘要")]
        public string Summary { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }

        [StringLength(13)]
        public string EISBN { get; set; }

        [Display(Name = "庫存")]
        public int Stock { get; set; }

        [Display(Name = "上架狀況")]
        public bool Status { get; set; }

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "折扣")]
        public int? Discount { get; set; }
    }
}