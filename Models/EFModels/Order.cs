namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            CustomerServiceMails = new HashSet<CustomerServiceMail>();
            OrderItems = new HashSet<OrderItem>();
        }

        //[RegularExpression(@"^.*\.(xls|xlsx)$", ErrorMessage = "請選擇副檔名為.xls或.xlsx的文件")]
        //public string XlsFile { get; set; }

        public long Id { get; set; }

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
        public DateTime OrderTime { get; set; }

        [Display(Name = "訂單狀態")]
        public int? OrderStatusId { get; set; }

        [Display(Name = "小計")]
        public decimal TotalAmount { get; set; }
        public string FormattedTotalAmount => Math.Ceiling(TotalAmount).ToString();

        [StringLength(255)]
        [Display(Name = "發票編號")]
        public string ShippingNumber { get; set; }

        [Display(Name = "出貨時間")]
        public DateTime? ShippingTime { get; set; }

        [Display(Name = "運費")]
        public decimal ShippingFee { get; set; }

        [Display(Name = "送貨狀態")]
        public int? ShippingStatusId { get; set; }

        [Display(Name = "總額")]
        public decimal TotalPayment { get; set; }
        public string FormattedTotalPayment => Math.Ceiling(TotalPayment).ToString();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerServiceMail> CustomerServiceMails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual ShippingStatus ShippingStatus { get; set; }

        public virtual User User { get; set; }
    }
}
