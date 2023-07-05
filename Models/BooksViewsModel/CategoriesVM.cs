using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewsModel
{
    public class CategoriesVM
    {
        public int Id { get; set; }


        [Display(Name = "分類名稱")]
        [Required]
        public string Name { get; set; }


        [Display(Name = "DisplayOrder")]
        [Required]
        public int DisplayOrder { get; set; }
    }
}