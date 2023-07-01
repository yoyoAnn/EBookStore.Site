using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class CSMailCriteria
	{
		public string MailStatus; 
		public bool IsRead { get; set; }
		public bool IsReplied { get; set; }
		public int? ProblemTypeId { get; set; }
		public string ProblemStatement { get; set; }
		public string Account { get; set; }
		public DateTime? CreatedTime {get; set; }
	}
}