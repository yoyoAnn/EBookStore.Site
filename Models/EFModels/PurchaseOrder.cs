namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PurchaseOrder
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public decimal PurchasePrice { get; set; }

        public int Qty { get; set; }

        [StringLength(100)]
        public string Detail { get; set; }

        public DateTime CreatedTime { get; set; }

        public int PublisherId { get; set; }

        public virtual Book Book { get; set; }

        public virtual Publisher Publisher { get; set; }
    }
}
