namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }

        public int OrderId { get; set; }

        public int Scores { get; set; }

        [Required]
        [StringLength(200)]
        public string Content { get; set; }

        public virtual Book Book { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }
    }
}
