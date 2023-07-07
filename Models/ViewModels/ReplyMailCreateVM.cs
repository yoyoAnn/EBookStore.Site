using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class ReplyMailCreateVM
	{
		public int Id { get; set; }

		public int CSId { get; set; }
		public string Account { get; set; }
		public int ProblemTypeId { get; set; }

		[Display(Name = "收件者")]
		[Required(ErrorMessage = "{0} 必填")]
		[StringLength(255)]
		[EmailAddress(ErrorMessage = "Email 格式有誤")]
		public string Email { get; set; }

		[Display(Name = "主旨")]
		[Required(ErrorMessage = "{0} 必填")]
		[StringLength(20, ErrorMessage = "字數不能超過{1}")]
		public string Title { get; set; }

		[Display(Name = "內容")]
		[Required(ErrorMessage = "{0} 必填")]
		[StringLength(500, ErrorMessage = "字數不能超過{1}")]
		public string Content { get; set; }
		public DateTime CreatedTime { get; set; }
	}
}