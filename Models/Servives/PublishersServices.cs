using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EBookStore.Site.Models.Servives
{
    public class PublishersServices : IPublisher
    {
        private readonly AppDbContext _db;

        public PublishersServices(AppDbContext db)
        {
            _db = db;
        }
        public void CreatePublisher(PublishersDto dto)
        {
            var publisher = new Publisher
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email
            };

            _db.Publishers.Add(publisher);
            _db.SaveChanges();
        }
        public bool IsPublisherNameExists(string name)
        {
            return _db.Publishers.Any(p => p.Name == name);
        }



        public int GetPublishersCount()
        {
            return _db.Publishers.Count();
        }
        public void CreatePublishersFromExcel(IEnumerable<HttpPostedFileBase> excelFiles, string CategoryName)
        {

            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {

                    if (workbook.TryGetWorksheet(CategoryName, out var worksheet))
                    {
                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            var name = row.Cell(2).Value.ToString().Trim();

                            if (IsPublisherNameExists(name))
                            {
                                continue;
                            }

                            var vm = new PublishersVM
                            {
                                Name = name,
                                Address = null,
                                Phone = null,
                                Email = null
                            };

                            CreatePublisher(vm.ToDto());
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