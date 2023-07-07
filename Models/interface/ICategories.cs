﻿using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Site.Models
{
    public interface ICategories
    {
        /// <summary>
        /// 回傳所有分類內容
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetCategories();

        /// <summary>
        /// 刪除一筆分類
        /// </summary>
        /// <param name="id"></param>
        void DeleteCategory(int id);
    }
}
