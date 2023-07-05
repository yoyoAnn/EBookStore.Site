using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Site.Models
{
    public interface IBooks
    {
        /// <summary>
        /// 取得所有書籍
        /// </summary>
        /// <returns></returns>
        IEnumerable<Book> GetBooks();

        /// <summary>
        /// 抓取的書籍根據價格由小往大排列
        /// </summary>
        /// <returns></returns>
        IEnumerable<Book> GetBooksByPriceAscending();
        /// <summary>
        /// 抓取的書籍根據價格由大往小排列
        /// </summary>
        /// <returns></returns>
        IEnumerable<Book> GetBooksByPriceDescending();
    }
}
