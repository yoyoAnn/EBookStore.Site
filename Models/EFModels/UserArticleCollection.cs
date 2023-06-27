namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserArticleCollection
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? ArticleId { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual Article Article { get; set; }

        public virtual User User { get; set; }
    }
}
