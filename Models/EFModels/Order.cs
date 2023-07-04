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

        public long Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string ReceiverName { get; set; }

        [Required]
        [StringLength(255)]
        public string ReceiverAddress { get; set; }

        [Required]
        [StringLength(10)]
        public string ReceiverPhone { get; set; }

        [StringLength(8)]
        public string TaxIdNum { get; set; }

        [StringLength(30)]
        public string VehicleNum { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        public DateTime OrderTime { get; set; }

        public int? OrderStatusId { get; set; }

        public decimal TotalAmount { get; set; }

        [StringLength(255)]
        public string ShippingNumber { get; set; }

        public DateTime? ShippingTime { get; set; }

        public decimal ShippingFee { get; set; }

        public int? ShippingStatusId { get; set; }

        public decimal TotalPayment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerServiceMail> CustomerServiceMails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual ShippingStatus ShippingStatus { get; set; }

        public virtual User User { get; set; }
    }
}
