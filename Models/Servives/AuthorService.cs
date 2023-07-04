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
        /// 確認作者數量
        /// </summary>
        /// <returns></returns>
        public int GetAuthorsCount()
        {
            return _db.Authors.Count();
        }


        public void CreateAuthorsFromExcel(IEnumerable<HttpPostedFileBase> excelFiles, string CategoryName)
        {

            
            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    //var worksheet = workbook.Worksheet(CategoryName);

                    if(workbook.TryGetWorksheet(CategoryName,out var worksheet))
                    {
                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            var delimiters = new char[] { ',', '、' };
                            var names = row.Cell(4).Value.ToString().Split(delimiters);
                            foreach (var name in names)
                            {
                                var trimmedName = name.Trim();
                                AuthorHelper.AddAuthorIfNotExists(trimmedName);
                            }

                        }
                    }
                    else
                    {
                        throw new Exception($"工作表 '{CategoryName}' 不存在。");
                    }
                    
                }
            }
        }
    }
}