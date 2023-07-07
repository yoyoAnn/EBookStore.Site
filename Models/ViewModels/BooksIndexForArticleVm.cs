using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.ViewModels
{
	public class BooksIndexForArticleVm
	{

		public int Id { get; set; }

	
		[Display(Name = "書名")]
		public string Name { get; set; }

		
		public int CategoryId { get; set; }

		[Display(Name = "書本分類")]
		public string CategoryName { get; set; }

		public int PublisherId { get; set; }

		[Display(Name = "出版商")]
		public string PublisherName { get; set; }

    }
}