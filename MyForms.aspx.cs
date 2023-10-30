using GCWebUsabilityTheme;
using IJPReporting.Helpers;
using IJPReporting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IJPReporting
{
    public partial class MyForms : BasePage
    {
        Guid GetCurrentUserId
        {
            get
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return this.User != null ? new Guid(manager.FindByName(this.User.Identity?.Name)?.Id) : Guid.Empty;
            }
        }

        ModelHelper modelHelper = new ModelHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFiles();
            }
        }

        protected void LoadFiles()
        {
            using (var context = new BD_IJPReportingEntities())
            {
                List<Files> files = context.Files.Where(x => x.creator_id == GetCurrentUserId).ToList();
                formsRpt.DataSource = files;
                formsRpt.DataBind();
            }
        }

        protected string GetUserNameById(Guid userId)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = manager.FindById(userId.ToString());
            return user != null ? String.Format("{0}, {1}", user.FirstName, user.LastName) : String.Empty;
        }

        protected string GetExtensionIcon(string filename)
        {
            string extension = Path.GetExtension(filename).ToUpper();
            if (extension == ".PDF")
            {
                return "fas fa-file-pdf text-danger";
            }
            else if (extension == ".DOC" || extension == ".DOCX")
            {
                return "fas fa-file-word text-primary";
            }
            else if (extension == ".XLS" || extension == ".XLSX")
            {
                return "fas fa-file-excel text-success";
            }
            else if (extension == ".CSV")
            {
                return "fas fa-file-csv text-success";
            }
            return "fas fa-file-alt";
        }

        protected void formsRpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Guid fileId = new Guid(e.CommandArgument.ToString());
            Files file = modelHelper.GetFileById(fileId);
            string extension = Path.GetExtension(file.file_client_name);
            var path = Server.MapPath("~//UploadedForms") + "//" + file.file_id + extension;
            if (e.CommandName == "Delete" && fileId != Guid.Empty)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    file.Delete();
                    Response.Redirect("~/MyForms.aspx", false);
                }
            }
            else if (e.CommandName == "Save")
            {
                string FilePath = Server.MapPath("~//UploadedForms//" + file.file_id + extension);
                WebClient User = new WebClient();
                string contentType = String.Empty;
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if(extension.ToUpper() == ".PDF")
                {
                    contentType = "application/pdf";
                } else if(extension.ToUpper() == ".CSV")
                {
                    contentType = "text/csv";
                } else if (extension.ToUpper() == ".XLSX" || extension.ToUpper() == ".XLS")
                {
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                } else if(extension.ToUpper() == ".DOC" || extension.ToUpper() == ".DOCS")
                {
                    contentType = "application/msword";
                }
                if (FileBuffer != null)
                {               
                    Response.ContentType = contentType;
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + file.file_client_name);
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }
        }

        //protected void btnOk_Click(object sender, EventArgs e)
        //{
        //    mpePopUp.Hide();
        //}

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    mpePopUp.Hide();
        //}

        //protected void btnOpenPopUp_Click(object sender, EventArgs e)
        //{
        //    mpePopUp.Show();
        //}
        //protected void UploadBtn_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        Guid userId = this.User != null ? new Guid(manager.FindByName(this.User.Identity?.Name)?.Id) : Guid.Empty;
        //        string extension = Path.GetExtension(formUpload.FileName);

        //        if (userId != Guid.Empty)
        //        {
        //            Files file = new Files()
        //            {
        //                creator_id = userId,
        //                file_client_name = formUpload.FileName,
        //                file_id = Guid.NewGuid(),
        //                creation_date = DateTime.Today
        //            };
        //            formUpload.SaveAs(Server.MapPath("~//UploadedForms") + "//" + file.file_id + extension);
        //            using (var context = new BD_IJPReportingEntities())
        //            {
        //                context.Files.Add(file);
        //                context.SaveChanges();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
    }
}