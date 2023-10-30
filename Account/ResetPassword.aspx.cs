using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using IJPReporting.Models;
using GCWebUsabilityTheme;

namespace IJPReporting.Account
{
    public partial class ResetPassword : BasePage
    {
        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            if (code != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = manager.FindByName(Email.Text);
                if (user == null)
                {
                    alert.Visible = true;
                    alert.Title = (Language.ToString().Equals("en")) ? "No user found" : "L'utilisateur n'existe pas";
                    alert.Content = (Language.ToString().Equals("en")) ? "No user with the userID (email) exists in the system. " : "Aucun utilisateur avec ce courriel n'existe.";
                    return;
                }
                var result = manager.ResetPassword(user.Id, code, Password.Text);
                if (result.Succeeded)
                {
                    user.LastPasswordChange = DateTime.Now;
                    manager.Update(user);
                    Response.Redirect("~/Account/ResetPasswordConfirmation");
                    return;
                }

                alert.Visible = true;
                alert.Title = GetGlobalResourceObject("Global", "AlertInvalidPasswordTitle").ToString();
                alert.Content = GetGlobalResourceObject("Global", "AlertInvalidPasswordContent").ToString(); ;

                return;
            }

            alert.Visible = true;
            alert.Title = (Language.ToString().Equals("en")) ? "An error has occured" : "Une erreur s'est produite";


        }
    }
}