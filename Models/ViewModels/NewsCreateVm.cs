using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class NewsCreateVm
	{

		public int Id { get; set; }

		[Required]
		[Display(Name ="標題")]
		[StringLength(255)]
		public string Title { get; set; }

		[Required]
		[Display(Name = "內容")]
		
		public string Content { get; set; }



		//public int PageViews { get; set; }

		[Required]
		[Display(Name = "發佈")]
		public bool Status { get; set; }

	
		[Display(Name = "首頁圖")]
		public string Image { get; set; }


		[Required]
		[Display(Name = "發佈日期")]
		public DateTime CreatedTime { get; set; }
	}
}