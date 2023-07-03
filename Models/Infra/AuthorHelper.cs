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
            using (var db = new AppDbContext()) // 使用自己的 DbContext
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

        public static bool IsAuthorNameExists(YourDbContext db, string name)
        {
            return db.Authors.Any(p => p.Name == name);
        }
    }
}