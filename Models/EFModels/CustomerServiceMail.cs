namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerServiceMail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerServiceMail()
        {
            RepliedMails = new HashSet<RepliedMail>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string UserAccount { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public int ProblemTypeId { get; set; }

        [Required]
        [StringLength(500)]
        public string ProblemStatement { get; set; }

        public string OrderId { get; set; }

        public bool IsRead { get; set; }

        public bool IsReplied { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual Order Order { get; set; }

        public virtual ProblemType ProblemType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RepliedMail> RepliedMails { get; set; }
    }
}
