using GCWebUsabilityTheme;
using IJPReporting.Helpers;
using IJPReporting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IJPReporting.Admin
{
    public partial class ManageUser : BasePage
    {
        ModelHelper ModelHelper = new ModelHelper();

        protected Guid UserID
        {
            get
            {
                Guid UserID = Guid.Empty;
                string query = Request.QueryString["userId"];
                if (query != Guid.Empty.ToString())
                {
                    if (!String.IsNullOrEmpty(query))
                    {
                        UserID = new Guid(query);
                    }
                }
                return UserID;
            }
        }

        protected ApplicationUser RequestedUser
        {
            get
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return manager?.FindById(UserID.ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (RequestedUser == null || currentUser == null || !currentUser.Roles.Any(x=>x.RoleId == RolesCode.Admin))
            {
                Response.Redirect("~/ClientReferralReporting");
            }

            if (!IsPostBack)
            {
                

                using (var context = new BD_IJPReportingEntities())
                {
                    var ctx = new ApplicationDbContext();
                    var roleStore = new RoleStore<IdentityRole>(ctx);
                    var roleMngr = new RoleManager<IdentityRole>(roleStore);
                    var roles = roleMngr.Roles.ToList();

                    Roles.DataSource = roles;
                    Roles.DataTextField = "Name";
                    Roles.DataValueField = "Id";
                    Roles.DataBind();

                    RegionsCBL.DataSource = context.Regions.ToList();
                    RegionsCBL.DataTextField = "Name";
                    RegionsCBL.DataValueField = "region_id";
                    RegionsCBL.DataBind();

                    RegionsDDL.DataSource = context.Regions.ToList();
                    RegionsDDL.DataTextField = "Name";
                    RegionsDDL.DataValueField = "region_id";
                    RegionsDDL.DataBind();

                    var userRegions = ModelHelper.GetRegionsByUser(RequestedUser);
                    var userRegionId = userRegions.Count > 0 ? userRegions.ElementAt(0).region_id : 0;
                    var communities = context.Community.Where(x => !x.IsDeleted && (userRegionId > 0 ? x.Region_id == userRegionId : true)).ToList().OrderBy(x=>x.Name);
                    Communities.DataSource = communities;
                    Communities.DataTextField = "Name";
                    Communities.DataValueField = "Community_Id";
                    Communities.DataBind();


                    var userCommunity = RequestedUser?.CommunityId;

                    var programs = context.Programs.Where(x => !x.isDeleted && (userCommunity > 0 ? x.community_Id == userCommunity : true)).ToList().OrderBy(x=>x.Name);

                    Programs.DataSource = programs;
                    Programs.DataTextField = "Name";
                    Programs.DataValueField = "programId";
                    Programs.DataBind();

                }




                FillUserInfo();

            }
            

            var selectedRole = Roles.SelectedValue;

            RegionsCBL.Visible = selectedRole == RolesCode.IJPRegionalCoordinator;
            RegionsDDL.Visible = selectedRole != RolesCode.IJPRegionalCoordinator;
            Communities.Visible = selectedRole == RolesCode.RecipientUmbOrg || selectedRole == RolesCode.Program;
            Programs.Visible = selectedRole == RolesCode.Program && Communities.SelectedIndex > 0;

        }

        protected void FillUserInfo()
        {
            string userRoleId = RequestedUser?.Roles.Count() > 0 ? RequestedUser.Roles.First()?.RoleId : null;
            string userCommunityId = RequestedUser?.CommunityId.ToString();
            string userProgramId = RequestedUser?.ProgramId.ToString();
            List<int> userRegionsIds = RequestedUser?.RegionsIds;

            var userRegions = ModelHelper.GetRegionsByUser(RequestedUser);
            string userLabel = RequestedUser != null ? String.Format("{0}, {1} (", RequestedUser.FirstName, RequestedUser.LastName) : "";

            foreach (var region in userRegions)
            {
                userLabel += userRegions.IndexOf(region) == userRegions.Count - 1 ? region.Name + ")" : region.Name + ", ";
            }
            userName.Text = userLabel;

            if (userRoleId != null && userRoleId != "0" && (Roles.Items.FindByValue(userRoleId) != null))
            {
                Roles.SelectedValue = userRoleId;
            }
            if (userCommunityId != "0" && (Communities.Items.FindByValue(userCommunityId) != null))
            {
                Communities.SelectedValue = userCommunityId;
            }
            if (userProgramId != "0" && (Programs.Items.FindByValue(userProgramId) != null))
            {
                Programs.SelectedValue = userProgramId;
            }
            if(userRegionsIds != null && userRegionsIds.Count > 0)
            {
                RegionsDDL.SelectedValue = userRegionsIds.ElementAt(0).ToString();
            }

            foreach (ListItem item in RegionsCBL.Items)
            {
                Int32.TryParse(item.Value, out int itemValue);
                if(userRegionsIds != null)
                {
                    item.Selected = userRegionsIds.Contains(itemValue);

                }
            }
        }

        protected void Roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        protected void saveChanges_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            Int32.TryParse(Programs.SelectedValue, out int selectedProgram);
            Int32.TryParse(Communities.SelectedValue, out int selectedCommunity);

            string selectedRoleText = Roles.SelectedItem.Text;
            string selectedRole = Roles.SelectedValue;

            // Programd and Community
            if(selectedRole == RolesCode.Program)
            {
                RequestedUser.ProgramId = selectedProgram;
                RequestedUser.CommunityId = selectedCommunity;

            } else if(selectedRole == RolesCode.RecipientUmbOrg)
            {
                RequestedUser.CommunityId = selectedCommunity;

            } else
            {
                RequestedUser.ProgramId = null;
                RequestedUser.CommunityId = null;
            }
            
            
            

            using (var context = new BD_IJPReportingEntities())
            {

                // Regions
                var userObj = context.AspNetUsers.SingleOrDefault(x => x.Id == RequestedUser.Id);

                userObj.Regions.Clear();
                
                context.SaveChanges();

                if (selectedRole == RolesCode.IJPRegionalCoordinator)
                {
                    foreach (ListItem regionItem in RegionsCBL.Items)
                    {
                        if (regionItem.Selected)
                        {
                            Int32.TryParse(regionItem.Value, out int regionId);
                            var region = context.Regions.SingleOrDefault(x => x.region_id == regionId);
                            if(region != null)
                            {
                                userObj.Regions.Add(region);
                            }
                        }
                    }
                } else
                {
                    Int32.TryParse(RegionsDDL.SelectedValue, out int regionId);
                    var region = context.Regions.SingleOrDefault(x => x.region_id == regionId);
                    userObj.Regions.Add(region);
                }
                

                // Roles
                var roles = manager.GetRoles(RequestedUser.Id);
                manager.RemoveFromRoles(RequestedUser.Id, roles.ToArray());
                manager.AddToRole(RequestedUser.Id, Roles.SelectedItem.Text);

                // Save changes
                manager.Update(RequestedUser);
                //context.SaveChanges();

                if (context.SaveChanges() > 0)
                {
                    #if !DEBUG
                    manager.SendEmail(RequestedUser.Id, "New role granted", String.Format("You have been granted {0} role", Roles.SelectedItem.Text));
                    #endif
                }

                FillUserInfo();
            }

            
            

        }

        protected void Communities_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                Int32.TryParse(Communities.SelectedValue, out int selectedCommunity);

                var programs = context.Programs.Where(x => !x.isDeleted && (selectedCommunity > 0 ? x.community_Id == selectedCommunity : true)).ToList().OrderBy(x => x.Name);
                Programs.DataSource = programs;
                Programs.DataBind();
            }
        }

        protected void RegionsDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                Int32.TryParse(RegionsDDL.SelectedValue, out int selectedRegion);
                var communities = context.Community.Where(x => !x.IsDeleted && (selectedRegion > 0 ? x.Region_id == selectedRegion : true)).ToList().OrderBy(x => x.Name);
                Communities.DataSource = communities;
                Communities.DataBind();
            }

        }
    }
}