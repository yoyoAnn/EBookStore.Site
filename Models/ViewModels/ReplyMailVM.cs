using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class ReplyMailVM
	{
		public int Id { get; set; }

		public int CSId { get; set; }

		[Required]
		[StringLength(255)]
		public string Email { get; set; }

		[Required]
		[StringLength(20)]
		public string Title { get; set; }

		[Required]
		[StringLength(500)]
		public string Content { get; set; }
	}
}