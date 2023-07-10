using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class RepliedMailVM
	{
		public int Id { get; set; }

		public int CSId { get; set; }

		[Display(Name = "使用者")]
		[Required]
		[StringLength(255)]
		public string UserAccount { get; set; }
		public int? ProblemTypeId { get; set; }

		[Display(Name = "收件者")]
		[Required(ErrorMessage = "{0} 必填")]
		[StringLength(50)]
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

		[Display(Name = "回覆時間")]
		public DateTime CreatedTime { get; set; }
	}
	public class RepliedMailCriteria
	{

		public int? ProblemTypeId { get; set; }
		public string ProblemTypeName { get; set; }
		public DateTime? CreatedTime { get; set; }
	}
}