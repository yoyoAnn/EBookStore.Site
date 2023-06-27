namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EBookOrder
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int EBookId { get; set; }

        public decimal Payment { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual EBook EBook { get; set; }

        public virtual User User { get; set; }
    }
}
