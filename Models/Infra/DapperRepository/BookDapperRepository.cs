using Dapper;
using EBookStore.Site.Models.BooksViewsModel;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.ViewsModel;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra.DapperRepository
{
    public class BookDapperRepository
    {
        private readonly IDbConnection _connection;


        public BookDapperRepository()
        {
            string connStr = "data source=.;initial catalog=EBookStore;user id=ebookLogin;password=123;MultipleActiveResultSets=True;App=EntityFramework\" providerName=\"System.Data.SqlClient";

            _connection = new SqlConnection(connStr);
        }

        public void CreateBookWithAuthor(BooksDapperVM vm)
        {

            try
            {
                int authorId = GetOrCreateAuthorId(vm.Author);

                int bookId = CreateBook(vm);

                CreateBookAuthorRelationship(bookId, authorId);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        private int CreateBook(BooksDapperVM vm)
        {
            // 創建 Book 紀錄並獲取其 Id
            var book = new Book
            {
                Name = vm.Name,
                CategoryId = vm.CategoryId,
                PublisherId = vm.PublisherId,
                PublishDate = vm.PublishDate,
                Summary = vm.Summary,
                ISBN = vm.ISBN,
                EISBN = vm.EISBN,
                Stock = vm.Stock,
                Status = vm.Status,
                Price = vm.Price,
                Discount = vm.Discount
            };

            string sql = @"
            INSERT INTO Books (Name, CategoryId, PublisherId, PublishDate, Summary, ISBN, EISBN, Stock, Status, Price, Discount)
            VALUES (@Name, @CategoryId, @PublisherId, @PublishDate, @Summary, @ISBN, @EISBN, @Stock, @Status, @Price, @Discount);
            SELECT SCOPE_IDENTITY();
        ";

            int bookId = _connection.ExecuteScalar<int>(sql, book);

            return bookId;
        }

        public void CreateBookAuthorRelationship(int bookId, int authorId)
        {
            // 在 BookAuthor 表中創建新的關聯紀錄
            string sql = @"
            INSERT INTO BookAuthors (BookId, AuthorId)
            VALUES (@BookId, @AuthorId);
        ";

            _connection.Execute(sql, new { BookId = bookId, AuthorId = authorId });
        }



        public int GetOrCreateAuthorId(string authorName)
        {
            // 檢查作者是否存在於 Author 表中，如果不存在，則在 Author 表中創建新的作者紀錄並獲取其 Id
            string sql = "SELECT Id FROM Authors WHERE Name = @AuthorName";
            int authorId = _connection.QuerySingleOrDefault<int>(sql, new { AuthorName = authorName });

            if (authorId == 0)
            {

                AuthorHelper.AddAuthorIfNotExists(authorName);
 
                authorId = _connection.QuerySingleOrDefault<int>(sql, new { AuthorName = authorName });
            }

            return authorId;
        }


        public void UpdateBook(BooksDapperVM vm, int authorId)
        {
            var book = new Book
            {
                Id = vm.Id,
                Name = vm.Name,
                CategoryId = vm.CategoryId,
                PublisherId = vm.PublisherId,
                PublishDate = vm.PublishDate,
                Summary = vm.Summary,
                ISBN = vm.ISBN,
                EISBN = vm.EISBN,
                Stock = vm.Stock,
                Status = vm.Status,
                Price = vm.Price,
                Discount = vm.Discount
            };
        }
        public void UpdateBook(BooksDapperVM vm)
        {
            var book = new Book
            {
                Id = vm.Id,
                Name = vm.Name,
                CategoryId = vm.CategoryId,
                PublisherId = vm.PublisherId,
                PublishDate = vm.PublishDate,
                Summary = vm.Summary,
                ISBN = vm.ISBN,
                EISBN = vm.EISBN,
                Stock = vm.Stock,
                Status = vm.Status,
                Price = vm.Price,
                Discount = vm.Discount
            };

            string sql = @"
        UPDATE Books
        SET Name = @Name, CategoryId = @CategoryId, PublisherId = @PublisherId, PublishDate = @PublishDate,
            Summary = @Summary, ISBN = @ISBN, EISBN = @EISBN, Stock = @Stock, Status = @Status,
            Price = @Price, Discount = @Discount
        WHERE Id = @Id;
    ";

            _connection.Execute(sql, book);
        }




        public BooksDapperVM GetBookById(int id)
        {
            string sql = $@"SELECT C.Name as CategoryName,B.ID as Id,B.Name as Name,P.Name as PublisherName ,A.Name as Author,
                         B.PublishDate as PublishDate,B.ISBN,B.EISBN,B.Price,B.Summary,B.Stock,
                         B.Status FROM Books as B
                         LEFT JOIN BookAuthors as BA ON BA.BookId = B.Id
                         LEFT JOIN Authors as A ON A.Id = BA.AuthorId
                         LEFT JOIN Publishers as P ON P.Id = B.PublisherId
                         LEFT JOIN Categories as C ON C.Id = B.CategoryId
                         WHERE B.Id = @Id";

            return _connection.QuerySingleOrDefault<BooksDapperVM>(sql, new { Id = id });
        }



        /// <summary>
        /// 取得書籍
        /// </summary>
        /// <returns></returns>
        public List<BooksDapperVM> GetBookItems()
        {

            string sql = $@"SELECT C.Name as CategoryName,B.ID as Id,B.Name as Name,P.Name as PublisherName ,A.Name as Author,
                         B.PublishDate as PublishDate,B.ISBN,B.EISBN,B.Price,B.Summary,B.Stock,
                         B.Status FROM Books as B
                         LEFT JOIN BookAuthors as BA ON BA.BookId = B.Id
                         LEFT JOIN Authors as A ON A.Id = BA.AuthorId
                         LEFT JOIN Publishers as P ON P.Id = B.PublisherId
                         LEFT JOIN Categories as C ON C.Id = B.CategoryId";

            IEnumerable<BooksDapperVM> bookitems = _connection.Query<BooksDapperVM>(sql);

            return bookitems.ToList();
        }




    }
}

