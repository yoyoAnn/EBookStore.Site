using DocumentFormat.OpenXml.Spreadsheet;
using EBookStore.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
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
