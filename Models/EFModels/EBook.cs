namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBook
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EBook()
        {
            EBookOrders = new HashSet<EBookOrder>();
        }

        public int Id { get; set; }

        public int BookId { get; set; }

        public decimal Price { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual Book Book { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EBookOrder> EBookOrders { get; set; }
    }
}
