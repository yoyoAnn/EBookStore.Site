using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Servives
{
    public class BooksService :IBooks
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
           return _db.Books.OrderBy(b =>b.Price).ToList();
        }

        public IEnumerable<Book> GetBooksByPriceDescending()
        {
            return _db.Books.OrderByDescending(b => b.Price).ToList();
        }
    }
}