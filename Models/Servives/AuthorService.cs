using ClosedXML.Excel;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Servives
{
    public class AuthorService
    {
        private readonly AppDbContext _db;
        public AuthorService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 確認資料庫裡面偶沒有相同作者名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsAuthorNameExists(string name)
        {
            return _db.Authors.Any(p => p.Name == name);
        }

        /// <summary>
        /// 確認作者數量
        /// </summary>
        /// <returns></returns>
        public int GetAuthorsCount()
        {
            return _db.Authors.Count();
        }


        public void CreateAuthorsFromExcel(IEnumerable<HttpPostedFileBase> excelFiles, string CategoryName)
        {

            var num = BookHelper.GetWorksheetNumber(CategoryName);
            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    var worksheet = workbook.Worksheet(num);

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var name = row.Cell(4).Value.ToString();

                        if (IsAuthorNameExists(name))
                        {
                            continue;
                        }

                        var dto = new AuthorDto
                        {
                            Name = name,
                            Photo = null,
                            Profile = null,
                        };

                        _db.Authors.Add(dto.ToEntity());
                        _db.SaveChanges();
                       
                    }
                }
            }
        }
    }
}