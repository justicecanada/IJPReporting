using GCWebUsabilityTheme;
using IJPReporting.Helpers;
using IJPReporting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WetControls.Controls;

namespace IJPReporting
{
    public partial class Questionnaires : BasePage
    {
        protected ApplicationUser CurrentUser
        {
            get
            {
                HttpContext ctx = HttpContext.Current;
                var currentUserId = ctx?.User?.Identity?.GetUserId();
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return manager.FindById(User.Identity.GetUserId());
            }
        }

        Helpers.ModelHelper helper = new Helpers.ModelHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUserRole = CurrentUser.Roles?.First()?.RoleId;
            btnCreateForm.Visible = currentUserRole != RolesCode.IJPReadAll && currentUserRole != RolesCode.PTRead;

            if (!IsPostBack)
            {
                using(var context = new BD_IJPReportingEntities())
                {
                    var services = context.Form_Type.Where(x => !x.IsDisabled).ToList();
                    
                    servicesDDL.DataSource = services;
                    servicesDDL.DataValueField = "FormTypeId";
                    servicesDDL.DataTextField = "Name";
                    servicesDDL.DataBind();

                    RegionsDDL.DataSource = context.Regions.ToList();
                    RegionsDDL.DataTextField = "Name";
                    RegionsDDL.DataValueField = "region_id";
                    RegionsDDL.DataBind();
                }
            }

            UpdateFilterVisibility();
        }

        protected void UpdateFilterVisibility()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var currentUser = manager.FindById(User.Identity.GetUserId());
            var currentUserRole = currentUser?.Roles.First();
            RegionsDDL.Visible = currentUserRole.RoleId != RolesCode.IJPRegionalCoordinator || currentUserRole.RoleId == RolesCode.Admin;
            Communities.Visible = (currentUserRole.RoleId == RolesCode.RecipientUmbOrg || currentUserRole.RoleId == RolesCode.Program || currentUserRole.RoleId == RolesCode.Admin) && RegionsDDL.SelectedIndex > 0;
            Programs.Visible = (currentUserRole.RoleId == RolesCode.Program || currentUserRole.RoleId == RolesCode.Admin) && Communities.SelectedIndex > 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            servicesDDL.ClearSelection();
            completionStatusDDL.ClearSelection();
            acceptanceStatusDDL.ClearSelection();
            refDateStart.Text = "";
            refDateEnd.Text = "";
            RegionsDDL.ClearSelection();
            Communities.ClearSelection();
            Programs.ClearSelection();
            UpdateFilterVisibility();

        }

        protected void Communities_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                Int32.TryParse(Communities.SelectedValue, out int selectedCommunityId);
                var programs = context.Programs.Where(x => !x.isDeleted && x.community_Id == selectedCommunityId).OrderBy(x => x.name_en).ToList();

                Programs.DataSource = programs;
                Programs.DataTextField = "Name";
                Programs.DataValueField = "programId";
                Programs.DataBind();
            }
        }

        protected void RegionsDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                Int32.TryParse(RegionsDDL.SelectedValue, out int selectedRegionId);
                var communities = context.Community.Where(x => !x.IsDeleted && x.Region_id == selectedRegionId).OrderBy(x => x.Community_name_en).ToList();
                Communities.DataSource = communities;
                Communities.DataTextField = "Name";
                Communities.DataValueField = "Community_Id";
                Communities.DataBind();
            }
        }
    }
}