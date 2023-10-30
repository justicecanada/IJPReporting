using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GCWebUsabilityTheme;
using IJPReporting.Helpers;
using IJPReporting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace IJPReporting.Admin
{
    public partial class ConfigRoles : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (currentUser == null || !currentUser.Roles.Any(x => x.RoleId == RolesCode.Admin))
            {
                Response.Redirect("~/ClientReferralReporting");
            }

            if (!IsPostBack)
            {
                loadUsers();

            }
        }

        public void loadUsers()
        {
            var context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));


            
            //var query =
            //    (from user in userMgr.Users
            //     join role in roleMgr.Roles on user.Roles.FirstOrDefault().RoleId equals role.Id
            //     select new { user.FirstName, user.LastName, user.Email, role.Name, user.Id });

            

            rptRoles.DataSource = context.Users.ToList();
            rptRoles.DataBind();



        }


        //protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    var context = new ApplicationDbContext();
        //    var roleStore = new RoleStore<IdentityRole>(context);
        //    var roleMgr = new RoleManager<IdentityRole>(roleStore);

        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {

        //        Label userRole = ((Label)e.Item.FindControl("lblRole"));

        //        DropDownList rolesList = ((DropDownList)e.Item.FindControl("ddlRoles"));

        //        ApplicationUser currUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        //        //var currUserRole = currUser.Roles.Join(roleMgr.Roles, cu => cu.RoleId, r => r.Id, (cu, r) => new { r.Name }).SingleOrDefault().Name;


        //        //if (currUserRole.Equals("Admin"))
        //        //{
        //        //    rolesList.DataSource = roleMgr.Roles.Where(x => !x.Name.Equals("SuperAdmin")).Select(x => new { x.Name }).ToList();
        //        //}
        //        //else
        //        //{
        //        //    rolesList.DataSource = roleMgr.Roles.Select(x => new { x.Name }).ToList();
        //        //}

        //        //rolesList.DataTextField = "Name";
        //        //rolesList.DataBind();

        //    }

        //}

        protected void rptRoles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String newrole;
            String userEmail;

            var context = new ApplicationDbContext();
            var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            userEmail = e.CommandArgument.ToString();

            //if (e.CommandName == "submit")
            //{


            //    newrole = ((DropDownList)e.Item.FindControl("ddlRoles")).SelectedItem.Value;

            //    if (String.IsNullOrEmpty(newrole))
            //    {
            //        return;
            //    }



            //    var user = userMgr.FindByEmail(userEmail);
            //    var userRole = user.Roles.Count() > 0 ? user.Roles.First() : null;
            //    var currRoleId = userRole != null ? userRole.RoleId : "0";
            //    var currRoleName = roleMgr.Roles.Where(x => x.Id == currRoleId).FirstOrDefault()?.Name;

            //    if(!String.IsNullOrEmpty(currRoleName))
            //    {
            //        userMgr.RemoveFromRole(user.Id, currRoleName);

            //    }

            //    userMgr.AddToRole(user.Id, newrole);

            //    userMgr.Update(user);



            //}
            //else 
            if (e.CommandName == "delete")
            {
                var user = userMgr.FindByEmail(userEmail);
                using(var ctx = new BD_IJPReportingEntities())
                {
                    var userObj = ctx.AspNetUsers.SingleOrDefault(x => x.Id == user.Id);
                    userObj.Regions.Clear();
                    userObj.AspNetRoles.Clear();
                    ctx.SaveChanges();
                    ctx.AspNetUsers.Remove(userObj);
                    ctx.SaveChanges();
                }

                //user = userMgr.FindByEmail(userEmail);
                //userMgr.Delete(user); // To fix

            } else if (e.CommandName == "Manage")
            {
                Response.Redirect("~/Admin/ManageUser.aspx?userId=" + e.CommandArgument);
            }
            loadUsers();
        }


    }
}