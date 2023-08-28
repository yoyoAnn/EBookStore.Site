using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
    public class UserIndexVM
    {
        public int Id { get; set; }

        [Display(Name = "帳號")]
        [Required]
        [StringLength(255)]
        public string Account { get; set; }

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

        [Display(Name = "手機")]
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }

        
        public string Address { get; set; }

        [Display(Name = "地址")]
        [StringLength(255)]
        public string AddressText
        {
            get
            {
                if (!string.IsNullOrEmpty(Address)) {
                    return this.Address.Length > 10
                 ? this.Address.Substring(0, 10) + "..."
                 : this.Address;
                }
                else { return ""; }
 
            }
            
        }

        public bool Gender { get; set; }

        [Display(Name = "性別")]
        public string GenderText
        {
            get
            {
                return Gender == false ? "男" : "女";
            }
        }

        [Display(Name = "照片")]
        [Required]
        [StringLength(255)]
        public string Photo { get; set; }

        [Display(Name = "創建時間")]
        public DateTime CreatedTime { get; set; }

        
        public bool IsConfirmed { get; set; }
        [Display(Name = "認證")]
        public string IsConfirmedText
        {
            get
            {
                return IsConfirmed == true ? "是" : "否";
            }
        }
      
        [StringLength(50)]
        public string ConfirmCode { get; set; }
    }
}