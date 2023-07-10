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
    public class OrderItemAllDapperRepository
    {

        private readonly string _connStr;
        public OrderItemAllDapperRepository()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public ItemDetailDapperVM GetAllItemByOrderId(long orderId)
        {
            string sql = $@"
SELECT Price * Qty AS [TotalPrice]
FROM [OrderItems]
where OrderId=@OrderId";

            IEnumerable<ItemDetailDapperVM> DetailItems = new SqlConnection(_connStr)
                .Query<ItemDetailDapperVM>(sql, new { OrderId = orderId });

            decimal totalPrice = DetailItems.Sum(item => item.TotalPrice);

            ItemDetailDapperVM result = new ItemDetailDapperVM
            {
                TotalPrice = totalPrice
            };

            return result;

        }
    }

    public class ItemDetailDapperVM
    {
        [Display(Name = "總額")]
        public decimal TotalPrice { get; set; }

    }
}