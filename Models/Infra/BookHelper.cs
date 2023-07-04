using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBookStore.Site.Models.Infra
{
    public static class BookHelper
    {
    
        /// <summary>
        /// 獲取分類資料
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetCategories()
        {

            using (var db = new AppDbContext())
            {
                // 從資料庫或其他資料來源獲取分類資料
                var categories = db.Categories.ToList();

                // 將分類資料轉換為選項列表
                var categoryList = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });

                return categoryList;
            }
            
        }
    }
}