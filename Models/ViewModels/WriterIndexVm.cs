using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class WriterIndexVm
	{
		public int Id { get; set; }

		[Display(Name = "照片")]
		public string Photo { get; set; }

		[Display(Name = "姓名")]
		public string Name { get; set; }
		[Display(Name = "簡介")]
		public string Profile { get; set; }

		[Display(Name = "簡介")]
		public string ProfileText
		{
			get
			{
				return
				this.Profile.Length >= 15 ? Profile.Substring(0, 15) + "..." : this.Profile;

			}

		}


		[Display(Name="信箱")]
		public string Email { get; set; }


	}
}