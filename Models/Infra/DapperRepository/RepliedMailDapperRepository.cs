using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using EBookStore.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

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
LEFT JOIN ProblemType as p ON p.Id = cs.ProblemTypeId
ORDER BY CreatedTime DESC";

			IEnumerable<RepliedMailVM> repliedMails = new SqlConnection(_connStr)
				.Query<RepliedMailVM>(sql);

			return repliedMails.ToList();

		}
		public RepliedMailEditVM GetRepliedMailById(int? id)
		{
			string sql = $@"
SELECT DISTINCT r.[Id],r.[CSId],cs.[UserAccount],r.[Email],r.[Title],r.[Content],
Replace(Convert(nvarchar(19),r.[CreatedTime],120),'-','/')　as 'CreatedTime',
cs.[ProblemTypeId],p.Name as 'ProblemTypeName',cs.[ProblemStatement] FROM RepliedMails as r
LEFT JOIN CustomerServiceMails as cs ON cs.Id = r.CSId
LEFT JOIN ProblemType as p ON p.Id = cs.ProblemTypeId
WHERE r.Id = @id";

			RepliedMailEditVM repliedMails = new SqlConnection(_connStr)
				.Query<RepliedMailEditVM>(sql, new { Id = id }).FirstOrDefault();

			return repliedMails;

		}
		public Result UpdateRepliedMails(RepliedMailEditVM editVM)
		{
			string sql = $@"
UPDATE RepliedMails
SET Email = @email,
Title = @title,
Content = @content,
CreatedTime = GETDATE()
WHERE Id = @id";

			var editMail = new SqlConnection(_connStr)
				.Query<RepliedMailEditVM>(sql, new {
					Id = editVM.Id,
					Email = editVM.Email,
					Title = editVM.Title,
					Content = editVM.Content
				});

			if(editMail != null)
			{
				return Result.Success();
			}
			return Result.Fail("更新失敗!");
		}
	}
}