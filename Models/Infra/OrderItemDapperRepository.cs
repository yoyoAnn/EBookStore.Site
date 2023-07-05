using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.Infra
{
    public class OrderItemDapperRepository
    {

        private readonly string _connStr;
        public OrderItemDapperRepository()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public IEnumerable<DetailDapperVM> GetOrdersItemsByOrderId(long orderId)
        {
            string sql = $@"
SELECT [Books].[Name], [Books].[Price], [OrderItems].[Qty], [Books].[Price] * [OrderItems].[Qty] AS [TotalPrice]
FROM [OrderItems]
LEFT JOIN [dbo].[Books] ON [OrderItems].[BookId] = [Books].[Id]
where OrderId=@OrderId";

            IEnumerable<DetailDapperVM> DetailItems = new SqlConnection(_connStr)
                .Query<DetailDapperVM>(sql, new { OrderId = orderId });

            return DetailItems.ToList();

        }
    }

    public class DetailDapperVM
    {

        [Display(Name = "名稱")]
        public string Name { get; set; }

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "個數")]
        public int Qty { get; set; }

        [Display(Name = "總額")]
        public decimal TotalPrice { get; set; }

    }
}