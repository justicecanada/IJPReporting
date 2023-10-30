using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using IJPReporting.Models;
using GCWebUsabilityTheme;

namespace IJPReporting.Account
{
    public partial class Manage : BasePage
    {
        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                LoadDataAccount();
            }
        }

        private void LoadDataAccount()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // récupérer l'usager
            ApplicationUser user = manager.FindById(User.Identity.GetUserId());

            ApplicationDbContext context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            IdentityRole role = roleMgr.FindById(user.Roles.First().RoleId);


            // jours restant avant changement de mot de passe
            string joursRestant = string.Format(GetLocalResourceObject("joursRestant").ToString(), (user.LastPasswordChange.Date.AddDays(60) - DateTime.Now.Date).Days);

            // data
            lblUserName.Text = string.Format("{0} {1}", user.FirstName, user.LastName);
            lblAccount.Text = string.Format(GetLocalResourceObject("Account").ToString(), role.Name);
            lblTelephone.Text = user.Telephone;
            lblEmail.Text = user.Email;
            lblLastPasswordChange.Text = string.Format("{0} <span class=\"text-danger small mrgn-lft-md\">{1}</span>", user.LastPasswordChange.ToLongDateString(), joursRestant);

            // render success message
            var message = Request.QueryString["m"];
            if (message != null)
            {
                if (message == "ChangePwdSuccess")
                {
                    wetAlertAccountSetting.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Success;
                    wetAlertAccountSetting.Title = GetLocalResourceObject("AlertSuccessTitle").ToString();
                    wetAlertAccountSetting.Content = GetLocalResourceObject("AlertPasswordChangedSuccessContent").ToString();
                    wetAlertAccountSetting.Visible = true;
                }
                else if (message == "OrderSuccess")
                {
                    wetAlertAccountSetting.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Success;
                    wetAlertAccountSetting.Title = GetLocalResourceObject("AlertOrderSuccessTitle").ToString();
                    wetAlertAccountSetting.Content = GetLocalResourceObject("AlertOrderSuccessContent").ToString();
                    wetAlertAccountSetting.Visible = true;
                }
            }
        }

        protected void btnEditAccount_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            // récupérer l'usager
            ApplicationUser user = manager.FindById(User.Identity.GetUserId());

            txtTelephone.Text = user.Telephone;
            txtEmail.Text = user.Email;

            pnlAccountEdit.Visible = true;
            pnlAccountView.Visible = false;

        }

        protected void btnCancelAccount_Click(object sender, EventArgs e)
        {
            pnlAccountEdit.Visible = false;
            pnlAccountView.Visible = true;
        }


        protected void btnSaveAccount_Click(object sender, EventArgs e)
        {
            if (txtEmail.IsValid && txtTelephone.IsValid)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                // récupérer l'usager
                ApplicationUser user = manager.FindById(User.Identity.GetUserId());

                user.Email = txtEmail.Text;
                user.Telephone = txtTelephone.Text;

                var result = manager.Update(user);

                if (result.Succeeded)
                {
                    // mise à jour des informations du compte
                    lblTelephone.Text = user.Telephone;
                    lblEmail.Text = user.Email;

                    wetAlertAccountSetting.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Success;
                    wetAlertAccountSetting.Title = GetLocalResourceObject("wetAlertSuccessTitle").ToString();
                    wetAlertAccountSetting.Content = GetLocalResourceObject("wetAlertSuccessAccountContent").ToString();

                    pnlAccountEdit.Visible = false;
                    pnlAccountView.Visible = true;
                }
                else
                {
                    wetAlertAccountSetting.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Danger;
                    wetAlertAccountSetting.Title = GetLocalResourceObject("wetAlertFailTitle").ToString();
                    wetAlertAccountSetting.Content = GetLocalResourceObject("wetAlertFailContent").ToString();
                }

                wetAlertAccountSetting.Visible = true;
            }
        } 
    }
}