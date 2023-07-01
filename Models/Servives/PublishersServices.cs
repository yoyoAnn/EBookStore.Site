using ClosedXML.Excel;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        /// <summary>
        /// 創建出版商
        /// </summary>
        /// <param name="vms"></param>
        //public void CreatePublishersFromExcel(IEnumerable<PublishersVM> vms)
        //{

        //    foreach (var vm in vms)
        //    {
        //        if (!IsPublisherNameExists(vm.Name))
        //        {
        //            using (var workbook = new XLWorkbook(vm.ExcelFile.InputStream))
        //            {
        //                var worksheet = workbook.Worksheet(1);//預設第一個工作表

        //                foreach (var row in worksheet.RowsUsed().Skip(1)) // 跳過標題列
        //                {
        //                    var name = row.Cell(4).Value.ToString(); // 預設第四欄為出版商，讀取第四個欄位的值

        //                    // 創建 Publisher 物件並設定 Address 屬性
        //                    var dto = new PublishersDto
        //                    {
        //                        Id = vm.Id,
        //                        Name = name,
        //                        Address = vm.Address,
        //                        Phone = vm.Phone,
        //                        Email = vm.Email
        //                    };

        //                    CreatePublisher(dto);
        //                }
        //            }
        //        }
        //    }
        //}

        public void CreatePublishersFromExcel(IEnumerable<HttpPostedFileBase> excelFiles)
        {
            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    var worksheet = workbook.Worksheet(1);

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var name = row.Cell(4).Value.ToString();

                        var dto = new PublishersDto
                        {
                            Name = name,
                            Address = null, // Set the address, phone, and email accordingly
                            Phone = null,
                            Email = null
                        };

                        CreatePublisher(dto);
                    }
                }
            }
        }
    }
}