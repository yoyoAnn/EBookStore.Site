using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra
{
    public static class AuthorHelper
    {
        public static void AddAuthorIfNotExists(string name)
        {
            using (var db = new AppDbContext()) 
            {
                if (!IsAuthorNameExists(db, name))
                {
                    var dto = new AuthorDto
                    {
                        Name = name.Trim(),
                        Photo = null,
                        Profile = null
                    };

                    db.Authors.Add(dto.ToEntity());
                    db.SaveChanges();
                }
            }
        }


        /// <summary>
        /// 確認資料庫裡面偶沒有相同作者名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsAuthorNameExists(AppDbContext db, string name)
        {
            return db.Authors.Any(p => p.Name == name);
        }
    }
}