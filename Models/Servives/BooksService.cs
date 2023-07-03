using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CoverPageProps;
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
    public class BooksService : IBooks
    {
        private readonly AppDbContext _db;

        public BooksService(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Book> GetBooks()
        {
            return _db.Books.ToList();
        }

        public IEnumerable<Book> GetBooksByPriceAscending()
        {
            return _db.Books.OrderBy(b => b.Price).ToList();
        }

        public IEnumerable<Book> GetBooksByPriceDescending()
        {
            return _db.Books.OrderByDescending(b => b.Price).ToList();
        }


        public void CreateBook(BooksDto dto)
        {
            var book = new Book
            {
                Id = dto.Id,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId,
                PublishDate = dto.PublishDate,
                Summary = dto.Summary,
                ISBN = dto.ISBN,
                EISBN = dto.ISBN,
                Status = dto.Status,
                Stock = dto.Stock,
                Price = dto.Price,
                Discount = dto.Discount,
            };

            _db.Books.Add(book);
            _db.SaveChanges();
        }


        public void CreateFromExcel(IEnumerable<HttpPostedFileBase> excelFiles, string CategoryName)
        {

            var num = BookHelper.GetWorksheetNumber(CategoryName);
            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    var worksheet = workbook.Worksheet(num);

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var name = row.Cell(1).Value.ToString();
                        var publisherId = row.Cell(2).GetValue<int>();
                        var publishDate = row.Cell(3).Value;
                        //var Authors = row.Cell(4).Value.ToString();
                        var iSBN = row.Cell(5).Value.ToString();
                        var price = row.Cell(6).GetValue<decimal>();
                        var summary = row.Cell(7).Value.ToString();



                        var vm = new BooksVM
                        {
                            Name = name,
                            PublisherId = publisherId,
                            PublishDate = publishDate,
                            ISBN = iSBN,
                            Price = price,
                            Summary = summary
                        };

                        CreateBook(vm.ToDto());
                    }
                }
            }
        }
    }
}