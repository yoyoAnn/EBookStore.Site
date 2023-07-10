using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
    public class EmployeeEditVM
    {
        public int Id { get; set; }

        [Display(Name = "職稱")]
        public int RoleId { get; set; }

        [Display(Name = "帳號")]
        [Required]
        [StringLength(255)]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool Gender { get; set; }

        [Display(Name = "性別")]
        public string GenderText
        {
            get
            {
                return Gender == false ? "男" : "女";
            }
        }

        [Display(Name = "手機")]
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        [Display(Name = "創建時間")]
        public DateTime CreatedTime { get; set; }
    }
}