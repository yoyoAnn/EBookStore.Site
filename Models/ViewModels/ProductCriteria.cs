using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewModels
{
    public class ProductCriteria
    {
        [Display(Name = "關鍵字搜尋")]
        public string Name { get; set; }

        [Display(Name = "訂單狀態")]
        public int? OrderId { get; set; }

        [Display(Name = "日期:(起)")]
        public DateTime? Date_Start { get; set; }

        [Display(Name = "日期:(迄)")]
        public DateTime? Date_End { get; set; }
    }
}