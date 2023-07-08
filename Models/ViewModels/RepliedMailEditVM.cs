using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class RepliedMailEditVM
	{
		public int Id { get; set; }

		[Display(Name = "客戶信件編號")]
		public int CSId { get; set; }

		[Display(Name = "使用者")]
		public string UserAccount { get; set; }

		[Display(Name = "收件者")]
		[Required(ErrorMessage = "{0} 必填")]
		[StringLength(50)]
		[EmailAddress(ErrorMessage = "Email 格式有誤")]
		public string Email { get; set; }

		[Display(Name = "主旨")]
		[Required(ErrorMessage = "{0} 必填")]
		[StringLength(30, ErrorMessage = "字數不能超過{1}")]
		public string Title { get; set; }

		[Display(Name = "內容")]
		[Required(ErrorMessage = "{0} 必填")]
		[StringLength(500, ErrorMessage = "字數不能超過{1}")]
		public string Content { get; set; }

		[Display(Name = "回覆時間")]
		public DateTime CreatedTime { get; set; }
	}
}