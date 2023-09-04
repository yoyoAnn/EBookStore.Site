using DocumentFormat.OpenXml.Spreadsheet;
using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;


namespace EBookStore.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {

        System.Threading.Timer sqlOrderTimer;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            System.Threading.AutoResetEvent areRoutine = new System.Threading.AutoResetEvent(false);
            System.Threading.TimerCallback tcb = WriteSql;

            //系統啟動1分鐘後執行，每30秒執行一次
            sqlOrderTimer = new System.Threading.Timer(tcb, areRoutine, 30000, 30000);

        }

        private string _connStr;
        private void WriteSql(object state)
        {

            _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;

            string filePath = @"D:\FUEN\Temp\orderReport.txt";
            string timeLogPath = Path.Combine(Path.GetDirectoryName(filePath), "timelog.txt");

            using (StreamWriter sw = new StreamWriter(timeLogPath, true))
            {
                sw.WriteLine(DateTime.Now.ToString() + " 排程啟動");
            }

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                string sql = @"
                        DECLARE @UpdatedOrders TABLE (OrderId NVARCHAR(20));

                        UPDATE [dbo].[Orders]
                        SET OrderStatusId = 3
                        OUTPUT inserted.Id INTO @UpdatedOrders
                        WHERE OrderStatusId = 1 AND DATEDIFF(SECOND, OrderTime, GETDATE()) > 30;

                        UPDATE Books
                        SET Books.Stock = Books.Stock + OrderItems.Qty
                        FROM [dbo].[Books]
                        JOIN [dbo].[OrderItems] ON Books.Id = OrderItems.BookId
                        JOIN @UpdatedOrders uo ON OrderItems.OrderId = uo.OrderId;

                        SELECT Id, ' 訂單已取消' as Status FROM [dbo].[Orders] WHERE OrderStatusId = 3 AND Id IN (SELECT OrderId FROM @UpdatedOrders);

                        UPDATE [dbo].[Orders]
                        SET OrderStatusId = 2
                        OUTPUT inserted.Id, ' 訂單已完成' as Status
                        WHERE OrderStatusId = 6 AND ShippingStatusId = 5 AND DATEDIFF(SECOND, OrderTime, GETDATE()) > 30";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            while (reader.Read())
                            {
                                sw.WriteLine(reader["Id"].ToString() + reader["Status"].ToString());
                            }
     
                            reader.NextResult();

                            while (reader.Read())
                            {
                                sw.WriteLine(reader["Id"].ToString() + reader["Status"].ToString());
                            }
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(timeLogPath, true))
            {
                sw.WriteLine(DateTime.Now.ToString() + " 排程完成");
            }
        }

        protected void Application_AuthenticateRequest()
        {
            if (!Request.IsAuthenticated)
            {
                return;
            }

            var id = (FormsIdentity)User.Identity;

            FormsAuthenticationTicket ticket = id.Ticket;

            string roles = ticket.UserData;

            string[] arrRoles = roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            IPrincipal currentUser = new GenericPrincipal(User.Identity, arrRoles);

            Context.User = currentUser;

        }

    }
}
