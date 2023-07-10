using ClosedXML.Excel;
using Dapper;
using EBookStore.Site.Models.BooksViewsModel;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Servives;
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
        private readonly AppDbContext _db;

        public BookDapperRepository(AppDbContext db)
        {

            _db = db;
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
                Status = true,
                Price = vm.Price,
                Discount = vm.Discount,
            };

            string sql = @"
            INSERT INTO Books (Name, CategoryId, PublisherId, PublishDate, Summary, ISBN, EISBN, Stock, Status, Price, Discount)
            VALUES (@Name, @CategoryId, @PublisherId, @PublishDate, @Summary, @ISBN, @EISBN, @Stock, @Status, @Price, @Discount);
            SELECT SCOPE_IDENTITY();
        ";

            int bookId = _connection.ExecuteScalar<int>(sql, book);

            return bookId;
        }

        /// <summary>
        /// 創建book author bookauthor關聯
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="authorId"></param>
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
                PublishDate = DateTime.Parse(vm.PublishDatetxt),
                Summary = vm.Summary,
                ISBN = vm.ISBN,
                EISBN = vm.EISBN,
                Stock = vm.Stock,
                Status = vm.Status,
                Price = vm.Price,
                Discount = vm.Discount
            };
        }
        public void UpdateBook(BooksDapperVM vm,int categoryId,int PublisherId)
        {


            var book = new BooksDapperVM
            {
                Id = vm.Id,
                Name = vm.Name,
                CategoryId = categoryId,
                PublisherId = PublisherId,
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



        /// <summary>
        /// 用BookId取得書籍資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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


        public void CreateFromExcel(IEnumerable<HttpPostedFileBase> excelFiles, string CategoryName)
        {
            foreach (var excelFile in excelFiles)
            {
                using (var workbook = new XLWorkbook(excelFile.InputStream))
                {
                    if (workbook.TryGetWorksheet(CategoryName, out var worksheet))
                    {
                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            var name = row.Cell(1).Value.ToString();
                            var publisherName = row.Cell(2).Value.ToString();
                            var publishDate = row.Cell(3).GetDateTime();
                            var authors = row.Cell(4).Value.ToString();
                            var isbn = row.Cell(5).Value.ToString();
                            var price = row.Cell(6).GetValue<decimal>();
                            var summary = row.Cell(7).Value.ToString();
                            int publisherId = GetOrCreatePublisherId(excelFiles,publisherName, CategoryName);
                            int categoryId = GetCategoryIdByName(CategoryName);

                            var bookVm = new BooksDapperVM
                            {
                                Name = name,
                                PublisherName = publisherName,
                                CategoryId = categoryId,
                                PublisherId = publisherId,
                                PublishDate = publishDate,
                                Author = authors,
                                Summary = summary,
                                ISBN = isbn,
                                Price = price,
                                Status = true,
                                Stock = 1,//預設庫存都是1
                                Discount = 1//預設沒折扣
                            };

                            if (BookHelper.IsBooksNameExists(_db, name))
                            {
                                continue; // 如果書名已存在，跳過此本書籍，處理下一本
                            }

                            CreateBookWithAuthor(bookVm);
                        }
                    }

                }
            }
        }


        public int GetBooksCount()
        {
            string query = "SELECT COUNT(*) FROM Books";
            int booksCount = _connection.ExecuteScalar<int>(query);

            return booksCount;
        }


        private int GetOrCreatePublisherId(IEnumerable<HttpPostedFileBase> excelFiles,string publisherName,string CategoryName)
        {
            string sql = "SELECT Id FROM Publishers WHERE Name = @PublisherName";
            int publisherId = _connection.QuerySingleOrDefault<int>(sql, new { PublisherName = publisherName });

            if (publisherId == 0)
            {
                var publisher = new PublishersServices(_db);
                publisher.CreatePublishersFromExcel(excelFiles, CategoryName);
                string insertSql = "INSERT INTO Publishers (Name) VALUES (@PublisherName);";
                publisherId = _connection.ExecuteScalar<int>(insertSql, new { PublisherName = publisherName });
            }

            return publisherId;
        }

        private int GetOrCreatePublisherId(string publisherName)
        {
            string sql = "SELECT Id FROM Publishers WHERE Name = @PublisherName";
            int publisherId = _connection.QuerySingleOrDefault<int>(sql, new { PublisherName = publisherName });

            return publisherId;
        }

        private int GetPublisherIdByName(int Id)
        {
            string sql = "SELECT Name FROM Publishers WHERE Id = @Id";
            int publisherId = _connection.QuerySingleOrDefault<int>(sql, new { PublisherName = Id });

            return publisherId;
        }

        private int GetCategoryIdByName(string categoryName)
        {
            string sql = "SELECT Id FROM Categories WHERE Name = @CategoryName";
            return _connection.QuerySingleOrDefault<int>(sql, new { CategoryName = categoryName });
        }

        private int GetCategoryNameById(int categoryId)
        {
            string sql = "SELECT Name FROM Categories WHERE Id = @Id";
            return _connection.QuerySingleOrDefault<int>(sql, new { Id = categoryId });
        }
    }
}

