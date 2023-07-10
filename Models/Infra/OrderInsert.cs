using Dapper;
using EBookStore.Site.Models.EFModels;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace EBookStore.Site.Models.Infra
{
    public class OrderInsert
    {
        private readonly string _connStr;
        public OrderInsert()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public IEnumerable<OrdersEditItemDapperVM> OrdersInsert(Order orders)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
 SET IDENTITY_INSERT [dbo].[Orders] ON 
 insert into Orders(Id,UserId,ReceiverName,ReceiverAddress,ReceiverPhone,TaxIdNum,VehicleNum,Remark,OrderTime,OrderStatusId,TotalAmount,
 ShippingNumber,ShippingTime,ShippingFee,ShippingStatusId,TotalPayment)
 values
  (@Id,@UserId,@ReceiverName,@ReceiverAddress,@ReceiverPhone,@TaxIdNum,@VehicleNum,@Remark,@OrderTime,@OrderStatusId,@TotalAmount,@ShippingNumber,
  @ShippingTime,@ShippingFee,@ShippingStatusId,@TotalPayment)

 SET IDENTITY_INSERT [dbo].[Orders] OFF");

            param.Add("Id", orders.Id);
            param.Add("UserId", orders.UserId);
            param.Add("ReceiverName", orders.ReceiverName);
            param.Add("ReceiverAddress", orders.ReceiverAddress);
            param.Add("ReceiverPhone", orders.ReceiverPhone);
            param.Add("TaxIdNum", orders.TaxIdNum);
            param.Add("VehicleNum", orders.VehicleNum);
            param.Add("Remark", orders.Remark);
            param.Add("OrderTime", orders.OrderTime);
            param.Add("OrderStatusId", orders.OrderStatusId);
            param.Add("TotalAmount", orders.TotalAmount);
            param.Add("ShippingNumber", orders.ShippingNumber);
            param.Add("ShippingTime", orders.ShippingTime);
            param.Add("ShippingFee", orders.ShippingFee);
            param.Add("ShippingStatusId", orders.ShippingStatusId);
            param.Add("TotalPayment", orders.TotalPayment);

            using (var connection = new SqlConnection(_connStr))
            {
                connection.Open();
                using (var txn = connection.BeginTransaction())
                {
                    connection.ExecuteScalar<OrdersItemDapperVM>(sql.ToString(), param, txn);
                    txn.Commit();
                }
            }
            using (var connection = new SqlConnection(_connStr))
            {
                // 執行更新後，您可以再次查詢相關資料並回傳
                string selectSql = "SELECT * FROM Orders WHERE Id = @Id";
                IEnumerable<OrdersEditItemDapperVM> cartItems = connection.Query<OrdersEditItemDapperVM>(selectSql, new { Id = orders.Id });

                return cartItems.ToList();
            }
        }


    }
}