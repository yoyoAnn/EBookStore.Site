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
    public class DeleteOrderItemDapperRepository
    {

        private readonly string _connStr;
        public DeleteOrderItemDapperRepository()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public IEnumerable<DeleteOrderItemVM> DeleteOrderItem(long orderId)
        {
            string sql = $@"
                            delete [OrderItems]
                            where OrderId=@UserId";

            IEnumerable<DeleteOrderItemVM> DetailItems = new SqlConnection(_connStr)
                .Query<DeleteOrderItemVM>(sql, new { UserId = orderId });

            return DetailItems.ToList();

        }
    }

    public class DeleteOrderItemVM
    {
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "名稱")]
        public long OrderId { get; set; }

        [Display(Name = "價格")]
        public int BookId { get; set; }

        [Display(Name = "個數")]
        public decimal Price { get; set; }

        [Display(Name = "總額")]
        public int Qty { get; set; }

    }
}