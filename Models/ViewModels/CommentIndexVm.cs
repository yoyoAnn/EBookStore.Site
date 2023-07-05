using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class CommentIndexVm
	{

		
		public int Id { get; set; }

		
		[Display(Name = "書名")]
		public string BookName { get; set; }

		
		[Display(Name = "使用者帳號")]
		public string UserAccount { get; set; }

		[Display(Name = "書籍類別")]
		public string CategoryName { get; set; }


		[Display(Name = "評分")]
		public int Scores { get; set; }


		
		public string Content { get; set; }

		[Display(Name = "評論")]
		public string ContentText
		{
			get
			{

				return this.Content.Length > 50
					? this.Content.Substring(0, 50) + "..."
					: this.Content;

			}
		}

	}
}