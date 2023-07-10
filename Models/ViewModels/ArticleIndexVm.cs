using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class ArticleIndexVm
	{


		public int Id { get; set; }

		[Display(Name = "書本")]
		public string BookName { get; set; }

		[Display(Name = "專欄作家")]
		public string WriterName { get; set; }

		[Display(Name = "專欄標題")]

		public string Title { get; set; }

			
		public string Content { get; set; }

		[Display(Name = "內容")]
		public string ContentText
		{
			get
			{
				return this.Content.Length > 20
					? this.Content.Substring(0, 20) + "..."
					: this.Content;
			}
		}


		[Display(Name = "瀏覽量")]
		public int PageViews { get; set; }

		
		//已發佈,未發佈
		public bool Status { get; set; }

		[Display(Name = "狀態")]
		public string StatusText
		{
			get
			{
				return this.Status ? "已發佈" : "未發佈";

			}
		}

		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "日期")]
		public DateTime CreatedTime { get; set; }

	}
}