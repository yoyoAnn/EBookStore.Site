using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class ArticleCreateVm
	{

		public int Id { get; set; }

		[Display(Name = "書本")]
		
		public int BookId { get; set; }



		[Display(Name ="專欄作家")]
		
		public int WriterId { get; set; }

		[Display(Name ="專欄標題")]
	
		public string Title { get; set; }

		[Display(Name="內容")]
		
		public string Content { get; set; }

		[Display(Name ="瀏覽量")]
		//public int PageViews { get; set; }

		//[Display(Name ="狀態")]
		//已發佈,未發佈
		public bool Status { get; set; }

		[Display(Name ="日期")]
		public DateTime CreatedTime { get; set; }


	
	}
}