//using System;
//using System.Configuration;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Net.Mail;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Owin;
//using IJPReporting.Models;
//using GCWebUsabilityTheme;
//using WetControls.Controls;
//using Microsoft.AspNet.Identity.EntityFramework;

//namespace IJPReporting.Account
//{
//    public partial class Register : BasePage
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                Page.Form.DefaultButton = this.btRegister.UniqueID;
//                using (var context = new BD_IJPReportingEntities())
//                {
//                    var roleStore = new RoleStore<IdentityRole>(context);
//                    var roleMngr = new RoleManager<IdentityRole>(roleStore);
//                    var roles = roleMngr.Roles.Where(x => !x.Name.Equals("admin", StringComparison.InvariantCultureIgnoreCase)
//                                                && !x.Name.Equals("superadmin", StringComparison.InvariantCultureIgnoreCase)).ToList();
//                    Roles.DataSource = roles;
//                    Roles.DataTextField = "Name";
//                    Roles.DataValueField = "Id";
//                    Roles.DataBind();

//                    RegionsDDL.DataSource = context.Regions.ToList();
//                    RegionsDDL.DataTextField = "Name";
//                    RegionsDDL.DataValueField = "region_id";
//                    RegionsDDL.DataBind();

//                    RegionsRBL.DataSource = context.Regions.ToList();
//                    RegionsRBL.DataTextField = "Name";
//                    RegionsRBL.DataValueField = "region_id";
//                    RegionsRBL.DataBind();

//                    Communities.DataSource = context.Community.Where(x => !x.IsDeleted).ToList();
//                    Communities.DataTextField = "Name";
//                    Communities.DataValueField = "Community_Id";
//                    Communities.DataBind();

//                    Programs.DataSource = context.Community.Where(x => !x.IsDeleted).ToList();
//                    Programs.DataTextField = "Name";
//                    Programs.DataValueField = "Community_Id";
//                    Programs.DataBind();
//                }
//            }
//        }

//        protected void CreateUser_Click(object sender, EventArgs e)
//        {
//            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
//            var roleManager = Context.GetOwinContext().Get<ApplicationRoleManager>();

//            if (!Email.IsValid)
//            {
//                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                alert.Visible = true;
//                alert.Title = (Language.ToString().Equals("en")) ? "Email is invalid" : "Le courriel n'est pas valide";
//                alert.Content = (Language.ToString().Equals("en")) ? "Please enter a .gc.ca or canada.ca email" : "S'il vous plait, utilisez un courriel .gc.ca ou canada.ca";
//                return;

//            }
//            if (!Telephone.IsValid)
//            {
//                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                alert.Visible = true;
//                alert.Title = (Language.ToString().Equals("en")) ? "Telephone is invalid" : "Le numéro de téléphone n'est pas valide";
//                alert.Content = (Language.ToString().Equals("en")) ? "Please enter a valid North American phone number" : "S'il vous plait, entrez un numéro de téléphone nord-américain valide";
//                return;

//            }
//            if (!FirstName.IsValid)
//            {
//                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                alert.Visible = true;
//                alert.Title = (Language.ToString().Equals("en")) ? "The first name field is mandatory" : "Le champ prénom est obligatoire";
//                alert.Content = (Language.ToString().Equals("en")) ? "Please enter your first name in the field" : "S'il vous plait, inscrivez votre prénom dans le champ";
//                return;

//            }
//            if (!LastName.IsValid)
//            {
//                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                alert.Visible = true;
//                alert.Title = (Language.ToString().Equals("en")) ? "The last name field is mandatory" : "Le champ nom est obligatoire";
//                alert.Content = (Language.ToString().Equals("en")) ? "Please enter your last name in the last name field." : "S'il vous plait, inscrivez votre nom dans le champ";
//                return;

//            }

//            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text, FirstName = FirstName.Text, LastName = LastName.Text, /*RegionId = Convert.ToInt32(Regions.SelectedValue),*/ Telephone = Telephone.Text, LastPasswordChange = DateTime.Now };
//            if (manager.FindByEmail(user.Email) != null)
//            {
//                if (Language.ToString().Equals("fr"))
//                {
//                    alert.Visible = true;
//                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                    alert.Title = "Impossible de créer le compte";
//                    alert.Content = "Le compte existe déjà";
//                }
//                else
//                {
//                    alert.Visible = true;
//                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                    alert.Title = "Unable to create an account";
//                    alert.Content = "The account already exists";
//                }
//                return;
//            }

//            IdentityResult result = manager.Create(user, Password.Text);

//            if (result.Succeeded)
//            {
//                manager.AddToRole(user.Id, "Client");
//                //manager.AddToRole(user, "Client");
//                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
//                string code = manager.GenerateEmailConfirmationToken(user.Id);
//                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);

//                try
//                {
//                    manager.SendEmail(user.Id, GetGlobalResourceObject("Email", "ConfirmAccountSubject").ToString(), string.Format(GetGlobalResourceObject("Email", "ConfirmAccountContent").ToString(), callbackUrl));

//                    //foreach (Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole userole in roleManager.FindByName("Admin").Users)
//                    //{
//                    //    manager.SendEmail(userole.UserId, GetGlobalResourceObject("Email", "AdminConfirmAccountSubject").ToString(), string.Format(GetGlobalResourceObject("Email", "AdminConfirmAccountContent").ToString(), user.FirstName + " " + user.LastName));
//                    //}
//                }
//                catch (Exception ex)
//                {
//                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                    alert.Visible = true;
//                    alert.Title = (ex.ToString());
//                    alert.Content = (ex.ToString());

//                    ExceptionIJP error = new ExceptionIJP();
//                    error.HandleException(ex);

//                    return;
//                }

//                if (user.EmailConfirmed)
//                {
//                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
//                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
//                }
//                else
//                {
//                    if (Language.ToString().Equals("en"))
//                    {
//                        alert.Visible = true;
//                        alert.AlertType = WetAlert.ALERT_TYPE.Warning;
//                        alert.Title = "Email validation";
//                        alert.Content = "An email has been sent to your account. Please view the email and confirm your account to complete the registration process.";

//                    }
//                    else
//                    {
//                        alert.Visible = true;
//                        alert.AlertType = WetAlert.ALERT_TYPE.Warning;
//                        alert.Title = "Validation de courriel";
//                        alert.Content = "Un courriel a été envoyé à votre compte. S'il vous plait, vérifiez le courriel et validez votre compte pour finaliser votre enregistrement.";

//                    }

//                }

//            }
//            else
//            {
//                if (result.Errors.FirstOrDefault().Contains("Password"))
//                {
//                    alert.Visible = true;
//                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
//                    alert.Title = GetGlobalResourceObject("Global", "AlertInvalidPasswordTitle").ToString();
//                    alert.Content = GetGlobalResourceObject("Global", "AlertInvalidPasswordContent").ToString(); ;

//                }


//            }
//        }

//        protected void Roles_SelectedIndexChanged(object sender, EventArgs e)
//        {

//        }
//    }
//}

using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using IJPReporting.Models;
using GCWebUsabilityTheme;
using WetControls.Controls;

namespace IJPReporting.Account
{
    public partial class Register : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Form.DefaultButton = this.btRegister.UniqueID;
                using (var context = new BD_IJPReportingEntities())
                {
                    Regions.DataSource = context.Regions.ToList();
                    Regions.DataTextField = "Name";
                    Regions.DataValueField = "region_id";
                    Regions.DataBind();
                }
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var roleManager = Context.GetOwinContext().Get<ApplicationRoleManager>();

            if (!Email.IsValid)
            {
                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                alert.Visible = true;
                alert.Title = (Language.ToString().Equals("en")) ? "Email is invalid" : "Le courriel n'est pas valide";
                alert.Content = (Language.ToString().Equals("en")) ? "Please enter a .gc.ca or canada.ca email" : "S'il vous plait, utilisez un courriel .gc.ca ou canada.ca";
                return;

            }
            if (!Telephone.IsValid)
            {
                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                alert.Visible = true;
                alert.Title = (Language.ToString().Equals("en")) ? "Telephone is invalid" : "Le numéro de téléphone n'est pas valide";
                alert.Content = (Language.ToString().Equals("en")) ? "Please enter a valid North American phone number" : "S'il vous plait, entrez un numéro de téléphone nord-américain valide";
                return;

            }
            if (!FirstName.IsValid)
            {
                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                alert.Visible = true;
                alert.Title = (Language.ToString().Equals("en")) ? "The first name field is mandatory" : "Le champ prénom est obligatoire";
                alert.Content = (Language.ToString().Equals("en")) ? "Please enter your first name in the field" : "S'il vous plait, inscrivez votre prénom dans le champ";
                return;

            }
            if (!LastName.IsValid)
            {
                alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                alert.Visible = true;
                alert.Title = (Language.ToString().Equals("en")) ? "The last name field is mandatory" : "Le champ nom est obligatoire";
                alert.Content = (Language.ToString().Equals("en")) ? "Please enter your last name in the last name field." : "S'il vous plait, inscrivez votre nom dans le champ";
                return;

            }

            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text, FirstName = FirstName.Text, LastName = LastName.Text, Telephone = Telephone.Text, LastPasswordChange = DateTime.Now };
            //var userRegionLink = new UsersRegions() { userId = user.Id, region_id = Convert.ToInt32(Regions.SelectedValue) };


            if (manager.FindByEmail(user.Email) != null)
            {
                if (Language.ToString().Equals("fr"))
                {
                    alert.Visible = true;
                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                    alert.Title = "Impossible de créer le compte";
                    alert.Content = "Le compte existe déjà";
                }
                else
                {
                    alert.Visible = true;
                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                    alert.Title = "Unable to create an account";
                    alert.Content = "The account already exists";
                }
                return;
            }

            IdentityResult result = manager.Create(user, Password.Text);
            

            if (result.Succeeded)
            {
                Int32.TryParse(Regions.SelectedValue, out int selectedRegionValue);
                using (var context = new BD_IJPReportingEntities())
                {
                    var userObj = context.AspNetUsers.SingleOrDefault(x => x.Id == user.Id);
                    var regionObj = context.Regions.SingleOrDefault(x => x.region_id == selectedRegionValue);
                    userObj.Regions.Add(regionObj);
                    //context.UsersRegions.Add(userRegionLink);
                    context.SaveChanges();
                }
                //manager.AddToRole(user.Id, "Client");
                //manager.AddToRole(user, "Client");
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                string code = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);

                try
                {
                    var absoluteUri = "/" + ConfigurationManager.AppSettings["rootProjet"] + "/Admin/ManageUser?userId=" + user.Id;
                    var manageUserAccountLnk =  new Uri(Request.Url, absoluteUri).AbsoluteUri.ToString();

                    manager.SendEmail(user.Id, GetGlobalResourceObject("Email", "ConfirmAccountSubject").ToString(), string.Format(GetGlobalResourceObject("Email", "ConfirmAccountContent").ToString(), callbackUrl));

                    foreach (Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole userole in roleManager.FindByName("Admin").Users)
                    {
                        manager.SendEmail(userole.UserId, GetGlobalResourceObject("Email", "AdminConfirmAccountSubject").ToString(), string.Format(GetGlobalResourceObject("Email", "AdminConfirmAccountContent").ToString(), user.FirstName + " " + user.LastName, manageUserAccountLnk));
                    }
                }
                catch (Exception ex)
                {
                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                    alert.Visible = true;
                    alert.Title = (ex.ToString());
                    alert.Content = (ex.ToString());

                    ExceptionIJP error = new ExceptionIJP();
                    error.HandleException(ex);

                    return;
                }

                if (user.EmailConfirmed)
                {
                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    if (Language.ToString().Equals("en"))
                    {
                        alert.Visible = true;
                        alert.AlertType = WetAlert.ALERT_TYPE.Warning;
                        alert.Title = "Email validation";
                        alert.Content = "An email has been sent to your account. Please view the email and confirm your account to complete the registration process.";

                    }
                    else
                    {
                        alert.Visible = true;
                        alert.AlertType = WetAlert.ALERT_TYPE.Warning;
                        alert.Title = "Validation de courriel";
                        alert.Content = "Un courriel a été envoyé à votre compte. S'il vous plait, vérifiez le courriel et validez votre compte pour finaliser votre enregistrement.";

                    }

                }

            }
            else
            {
                if (result.Errors.FirstOrDefault().Contains("Password"))
                {
                    alert.Visible = true;
                    alert.AlertType = WetAlert.ALERT_TYPE.Danger;
                    alert.Title = GetGlobalResourceObject("Global", "AlertInvalidPasswordTitle").ToString();
                    alert.Content = GetGlobalResourceObject("Global", "AlertInvalidPasswordContent").ToString(); ;

                }


            }
        }
    }
}