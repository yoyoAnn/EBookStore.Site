using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra
{
	public class Result
	{
		public bool IsSuccess { get; private set; }
		public bool IsFail { get; private set; }
		public string ErrorMessage { get; private set; }

		public static Result Success() => new Result { IsSuccess = true, IsFail = false, ErrorMessage = null };
		public static Result Fail(string errorMessage) => new Result { IsSuccess = false, IsFail = true, ErrorMessage = errorMessage };
	}
}