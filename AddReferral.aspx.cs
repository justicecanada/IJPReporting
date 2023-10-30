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

namespace IJPReporting
{
    public partial class CreateForm : BasePage
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

        ModelHelper helper = new ModelHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUserRole = CurrentUser.Roles?.First()?.RoleId;

            if (currentUserRole == RolesCode.IJPReadAll || currentUserRole == RolesCode.PTRead)
            {
                Response.Redirect("~/ClientReferralReporting.aspx");
            }

            if (!IsPostBack)
            {
                LoadForms();
                PopulateCommunitynProgram();
            }
        }

        protected void PopulateCommunitynProgram()
        {
            var currentUserRole = CurrentUser.Roles?.First()?.RoleId;
            var currentUserCommunity = CurrentUser.CommunityId;
            var currentUserProgram = CurrentUser.ProgramId;
            var currentUserRegions = CurrentUser.RegionsIds;

            try
            {
                using (var context = new Models.BD_IJPReportingEntities())
                {
                    // show all communities
                    var communities = context.Community.Where(x => !x.IsDeleted).ToList().OrderBy(x => x.Name).ToList();

                    // limit communities to the user's regions
                    if (currentUserRole != RolesCode.Admin)
                    {
                        communities = communities.Where(x => (currentUserRegions.Count > 0 ? currentUserRegions.Any(y => x.Region_id == x.Region_id) : false)).ToList();
                    }
                    
                    Communities.DataSource = communities;
                    Communities.DataTextField = "Name";
                    Communities.DataValueField = "Community_Id";
                    Communities.DataBind();

                    if(currentUserCommunity != null && currentUserCommunity > 0)
                    {
                        var programs = context.Programs.Where(x => !x.isDeleted && x.community_Id == currentUserCommunity).ToList().OrderBy(x => x.Name).ToList();

                        Programs.DataSource = programs;
                        Programs.DataTextField = "Name";
                        Programs.DataValueField = "programId";
                        Programs.DataBind();
                    }

                    
                }

                // populate 
                if (currentUserCommunity != null && (Communities.Items.FindByValue(currentUserCommunity.ToString()) != null))
                {
                    Communities.SelectedValue = currentUserCommunity.ToString();
                }
                if (currentUserProgram != null && (Programs.Items.FindByValue(currentUserProgram.ToString()) != null))
                {
                    Programs.SelectedValue = currentUserProgram.ToString();
                }

                Communities.Enabled = currentUserRole != RolesCode.Program;
                Programs.Enabled = currentUserRole != RolesCode.Program;
                
            }
            catch (Exception)
            {

            }     
        }

        protected void LoadForms()
        {
            formsTypes.DataSource = helper.GetProgramsForms();
            formsTypes.DataTextField = "Name";
            formsTypes.DataValueField = "FormTypeId";
            formsTypes.DataBind();
        }

        protected void goToWizard_Click(object sender, EventArgs e)
        {
            if (Page.IsWetValid())
            {    
                try
                {
                    
                    using (var context = new BD_IJPReportingEntities())
                    {
                        var currentUserId = User?.Identity?.GetUserId();
                        var currentUserObj = context.AspNetUsers.SingleOrDefault(x => x.Id == currentUserId);
                        var currentUserRegions = currentUserObj?.Regions;
                        Int32.TryParse(Communities.SelectedValue, out int selectedCommunity);
                        Int32.TryParse(Programs.SelectedValue, out int selectedProgram);
                        var communityRegionId = context.Community.SingleOrDefault(x => x.Community_Id == selectedCommunity).Region_id;
                        var RegionObj = context.Regions.SingleOrDefault(x => x.region_id == communityRegionId);

                        Forms form = new Forms
                        {
                            form_id = Guid.NewGuid(),
                            ClientFileNumberId = clientFileNumber.Text,
                            creator_id = currentUserId,
                            program_id = selectedProgram,
                            community_id = selectedCommunity,
                            RefDate = Convert.ToDateTime(refDate.Text),
                            FormType_id = formsTypes.SelectedValue.ToInt32(),
                            date_creation = DateTime.Today,
                            date_updated = DateTime.Today
                        };
                        form.Regions.Add(RegionObj);
                        context.Forms.Add(form);
                        int result = context.SaveChanges();
                        if(result > 0)
                        {
                            Response.Redirect("~/Referral?refId=" + form.form_id, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionIJP error = new ExceptionIJP();
                    error.HandleException(ex);
                }

                
            }
        }

        protected void Communities_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                Int32.TryParse(Communities.SelectedValue, out int selectedCommunity);
                var programs = context.Programs.Where(x => !x.isDeleted && (selectedCommunity > 0 ? x.community_Id == selectedCommunity : true)).ToList().OrderBy(x => x.Name).ToList();
                Programs.DataSource = programs;
                Programs.DataTextField = "Name";
                Programs.DataValueField = "programId";
                Programs.DataBind();
            }
        }
    }
}