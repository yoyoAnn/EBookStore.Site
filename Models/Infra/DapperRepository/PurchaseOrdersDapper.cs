using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EBookStore.Site.Models.BooksViewsModel;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
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

        public PurchaseOrdersDapper(AppDbContext db)
        {
            _db = db;
            string connStr = "data source=.;initial catalog=EBookStore;user id=ebookLogin;password=123;MultipleActiveResultSets=True;App=EntityFramework\" providerName=\"System.Data.SqlClient";

            _connection = new SqlConnection(connStr);
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


        public void ConfirmOrder(PurchaseOrderDapperVM vm)
        {
            string sql = @"
            UPDATE Books
            SET Stock = Stock + @Qty
            WHERE Id = @BookId;
        ";

            _connection.Execute(sql, new { BookId = vm.BookId, Qty = vm.Qty });

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

    }
}