using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EBookStore.Site.Models.BooksViewsModel;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Servives;
using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra.DapperRepository
{
    public class PurchaseOrdersDapper : IPurchaseOrders
    {
        private readonly IDbConnection _connection;
        private readonly AppDbContext _db;
        public List<string> NonExistingBooks { get; } = new List<string>();//儲存不存在的書籍名稱

        public PurchaseOrdersDapper(AppDbContext db)
        {
            _db = db;
            string connStr = "data source=.;initial catalog=EBookStore;user id=ebookLogin;password=123;MultipleActiveResultSets=True;App=EntityFramework\" providerName=\"System.Data.SqlClient";

            _connection = new SqlConnection(connStr);
        }


      

        public void CreateFromExcel(IEnumerable<HttpPostedFileBase> excelFiles, string excelSheetName)
        {
  
            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    if (workbook.TryGetWorksheet(excelSheetName, out var worksheet))
                    {
                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            var bookname = row.Cell(1).Value.ToString();
                            var publisherName = row.Cell(2).Value.ToString();
                            var qty = row.Cell(3).GetValue<int>();
                            var detail = row.Cell(4).Value.ToString();
                            var price = row.Cell(5).GetValue<decimal>();

                            var purchaseOrder = new PurchaseOrderDapperVM
                            {
                                BookName = bookname,
                                PublisherName = publisherName,
                                PublisherId = GetOrCreatePublisherId(publisherName),
                                BookId = GetOrCreateBookId(bookname),
                                Qty = qty,
                                Detail = detail,
                                PurchasePrice = price,
                            };

                            if (purchaseOrder.BookId == 0)
                            {
                                NonExistingBooks.Add(bookname);
                                continue;
                            }

                            if (purchaseOrder.PublisherId == 0)
                            {
                                var services = new PublishersServices(_db);
                                var vm = new PublishersVM();
                                vm.Name = publisherName;

                                services.CreatePublisher(vm.ToDto());
                            }
                            Create(purchaseOrder);

                        }
                    }

                }
            }
        }


        public void Create(PurchaseOrderDapperVM vm)
        {
            var purchaseOrder = new PurchaseOrderDapperVM
            {
                BookId = vm.BookId,
                PublisherId = vm.PublisherId,
                Qty = vm.Qty,
                Detail = vm.Detail,
                PurchasePrice = vm.PurchasePrice
            };

            string sql = @"
            INSERT INTO PurchaseOrders (BookId, PublisherId, Qty, Detail, PurchasePrice)
            VALUES (@BookId, @PublisherId, @Qty, @Detail, @PurchasePrice);
        ";

            _connection.Execute(sql, purchaseOrder);
        }

        public void Edit(PurchaseOrderDapperVM vm)
        {
            var purchaseOrder = new PurchaseOrderDapperVM
            {
                BookId = vm.BookId,
                PublisherId = vm.PublisherId,
                Qty = vm.Qty,
                Detail = vm.Detail,
                PurchasePrice = vm.PurchasePrice,
                CreateTime = vm.CreateTime
            };

            string sql = @"Update PurchaseOrders SET Qty = @Qty,Detail = @Detail,PurchasePrice = @PurchasePrice WHERE Id = @Id ";

            _connection.Execute(sql, new { Qty = vm.Qty, Detail = vm.Detail, Id = vm.Id, PurchasePrice = vm.PurchasePrice });
        }



        public int GetOrCreatePublisherId(string publisherName)
        {
            string publisherIdQuery = "SELECT Id FROM Publishers WHERE Name = @PublisherName";
            int publisherId = _connection.QuerySingleOrDefault<int>(publisherIdQuery, new { PublisherName = publisherName });

            return publisherId;
        }


        public int GetOrCreateBookId(string bookName)
        {
            string bookIdQuery = "SELECT Id FROM Books WHERE Name = @BookName";
            int bookId = _connection.QuerySingleOrDefault<int>(bookIdQuery, new { BookName = bookName });

            return bookId;
        }


        public void Delete(int id)
        {
            string sql = @"
            DELETE FROM PurchaseOrders
            WHERE Id = @PurchaseOrderId;
        ";

            _connection.Execute(sql, new { PurchaseOrderId = id });
        }

        public void Delete()
        {
            string sql = @"DELETE FROM PurchaseOrders";

            _connection.Execute(sql);
        }


        public void ConfirmOrder(PurchaseOrderDapperVM vm)
        {
            string sql = @"
            UPDATE Books
            SET Stock = Stock + @Qty
            WHERE Id = @BookId;
        ";

            _connection.Execute(sql, new { BookId = vm.BookId, Qty = vm.Qty });

        }
        /// <summary>
        /// 取得所有訂單明細並全部加入歷史訂單
        /// </summary>
        /// <param name="vmList"></param>
        public void ConfirmOrder(IEnumerable<PurchaseOrderDapperVM> vmList)
        {
            string sql = @"
            UPDATE Books
            SET Stock = Stock + @Qty
            WHERE Id = @BookId;
        ";
            foreach (var vm in vmList)
            {
                _connection.Execute(sql, new { BookId = vm.BookId, Qty = vm.Qty });
                CreateInHistory(vm);
                Delete(vm.Id);
            }
        }

        public void CreateInHistory(PurchaseOrderDapperVM vm)
        {
            var purchaseOrder = new PurchaseOrderDapperVM
            {
                Id = vm.Id,
                BookName = vm.BookName,
                BookId = vm.BookId,
                CreateTime = vm.CreateTime,
                PublisherId = vm.PublisherId,
                PublisherName = vm.PublisherName,
                Qty = vm.Qty,
                Detail = vm.Detail,
                PurchasePrice = vm.PurchasePrice
            };

            string sql = @"
            INSERT INTO PurchaseOrderHistory (BookId, PublisherId, Qty, Detail, PurchasePrice)
            VALUES (@BookId, @PublisherId, @Qty, @Detail, @PurchasePrice);
        ";

            _connection.Execute(sql, purchaseOrder);
        }



        public PurchaseOrderDapperVM GetById(int id)
        {
            string sql = @"SELECT PO.Id as Id,B.Id as BookId,B.Name as BookName,P.Name as PublisherName,
                         P.Id as PublisherId,PO.Qty,PO.Detail as Detail,
                         PO.CreatedTime as CreateTime,PO.PurchasePrice as PurchasePrice
                         FROM PurchaseOrders AS PO
                         left JOIN Books as B ON B.Id = PO.BookId
                         JOIN Publishers as P ON P.Id = PO.PublisherId where PO.Id = @Id";

            return _connection.QuerySingleOrDefault<PurchaseOrderDapperVM>(sql, new { Id = id });
        }

        public List<PurchaseOrderDapperVM> GetAll()
        {
            string sql = @"SELECT PO.Id as Id,B.Id as BookId,B.Name as BookName,P.Name as PublisherName,
                         P.Id as PublisherId,PO.Qty,PO.Detail as Detail,
                         PO.CreatedTime as CreateTime,PO.PurchasePrice as PurchasePrice
                         FROM PurchaseOrders AS PO
                         left JOIN Books as B ON B.Id = PO.BookId
                         JOIN Publishers as P ON P.Id = PO.PublisherId";


            IEnumerable<PurchaseOrderDapperVM> purchaseOrders = _connection.Query<PurchaseOrderDapperVM>(sql);

            return purchaseOrders.ToList();
        }

        public List<PurchaseOrderDapperVM> GetHistory()
        {
            string sql = @"SELECT PO.Id as Id,B.Id as BookId,B.Name as BookName,P.Name as PublisherName,
                         P.Id as PublisherId,PO.Qty,PO.Detail as Detail,
                         PO.CreateTime as CreateTime,PO.PurchasePrice as PurchasePrice
                         FROM PurchaseOrderHistory AS PO
                         left JOIN Books as B ON B.Id = PO.BookId
                         JOIN Publishers as P ON P.Id = PO.PublisherId";


            IEnumerable<PurchaseOrderDapperVM> purchaseOrders = _connection.Query<PurchaseOrderDapperVM>(sql);

            return purchaseOrders.ToList();
        }

        public PurchaseOrderDapperVM GetHistoryById(int id)
        {
            string sql = @"SELECT PO.Id as Id,B.Id as BookId,B.Name as BookName,P.Name as PublisherName,
                         P.Id as PublisherId,PO.Qty,PO.Detail as Detail,
                         PO.CreateTime as CreateTime,PO.PurchasePrice as PurchasePrice
                         FROM PurchaseOrderHistory AS PO
                         left JOIN Books as B ON B.Id = PO.BookId
                         JOIN Publishers as P ON P.Id = PO.PublisherId where PO.Id = @Id";

            return _connection.QuerySingleOrDefault<PurchaseOrderDapperVM>(sql, new { Id = id });
        }

    }
}