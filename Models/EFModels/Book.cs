namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            Articles = new HashSet<Article>();
            BookAuthors = new HashSet<BookAuthor>();
            BookImages = new HashSet<BookImage>();
            Carts = new HashSet<Cart>();
            Comments = new HashSet<Comment>();
            EBooks = new HashSet<EBook>();
            OrderItems = new HashSet<OrderItem>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "書名")]
        public string Name { get; set; }

        [Display(Name = "書本分類")]
        public int CategoryId { get; set; }

        [Display(Name = "出版商")]
        public int PublisherId { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "出版日期")]
        public DateTime PublishDate { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "內容摘要")]
        public string Summary { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }

        [StringLength(13)]
        public string EISBN { get; set; }

        [Display(Name = "庫存")]
        public int Stock { get; set; }

        [Display(Name = "上架狀況")]
        public bool Status { get; set; }

        [Display(Name = "價格")]
        public decimal Price { get; set; }

        [Display(Name = "折扣")]
        public int? Discount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Article> Articles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookImage> BookImages { get; set; }

        public virtual Category Category { get; set; }

        public virtual Publisher Publisher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EBook> EBooks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
