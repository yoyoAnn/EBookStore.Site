namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookImage
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        [StringLength(255)]
        public string Image { get; set; }

        public virtual Book Book { get; set; }
    }
}
