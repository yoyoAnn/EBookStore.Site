using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class NewsCriteria
	{
		public string Title { get; set; }

		public bool? Status { get; set; }

		

		public DateTime? StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; }

	}
}