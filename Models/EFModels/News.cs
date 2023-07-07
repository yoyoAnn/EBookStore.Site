namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
		public string Content { get; set; }

        public int PageViews { get; set; }

        public bool Status { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
