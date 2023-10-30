using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using IJPReporting.Models;
using GCWebUsabilityTheme;
using System.Windows.Forms;

namespace IJPReporting.Account
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                resendConfirmation.Visible = false;
                Page.Form.DefaultButton = this.btn_login.UniqueID;
                RegisterHyperLink.NavigateUrl = "Register";
                // Enable this once you have account confirmation enabled for password reset functionality
                ForgotPasswordHyperLink.NavigateUrl = "Forgot";
                //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
                var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                ApplicationUser user = manager.FindByName(Email.Text);
                if (user != null && !manager.IsEmailConfirmed(user.Id))
                {
                    alert.Visible = true;
                    alert.Title = GetLocalResourceObject("AlertEmailNotConfirmedTitle").ToString();
                    alert.Content = GetLocalResourceObject("AlertEmailNotConfirmedContent").ToString();
                    resendConfirmation.Visible = true;

                    return;
                }


                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, false, shouldLockout: true);
                string rt = Request.QueryString["ReturnUrl"];
                switch (result)
                {
                    case SignInStatus.Success:
                        Session["SHOW_TC"] = true;
                        Response.Redirect(rt != null ? rt : "~/ClientReferralReporting");
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("~/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("~/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                        Request.QueryString["ReturnUrl"],
                                                        false),
                                          true);
                        break;
                    case SignInStatus.Failure:
                        alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Warning;
                        alert.Title = GetGlobalResourceObject("Global", "AlertInvalidLoginTitle").ToString();
                        alert.Content = GetGlobalResourceObject("Global", "AlertInvalidLoginContent").ToString();
                        alert.Visible = true;
                        break;
                    default:
                        alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Danger;
                        alert.Title = GetGlobalResourceObject("Global", "AlertErrorTitle").ToString();
                        alert.Content = GetGlobalResourceObject("Global", "AlertErrorContent").ToString();
                        alert.Visible = true;
                        break;
                }
            }
        }

        protected void resendConfirmation_Click(object sender, EventArgs e)
        {
            try
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindByName(Email.Text);
                string code = manager.GenerateEmailConfirmationToken(user.Id);

                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                manager.SendEmail(user.Id, GetGlobalResourceObject("Email", "ConfirmAccountSubject").ToString(), string.Format(GetGlobalResourceObject("Email", "ConfirmAccountContent").ToString(), callbackUrl));
            }
            catch (Exception ex)
            {
                // error
                alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Danger;
                alert.Title = GetGlobalResourceObject("Global", "AlertSendErrorTitle").ToString();
                alert.Content = GetGlobalResourceObject("Global", "AlertSendErrorContent").ToString();
                alert.Visible = true;

                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

    }
}