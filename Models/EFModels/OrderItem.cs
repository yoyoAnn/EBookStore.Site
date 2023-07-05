namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderItem
    {
        public int Id { get; set; }

        public long OrderId { get; set; }

        public int BookId { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

        public virtual Book Book { get; set; }

        public virtual Order Order { get; set; }
    }
}
