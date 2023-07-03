using ClosedXML.Excel;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
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
        

        public int GetWorksheetNumber(string category)
        {
            switch (category)
            {
                case "文學小說":
                    return 1;
                case "商業理財":
                    return 2;
                case "藝術設計":
                    return 3;
                case "人文社科":
                    return 4;
                case "心理勵志":
                    return 5;
                case "宗教命理":
                    return 6;
                case "自然科普":
                    return 7;
                case "醫療保健":
                    return 8;
                case "飲食":
                    return 9;
                case "生活風格":
                    return 10;
                default:
                    throw new ArgumentException("Invalid category");
            }
        }

   

        public IEnumerable<Publisher> GetExistingPublishers()
        {
            var existingPublishers = _db.Publishers.ToList();
            return existingPublishers;
        }

        public int GetPublishersCount()
        {
            return _db.Publishers.Count();
        }
        public void CreatePublishersFromExcel(IEnumerable<HttpPostedFileBase> excelFiles,string CategoryName)
        {

            var num = GetWorksheetNumber(CategoryName);
            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    var worksheet = workbook.Worksheet(num);

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var name = row.Cell(2).Value.ToString();

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
            }
        }
    }
}