using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GCWebUsabilityTheme;
using IJPReporting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace IJPReporting.Admin
{
    public partial class Admin : BasePage
    {
        private static List<string> users_sa = new List<string>();
        private static List<string> users_client = new List<string>();
        private static Dictionary<string, string> roleChange_cache = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                var context = new ApplicationDbContext();

                var roleStore = new RoleStore<IdentityRole>(context);
                var roleMgr = new RoleManager<IdentityRole>(roleStore);

                var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

                var sa_id = roleMgr.FindByName("SuperAdmin").Id;
                var a_id = roleMgr.FindByName("Admin").Id;
                var c_id = roleMgr.FindByName("Client").Id;



                repeater_userSA.DataSource = userMgr.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(sa_id) && x.UserName.Contains("@justice.gc.ca")).ToList();//context.Users.Where(x => (x.Roles.Select(y => y.RoleId).Contains(sa_id) || x.Roles.Select(y => y.RoleId).Contains(a_id) || x.Roles.Select(y => y.RoleId).Contains(c_id)) && x.UserName.Contains("@justice.gc.ca")).ToList();
                repeater_userSA.DataBind();

                repeater_userAdmin.DataSource = userMgr.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(a_id) && x.UserName.Contains("@justice.gc.ca")).ToList();
                repeater_userAdmin.DataBind();

                repeater_userClient.DataSource = userMgr.Users.Where(x => !x.Roles.Select(y => y.RoleId).Contains(sa_id) && !x.Roles.Select(y => y.RoleId).Contains(a_id) && x.UserName.Contains("@justice.gc.ca")).ToList();
                repeater_userClient.DataBind();
            }



        }

        protected void UserRoleSuperAdmin_Change(Object sender, EventArgs e)
        {

            int index = -1;
            Int32.TryParse(((DropDownList)sender).Attributes["class"].Substring(6), out index);

            if (index > -1)
            {
                lbl_test.Text = users_sa[index];

                if (!roleChange_cache.ContainsKey(users_sa[index]))
                    roleChange_cache.Add(users_sa[index], ((DropDownList)sender).SelectedValue);
                else
                    roleChange_cache[users_sa[index]] = ((DropDownList)sender).SelectedValue;
            }


        }
        protected void UserRoleAdmin_Change(Object sender, EventArgs e)
        {

            //roleChange_cache.Add();
        }
        protected void UserRoleClient_Change(Object sender, EventArgs e)
        {
            int index = -1;
            Int32.TryParse(((DropDownList)sender).Attributes["class"].Substring(6), out index);

            if (index > -1)
            {
                lbl_test.Text = users_client[index];

                if (!roleChange_cache.ContainsKey(users_client[index]))
                    roleChange_cache.Add(users_client[index], ((DropDownList)sender).SelectedValue);
                else
                    roleChange_cache[users_client[index]] = ((DropDownList)sender).SelectedValue;
            }

        }

        protected void lbl_superAdmin_DataBinding(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                users_sa.Add(((Label)sender).Text);
            }

        }

        protected void lbl_client_DataBinding(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                users_client.Add(((Label)sender).Text);
            }
        }

        protected void btn_submitRoles_ServerClick(object sender, EventArgs e)
        {
            var context = new ApplicationDbContext();
            var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            foreach (KeyValuePair<string, string> user in roleChange_cache)
            {
                var user_Account = userMgr.FindByName(user.Key);
                var UserRoles = user_Account.Roles;
                if (user_Account.Roles != null)
                {
                    foreach (IdentityUserRole role in UserRoles.ToList())
                    {
                        userMgr.RemoveFromRole(userMgr.FindByName(user.Key).Id, roleMgr.FindById(role.RoleId).Name);
                    }
                }
                userMgr.AddToRole(userMgr.FindByName(user.Key).Id, user.Value);

                Response.Redirect(Request.RawUrl);
            }
        }

        protected void bt_rm_sa_ServerClick(object sender, EventArgs e)
        {
            var context = new ApplicationDbContext();
            var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            int index = -1;
            string user_index = ((HtmlButton)sender).Attributes["class"].Substring(31);
            Int32.TryParse(user_index, out index);

            if (index > -1)
            {
                userMgr.RemoveFromRole(userMgr.FindByName(users_sa[index]).Id, "SuperAdmin");
            }

            Response.Redirect(Request.RawUrl);
        }
    }
}