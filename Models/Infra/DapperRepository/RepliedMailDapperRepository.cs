using Dapper;
using EBookStore.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EBookStore.Site.Models.Infra.DapperRepository
{
	public class RepliedMailDapperRepository
	{
		private readonly string _connStr;
		public RepliedMailDapperRepository()
		{
			_connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
		}

		public IEnumerable<RepliedMailVM> GetRepliedMails()
		{
			string sql = $@"
SELECT DISTINCT r.[Id],r.[CSId],cs.[UserAccount],r.[Email],r.[Title],r.[Content],
Replace(Convert(nvarchar(19),r.[CreatedTime],120),'-','/')　as 'CreatedTime',
cs.[ProblemTypeId],p.Name,cs.[ProblemStatement] FROM RepliedMails as r
LEFT JOIN CustomerServiceMails as cs ON cs.Id = r.CSId
LEFT JOIN ProblemType as p ON p.Id = cs.ProblemTypeId";

			IEnumerable<RepliedMailVM> repliedMails = new SqlConnection(_connStr)
				.Query<RepliedMailVM>(sql/*, new { Account = memberAccount }*/);

			return repliedMails.ToList();

		}
	}
}