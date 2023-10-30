using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IJPReporting.Logic;
using System.Web.UI;
using IJPReporting.Account;

namespace IJPReporting
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create the custom role and user.
            //RoleActions roleActions = new RoleActions();
            //roleActions.AddUserAndRole();
        }

        void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                // get user
                var user = Context.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                // has their password expired?


                if (user != null
                    && user.LastPasswordChange.Date.AddDays(60) < DateTime.Now.Date
                    && !Request.Path.EndsWith("ManagePassword.aspx") && !Request.Path.EndsWith("ManagePassword")
                    && !Request.Path.Contains("_browserLink/requestData"))
                {
                    Response.Redirect("~/Account/ManagePassword.aspx?m=PwdExpired");
                }
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            IJPReporting.ExceptionIJP ex = new ExceptionIJP();
            ex.HandleException(Server.GetLastError().GetBaseException());
        }

        public Control upModal { get; set; }
    }
}