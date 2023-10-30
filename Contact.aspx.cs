using GCWebUsabilityTheme;
using IJPReporting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IJPReporting
{
    public partial class Contact : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListItemCollection subjectsEmail = new ListItemCollection
                {
                    new ListItem("Tech Support", "sweb@justice.gc.ca"),
                    new ListItem("Web Admin", "ymanaf@justice.gc.ca")
                };
                subjectDdl.DataSource = subjectsEmail;
                subjectDdl.DataBind();
            }
        }

        protected void SendEmail_Click(object sender, EventArgs e)
        {
            if (this.IsWetValid())
            {
                Mailer mailer = new Mailer();
                string from = emailField.Text;
                string name = nameField.Text;
                string message = messageField.Text;
                string subject = String.Format("Message from {0}", name);
                string to = subjectDdl.SelectedValue == "sweb@justice.gc.ca" ? "sweb@justice.gc.ca" : "ymanaf@justice.gc.ca";
                bool sent = mailer.SendMail(subject, message, from, to) == 1;
                if (sent)
                {
                    wetAlert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Success;
                    wetAlert.Title = "Nous avons recus votre message!";
                    wetAlert.Visible = true;
                    upAlert.Update();
                    emailField.Text = String.Empty;
                    nameField.Text = String.Empty;
                    messageField.Text = String.Empty;
                    subjectDdl.ClearSelection();
                }
                else
                {
                    wetAlert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Danger;
                    wetAlert.Title = "Un problème est survenu, veuillez ressayer plus tard!";
                    wetAlert.Visible = true;
                    upAlert.Update();
                }
            } else
            {
                wetAlert.AlertType = WetControls.Controls.WetAlert.ALERT_TYPE.Danger;
                wetAlert.Title = "Un problème est survenu, veuillez ressayer plus tard!";
                wetAlert.Visible = true;
                upAlert.Update();
            }
            
        }
    }
}