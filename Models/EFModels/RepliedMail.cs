namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RepliedMail
    {
        public int Id { get; set; }

        public int CSId { get; set; }

        [Required]
        [StringLength(500)]
        public string RepliedContent { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual CustomerServiceMail CustomerServiceMail { get; set; }
    }
}
