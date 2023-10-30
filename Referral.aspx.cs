using GCWebUsabilityTheme;
using IJPReporting.Helpers;
using IJPReporting.Logic;
using IJPReporting.Managers;
using IJPReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IJPReporting
{
    [Serializable]
    public partial class Referral : BasePage
    {
        ModelHelper ModelHelper = new ModelHelper();
        WizardManager WizardManager = new WizardManager();
        WizardHelper WizardHelper = new WizardHelper();

        public static Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
                return root;

            return root.Controls.Cast<Control>()
               .Select(c => FindControlRecursive(c, id))
               .FirstOrDefault(c => c != null);
        }

        protected Guid FormId
        {
            get
            {
                Guid formId = Guid.Empty;
                string query = Request.QueryString["refId"];
                if (query != Guid.Empty.ToString())
                {
                    if (!String.IsNullOrEmpty(query))
                    {
                        formId = new Guid(query);
                    }
                }
                return formId;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return Request.QueryString["readOnly"]?.ToLower() == "true";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Forms form = ModelHelper.GetFormById(FormId);
            SaveWizardBtn.Visible = !ReadOnly;
            if (form != null)
            {
                formNameLabel.Text = ReadOnly ? String.Format("{0} (Read Only)", form.Form_Type.FormTitle_en) : form.Form_Type.FormTitle_en;
                clientFileNumber.Text = String.Format("#{0}", form.ClientFileNumberId);
                this.Title = String.Format("Referral {0} #{1}", form.Form_Type.FormTitle_en, form.ClientFileNumberId);
            }

            if (!IsPostBack)
            {
                if (FormId == Guid.Empty || FormId == null)
                {
                    Response.Redirect("~/Default.aspx", false);
                }
                else
                {
                    ViewState["form"] = form;
                    WizardManager.LoadWizard(FormId);
                    WizardManager.TriggerWizardEvents(FormId);
                }
            }
            else
            {
                ViewState["form"] = form;
                WizardManager.LoadWizard(FormId);
                WizardManager.Load_Dynamic_Controls();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (IsPostBack)
            {
                WizardManager.TriggerWizardEvents(FormId);
            }
        }

        protected void SideBarButton_Click(object sender, EventArgs e)
        {
            //test
            Wizard wizard = wizardPh.FindControl("wizard_" + FormId) as Wizard;
            Forms form = ViewState["form"] as Forms;
            if (wizard != null && form != null)
            {
                WizardHelper.SaveWizardResponses(wizard, form, false);
            }
            LinkButton btnStep = (LinkButton)sender;
            string step = btnStep.ClientID.ElementAt((btnStep.ClientID.Count()) - 1).ToString();
            var _step = Int32.TryParse(step, out int j);

            wizard.MoveTo(wizard.WizardSteps[j]);

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Wizard wizard = FindControlRecursive(wizardPh, "wizard_" + FormId) as Wizard;
            Forms form = ViewState["form"] as Forms;
            if (wizard != null && form != null)
            {
                if (WizardHelper.DisableStep(wizard, FormId))
                {
                    WizardHelper.SaveWizardResponses(wizard, form, false);
                }
            }

        }

        protected void SaveWizardBtn_Click(object sender, EventArgs e)
        {
            Wizard wizard = wizardPh.FindControl("wizard_" + FormId) as Wizard;
            Forms form = ViewState["form"] as Forms;
            if (wizard != null && form != null)
            {
                WizardHelper.SaveWizardResponses(wizard, form, false);
            }
        }

        //protected void DownloadWizard_Click(object sender, EventArgs e)
        //{
        //    Wizard wizard = wizardPh.FindControl("wizard_" + FormId) as Wizard;
        //    Forms form = ViewState["form"] as Forms;
        //    if (wizard != null && form != null)
        //    {
        //        string csv = WizardManager.DownloadReferral(wizard);
        //        Response.Clear();
        //        Response.ContentType = "text/csv";
        //        Response.AddHeader("Content-Disposition", "attachment;filename=myfilename.csv");
        //        Response.Write(csv);
        //        Response.End();
        //    }
        //}
    }
}