using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using GCWebUsabilityTheme;

namespace IJPReporting.Account
{
    public partial class ManagePassword : BasePage
    {
        //protected string SuccessMessage
        //{
        //    get;
        //    private set;
        //}

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword(manager))
                {
                    pnlChangePassword.Visible = true;
                }
                else
                {
                    Response.Redirect("~/Account/Manage");
                }

                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    // Form.Action = ResolveUrl("~/Account/Manage");

                    if (message == "PwdExpired")
                    {
                        alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Warning;
                        alert.Title = GetLocalResourceObject("AlertPwdExpiredTitle").ToString();
                        alert.Content = GetLocalResourceObject("AlertPwdExpiredContent").ToString();
                        alert.Visible = true;
                    }
                }


                //else
                //{
                //    setPassword.Visible = true;
                //    changePasswordHolder.Visible = false;
                //}

                // Render success message
                //var message = Request.QueryString["m"];
                //if (message != null)
                //{
                //    // Strip the query string from action
                //    Form.Action = ResolveUrl("");
                //}
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();

                IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), CurrentPasswordLabel.Text, NewPasswordLabel.Text);

                if (result.Succeeded)
                {
                    Models.ApplicationUser user;

                    using (var context = new Models.ApplicationDbContext())
                    {
                        var id = User.Identity.GetUserId();
                        user = context.Users.SingleOrDefault(x => x.Id == id);
                        user.LastPasswordChange = DateTime.Now;
                    }

                    using (var context = new Models.ApplicationDbContext())
                    {
                        context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }

                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                }
                else
                {
                    alert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Warning;
                    alert.Title = GetGlobalResourceObject("Global", "AlertInvalidPasswordTitle").ToString();
                    alert.Content = GetGlobalResourceObject("Global", "AlertInvalidPasswordContent").ToString();
                    alert.Visible = true;
                }
            }
        }

        //protected void SetPassword_Click(object sender, EventArgs e)
        //{
        //    if (IsValid)
        //    {
        //        // Create the local login info and link the local account to the user
        //        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        IdentityResult result = manager.AddPassword(User.Identity.GetUserId(), password.Text);
        //        if (result.Succeeded)
        //        {
        //            Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //        }
        //    }
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}
    }
}