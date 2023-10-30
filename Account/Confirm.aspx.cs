using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using IJPReporting.Models;
using GCWebUsabilityTheme;

namespace IJPReporting.Account
{
    public partial class Confirm : BasePage
    {
        //protected string StatusMessage
        //{
        //    get;
        //    private set;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string code = IdentityHelper.GetCodeFromRequest(Request);
                string userId = IdentityHelper.GetUserIdFromRequest(Request);
                if (code != null && userId != null)
                {
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var result = manager.ConfirmEmail(userId, code);
                    if (result.Succeeded)
                    {
                        wetAlert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Success;
                        wetAlert.Title = GetLocalResourceObject("ConfirmSuccessTitle").ToString();
                        wetAlert.Content = GetLocalResourceObject("ConfirmSuccessContent").ToString();
                        //manager.SetTwoFactorEnabled(userId, true);

                        return;
                    }
                }

                wetAlert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Danger;
                wetAlert.Title = GetGlobalResourceObject("Global", "AlertErrorTitle").ToString();
                wetAlert.Content = GetGlobalResourceObject("Global", "AlertErrorContent").ToString();
            }
        }
    }
}