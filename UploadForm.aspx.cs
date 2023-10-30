using GCWebUsabilityTheme;
using IJPReporting.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IJPReporting
{
    public partial class Upload : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                Guid userId = this.User != null ? new Guid(manager.FindByName(this.User.Identity?.Name)?.Id) : Guid.Empty;
                string extension = Path.GetExtension(formUpload.FileName);
                
                if(userId != Guid.Empty)
                {
                    Files file = new Files()
                    {
                        creator_id = userId,
                        file_client_name = formUpload.FileName,
                        file_id = Guid.NewGuid(),
                        creation_date = DateTime.Today
                    };
                    formUpload.SaveAs(Server.MapPath("~//UploadedForms") + "//" + file.file_id + extension);
                    using (var context = new BD_IJPReportingEntities())
                    {
                        context.Files.Add(file);
                        context.SaveChanges();
                    }
                    Response.Redirect("~/MyForms.aspx", false);
                }
                
            } catch (Exception ex)
            {

            }
            
        }
    }
}