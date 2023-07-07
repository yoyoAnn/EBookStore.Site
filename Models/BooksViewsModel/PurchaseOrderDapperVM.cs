using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EBookStore.Site.Models.BooksViewsModel
{
    public class PurchaseOrderDapperVM
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        [Display(Name = "書名")]
        public string BookName{ get; set; }
        

        public int PublisherId { get; set; }

        [Required]
        [Display(Name = "出版商名稱")]
        public string PublisherName { get; set; }

        [Display(Name = "創建日期")]
        public DateTime CreateTime { get; set; }

        public string CreateTimetxt => CreateTime.ToString("yyyy/MM/dd");

        [Display(Name = "進貨數量")]
        [Range(1, double.MaxValue, ErrorMessage = "進貨價格必須大於0")]
        public int Qty { get; set; }

        [Display(Name = "進貨明細")]
        public string Detail { get; set; }

        [Required]
        [Display(Name = "進貨價格")]
        [Range(1, double.MaxValue, ErrorMessage = "進貨價格必須大於0")]
        public Decimal PurchasePrice { get; set; }
    }
}