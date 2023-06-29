using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace EBookStore.Site.Models.Servives
{
    public class CategoriesServer
    {
        private readonly AppDbContext db;

        public CategoriesServer(AppDbContext dbContext)
        {
            db = dbContext;
        }

        public IEnumerable<Category> GetCategories()
        {
            return db.Categories.ToList();
        }


        private void CheckDuplicateCategory(CategoriesDto dto)
        {
            var checkDisplay = db.Categories.Any(c => c.DisplayOrder == dto.DisplayOrder);
            var checkName = db.Categories.Any(c => c.Name == dto.Name);

            if (checkDisplay && checkName)
            {
                throw new Exception("已有相同的書籍分類與DisplayOrder");

            }
            else if (checkDisplay)
            {
                throw new Exception("已有相同的DisplayOrder");
            }
            else if (checkName)
            {
                throw new Exception("書籍分類名稱已有，不能重複創建");
            }
        }

        public void CreateCategory(CategoriesDto dto)
        {
            //errorMessage = null;

            CheckDuplicateCategory(dto);

            var category = new Category
            {
                Name = dto.Name,
                DisplayOrder = dto.DisplayOrder
            };

            db.Categories.Add(category);
            db.SaveChanges();

        }
    }
}