using Dapper;
using EBookStore.Site.Models.BooksViewsModel;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra.DapperRepository
{
    public class BookDapperRepository
    {


        /// <summary>
        /// 取得書籍
        /// </summary>
        /// <returns></returns>
        public List<BooksDapperVM> GetBookItems()
        {
            string connStr = "data source=.;initial catalog=EBookStore;user id=ebookLogin;password=123;MultipleActiveResultSets=True;App=EntityFramework\" providerName=\"System.Data.SqlClient";

            string sql = $@"SELECT C.Name as CategoryName,B.ID as Id,B.Name as Name,P.Name as PublisherName ,A.Name as Author,
                         B.PublishDate as PublishDate,B.ISBN,B.EISBN,B.Price,B.Summary,B.Stock,
                         B.Status FROM Books as B
                         LEFT JOIN BookAuthors as BA ON BA.BookId = B.Id
                         LEFT JOIN Authors as A ON A.Id = BA.AuthorId
                         LEFT JOIN Publishers as P ON P.Id = B.PublisherId
                         LEFT JOIN Categories as C ON C.Id = B.CategoryId";

            IEnumerable<BooksDapperVM> bookitems = new SqlConnection(connStr).Query<BooksDapperVM>(sql);

            return bookitems.ToList();
        }




    }
}

