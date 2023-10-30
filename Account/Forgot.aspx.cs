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
    public partial class ForgotPassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btForgot_Click(object sender, EventArgs e)
        {
            if (Email.IsValid)
            {
                // Validate the user's email address
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = manager.FindByName(Email.Text);

                if (user == null)
                {
                    alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Warning;
                    alert.Title = GetLocalResourceObject("AccountNotExistTitle").ToString();
                    alert.Content = GetLocalResourceObject("AccountNotExistContent").ToString();
                    alert.Visible = true;
                }
                else if (manager.IsEmailConfirmed(user.Id))
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send email with the code and the redirect to reset password page
                    string code = manager.GeneratePasswordResetToken(user.Id);
                    string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);

                    manager.SendEmail(user.Id, GetGlobalResourceObject("Email", "ResetPasswordSubject").ToString(), string.Format(GetGlobalResourceObject("Email", "ResetPasswordContent").ToString(), callbackUrl));

                    pnlForgot.Visible = false;

                    alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Info;
                    alert.Title = GetLocalResourceObject("EmailResetInfoTitle").ToString();
                    alert.Content = GetLocalResourceObject("EmailResetInfoContent").ToString();
                    alert.Visible = true;
                }
                else
                {
                    alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Warning;
                    alert.Title = GetLocalResourceObject("EmailNotConfirmedTitle").ToString();
                    alert.Content = GetLocalResourceObject("EmailNotConfirmedContent").ToString();
                    alert.Visible = true;
                    resendConfirmation.Visible = true;
                }
            }
            else
            {
                alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Warning;
                alert.Title = GetLocalResourceObject("EmailNotValidTitle").ToString();
                alert.Content = GetLocalResourceObject("EmailNotValidContent").ToString();
                alert.Visible = true;
            }
        }

        protected void resendConfirmation_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindByName(Email.Text);
            string code = manager.GenerateEmailConfirmationToken(user.Id);

            string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
            manager.SendEmail(user.Id, GetGlobalResourceObject("Email", "ConfirmAccountSubject").ToString(), string.Format(GetGlobalResourceObject("Email", "ConfirmAccountContent").ToString(), callbackUrl));
        }
    }
}