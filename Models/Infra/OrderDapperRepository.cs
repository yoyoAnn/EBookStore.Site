using Dapper;
using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Razor.Tokenizer;
using System.Xml.Linq;

namespace EBookStore.Site.Models.Infra
{
    public class OrderDapperRepository
    {
        private readonly string _connStr;
        public OrderDapperRepository()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public IEnumerable<OrdersItemDapperVM> GetOrdersItemsByAccount(string memberAccount)
        {
            string sql = $@"
SELECT DISTINCT Orders.Id as OrdersId, Orders.*,Users.Account, OrderStatuses.[Name] as OrderStatusesName, ShippingStatuses.[Name] as ShippingStatusesName FROM Orders
LEFT JOIN OrderStatuses ON Orders.OrderStatusId = OrderStatuses.Id
LEFT JOIN ShippingStatuses ON Orders.ShippingStatusId = ShippingStatuses.Id
LEFT JOIN Users ON Orders.UserId = Users.Id
LEFT JOIN OrderItems ON Orders.Id = OrderItems.OrderId";

            IEnumerable<OrdersItemDapperVM> cartItems = new SqlConnection(_connStr)
                .Query<OrdersItemDapperVM>(sql/*, new { Account = memberAccount }*/);

            return cartItems.ToList();

        }
    }

    public class OrdersItemDapperVM
    {
        [Display(Name = "訂單編號")]
        public string Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "收件人姓名")]
        public string ReceiverName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "收件人地址")]
        public string ReceiverAddress { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "收件人電話")]
        public string ReceiverPhone { get; set; }

        [StringLength(8)]
        [Display(Name = "發票編號")]
        public string TaxIdNum { get; set; }

        [StringLength(30)]
        [Display(Name = "載具編號")]
        public string VehicleNum { get; set; }

        [StringLength(50)]
        [Display(Name = "備註")]
        public string Remark { get; set; }

        [Display(Name = "下單時間")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime OrderTime { get; set; }

        [Display(Name = "訂單狀態")]
        public int? OrderStatusId { get; set; }

        [Display(Name = "小計")]
        public decimal TotalAmount { get; set; }
        public string FormattedTotalAmount => Math.Ceiling(TotalAmount).ToString();

        [StringLength(255)]
        [Display(Name = "貨運編號")]
        public string ShippingNumber { get; set; }

        [Display(Name = "到貨時間")]
        public DateTime? ShippingTime { get; set; }

        [Display(Name = "運費")]
        public decimal ShippingFee { get; set; }
        public string FormattedShippingFee => Math.Ceiling(ShippingFee).ToString();

        [Display(Name = "送貨狀態")]
        public int? ShippingStatusId { get; set; }

        [Display(Name = "總額")]
        public decimal TotalPayment { get; set; }
        public string FormattedTotalPayment => Math.Ceiling(TotalPayment).ToString();


        public long OrderId { get; set; }

        public int BookId { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public bool? Gender { get; set; }

        [Required]
        [StringLength(255)]
        public string Photo { get; set; }


        public DateTime CreatedTime { get; set; }

        public bool IsConfirmed { get; set; }

        [StringLength(50)]
        public string ConfirmCode { get; set; }

        [Display(Name = "訂單編號")]
        public string OrdersId { get; set; }

        [Display(Name = "訂單狀態")]
        public string OrderStatusesName { get; set; }

        [Display(Name = "送貨狀態")]
        public string ShippingStatusesName { get; set; }

    }
}