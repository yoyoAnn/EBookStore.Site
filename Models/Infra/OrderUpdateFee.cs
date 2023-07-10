using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra
{
    public class OrderUpdateFee
    {
        private readonly string _connStr;
        public OrderUpdateFee()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public IEnumerable<OrdersEditItemDapperVM> OrderUpdatePayment(long OrderId, decimal TotalAmount)
        {
            string sql = @"
                            UPDATE Orders
                            SET 
                            TotalAmount = @TotalAmount,
                            TotalPayment = @TotalAmount + ShippingFee
                            WHERE 
                            Id = @Id";

            using (var connection = new SqlConnection(_connStr))
            {
                connection.Execute(sql, new { Id = OrderId, TotalAmount = TotalAmount });
            }

            using (var connection = new SqlConnection(_connStr))
            {
                // 執行更新後，您可以再次查詢相關資料並回傳
                string selectSql = "SELECT * FROM Orders WHERE Id = @Id";
                IEnumerable<OrdersEditItemDapperVM> cartItems = connection.Query<OrdersEditItemDapperVM>(selectSql, new { Id = OrderId });

                return cartItems.ToList();
            }
        }


    }
}