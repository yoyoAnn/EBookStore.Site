namespace EBookStore.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(255)]
        public string Account { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool Gender { get; set; }

        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual Role Role { get; set; }
    }
}
