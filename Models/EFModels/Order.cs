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

        //[RegularExpression(@"^.*\.(xls|xlsx)$", ErrorMessage = "�п�ܰ��ɦW��.xls��.xlsx�����")]
        //public string XlsFile { get; set; }

        public long Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "����H�m�W")]
        public string ReceiverName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "����H�a�}")]
        public string ReceiverAddress { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "����H�q��")]
        public string ReceiverPhone { get; set; }

        [StringLength(8)]
        [Display(Name = "�o���s��")]
        public string TaxIdNum { get; set; }

        [StringLength(30)]
        [Display(Name = "����s��")]
        public string VehicleNum { get; set; }

        [StringLength(50)]
        [Display(Name = "�Ƶ�")]
        public string Remark { get; set; }

        [Display(Name = "�U��ɶ�")]
        public DateTime OrderTime { get; set; }

        [Display(Name = "�q�檬�A")]
        public int? OrderStatusId { get; set; }

        [Display(Name = "�p�p")]
        public decimal TotalAmount { get; set; }
        public string FormattedTotalAmount => Math.Ceiling(TotalAmount).ToString();

        [StringLength(255)]
        [Display(Name = "�o���s��")]
        public string ShippingNumber { get; set; }

        [Display(Name = "�X�f�ɶ�")]
        public DateTime? ShippingTime { get; set; }

        [Display(Name = "�B�O")]
        public decimal ShippingFee { get; set; }

        [Display(Name = "�e�f���A")]
        public int? ShippingStatusId { get; set; }

        [Display(Name = "�`�B")]
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
