using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.DTOs
{
    public class CategoriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

    }

    public static class CategoriesExts
    {
        public static CategoriesDto ToDto(this CategoriesVM vm)
        {
            return new CategoriesDto
            {
                Id = vm.Id,
                Name = vm.Name,
                DisplayOrder = vm.DisplayOrder
            };
        }
    }
}