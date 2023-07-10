using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewModels
{
	public class NewsEditVm
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "標題")]
		[StringLength(255)]
		public string Title { get; set; }

		[Required]
		[Display(Name = "內容")]

		public string Content { get; set; }



		//public int PageViews { get; set; }

		[Required]
		[Display(Name = "狀態")]
		public bool Status { get; set; }

		public string StatusText
		{
			get
			{
				return this.Status ? "已發佈" : "未發佈";

			}
		}

		[Display(Name = "首頁圖")]
		public string Image { get; set; }


		
		public string ImageText { get; set; }


		[Required]
		[Display(Name = "日期")]
		public DateTime CreatedTime { get; set; }

	}
}