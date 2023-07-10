﻿using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
    public class ForgetPasswordVM
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0} 必填")]
        [StringLength(30)]
        public string Account { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} 必填")]
        [StringLength(256)]
        public string Email { get; set; }
    }
}