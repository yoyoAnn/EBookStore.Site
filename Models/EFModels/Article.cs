namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Article
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Article()
        {
            UserArticleCollections = new HashSet<UserArticleCollection>();
        }

        public int Id { get; set; }

        public int BookId { get; set; }

        public int WriterId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Content { get; set; }

        public int PageViews { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual Book Book { get; set; }

        public virtual Writer Writer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserArticleCollection> UserArticleCollections { get; set; }
    }
}
