using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewsModel
{
    public class PublishersVM
    {
        public int Id { get; set; }


        [Display(Name = "出版商名稱")]
        [Required]
        public string Name { get; set; }


        [Display(Name = "地址")]
        public string Address { get; set; } = null;

        [Display(Name = "電話")]
        public string Phone { get; set; } = null;

        [Display(Name = "Email")]
        public string Email { get; set; } = null;

        public HttpPostedFileBase ExcelFile { get; set; }
    }
}