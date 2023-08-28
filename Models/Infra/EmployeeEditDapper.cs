using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace EBookStore.Site.Models.Infra
{
    public class EmployeeEditDapper
    {

        private readonly string _connStr;
        public EmployeeEditDapper()
        {
            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }

        public void EmployeeDapper(int RoleId, int Id)
        {
            string sql = @"
                                update [dbo].[Employees]
                                set
                                RoleId=@RoleId
                                where
                                [Id]=@Id";

            using (var connection = new SqlConnection(_connStr))
            {
                connection.Execute(sql, new { RoleId = RoleId, Id = Id });
            }

            //using (var connection = new SqlConnection(_connStr))
            //{
            //    // 執行更新後，您可以再次查詢相關資料並回傳
            //    string selectSql = "SELECT * FROM Orders WHERE Id = @Id";
            //    IEnumerable<OrdersEditItemDapperVM> cartItems = connection.Query<OrdersEditItemDapperVM>(selectSql, new { Id = OrderId });

            //    return cartItems.ToList();
            //}
        }
    }

    public class EmployeeDapperVM
    {
        public int Id { get; set; }

        public string RoleId { get; set; }

        [Display(Name = "帳號")]
        [StringLength(255)]
        public string Account { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "性別")]
        public bool Gender { get; set; }

        [Display(Name = "手機")]
        [StringLength(10)]
        public string Phone { get; set; }

        [Display(Name = "創建時間")]
        public DateTime CreatedTime { get; set; }

    }
}