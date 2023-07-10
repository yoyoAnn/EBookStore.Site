using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
    public class EditProfileVM
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(255)]
        //public string Password { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Email 格式有誤")]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "手機")]
        [StringLength(10)]
        public string Phone { get; set; }
    }
}