using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.ViewModels
{
	public class ArticleEditVm
	{
		public int Id { get; set; }

		[Display(Name = "書本")]
		[Required]
		public int BookId { get; set; }


		[Display(Name = "專欄作家")]
		[Required]
		public int WriterId { get; set; }

		[Display(Name = "專欄標題")]
		[Required]
		[StringLength(255)]
		public string Title { get; set; }

		[Display(Name = "內容")]
		[Required]
		public string Content { get; set; }

		[Display(Name ="瀏覽量")]
		public int PageViews { get; set; }

		[Display(Name = "狀態")]
		//已發佈,未發佈
		public bool Status { get; set; }

		[Display(Name = "日期")]
		public DateTime CreatedTime { get; set; }



	}
}