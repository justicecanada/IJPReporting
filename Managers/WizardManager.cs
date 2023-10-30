using IJPReporting.Code;
using IJPReporting.Helpers;
using IJPReporting.Logic;
using IJPReporting.Models;
using IJPReporting.UserControls;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WetControls.Controls;
using WetControls.Interfaces;
using static IJPReporting.Helpers.IJPCodes;

namespace IJPReporting.Managers
{
    [Serializable]
    public class WizardManager
    {
        ModelHelper ModelHelper = new ModelHelper();
        WizardHelper WizardHelper = new WizardHelper();

        public static Page CurrentPage
        {
            get
            {
                return HttpContext.Current.CurrentHandler as Page;
            }
        }

        public static Wizard _wizard
        {
            get
            {
                return (Wizard)FindControlRecursive(CurrentPage, String.Format("wizard_{0}", (GetCurrentPageViewState()["form"] as Forms).form_id));

            }
        }


        public static StateBag GetCurrentPageViewState()
        {
            Page page = HttpContext.Current.Handler as Page;
            var viewStateProp = page?.GetType().GetProperty("ViewState",
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            return (StateBag)viewStateProp?.GetValue(page);
        }

        public static Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
                return root;

            return root.Controls.Cast<Control>()
               .Select(c => FindControlRecursive(c, id))
               .FirstOrDefault(c => c != null);
        }

        public bool ReadOnly
        {
            get
            {
                return CurrentPage.Request.QueryString["readOnly"]?.ToLower() == "true";
            }
        }
        public bool Edit
        {
            get
            {
                return CurrentPage.Request.QueryString["Edit"]?.ToLower() == "true";
            }
        }

        public bool Review
        {
            get
            {
                return CurrentPage.Request.QueryString["Review"]?.ToLower() == "true";
            }
        }

        public List<ViewStateControl> ViewStateControls
        {
            get
            {
                if (GetCurrentPageViewState()["dynamic_controls"] == null)
                {
                    GetCurrentPageViewState()["dynamic_controls"] = new List<ViewStateControl>();
                }
                return (List<ViewStateControl>)GetCurrentPageViewState()["dynamic_controls"];
            }
        }
        public void Wizard_PreRender(object sender, EventArgs e)
        {
            var SideBarList = (Repeater)FindControlRecursive(CurrentPage, "SideBarList");
            var steps = _wizard.WizardSteps;
            List<WizardStepBase> tempSteps = new List<WizardStepBase>();
            int? lastStepIndex = WizardHelper.GetLastStepIndex(_wizard, (GetCurrentPageViewState()["form"] as Forms).form_id);
          
            if (lastStepIndex != null)
            {
                for (int i = 0; i < steps.Count; ++i)
                {
                    if (i <= lastStepIndex)
                    {
                        tempSteps.Add(steps[i]);
                    }
                }
                SideBarList.DataSource = tempSteps;

            } else
            {
                SideBarList.DataSource = steps;
            }


            SideBarList.DataBind();

        }
        

        public static string GetClassForWizardStep(object wizardStep)
        {


            if (!(wizardStep is WizardStep step))
            {
                return "";
            }
            int stepIndex = _wizard.WizardSteps.IndexOf(step);

            if (stepIndex < _wizard.ActiveStepIndex)
            {
                return "prevStep";
            }
            else if (stepIndex > _wizard.ActiveStepIndex)
            {
                return "nextStep";
            }
            else
            {
                return "currentStep";
            }
        }

        public void LoadWizard(Guid? formId)
        {
            try
            {
                Forms form = ModelHelper.GetFormById(formId);
                if (form != null)
                {
                    Wizard wizard = new Wizard
                    {
                        FinishDestinationPageUrl = "~/ClientReferralReporting.aspx",
                        ID = "wizard_" + form.form_id
                    };
                    wizard.CssClass = "mrgn-tp-md";
                    wizard.StepNextButtonStyle.CssClass = "btn btn-default";
                    wizard.StartNextButtonStyle.CssClass = "btn btn-default";
                    wizard.FinishPreviousButtonStyle.CssClass = "btn btn-default";
                    wizard.StepPreviousButtonStyle.CssClass = "btn btn-default mrgn-rght-sm";
                    wizard.FinishCompleteButtonStyle.CssClass = "btn btn-primary mrgn-lft-sm";
                    wizard.FinishCompleteButtonText = "Save and Exit";
                    wizard.DisplaySideBar = false;
                    wizard.PreRender += new EventHandler(Wizard_PreRender);
                    if (/*!ReadOnly*/ true)
                    {
                        wizard.PreviousButtonClick += new WizardNavigationEventHandler(Wizard_PreviousButtonClick);
                        wizard.NextButtonClick += new WizardNavigationEventHandler(Wizard_NextButtonClick);
                        wizard.FinishButtonClick += new WizardNavigationEventHandler(Wizard_FinishButtonClick);
                    }

                    WizardStepBase newStep;
                    for (int j = 0; j < form.Form_Type.Section.OrderBy(x=>x.Ordre).Count(); ++j)
                    {
                        // add a stepwizard for every form section
                        var section = form.Form_Type.Section.OrderBy(x => x.Ordre).ElementAt(j);

                        newStep = new WizardStep
                        {
                            Title = section.Titre_fr,
                            ID = "step_" + j,
                            StepType = WizardStepType.Auto

                        };

                        // add questions to section
                        foreach (var q in section.Questions.Where(x => !x.IsSubQuestion && !x.isObsolete && x.FormTypeId == form.FormType_id).OrderBy(x => x.NoQuestion))
                        {
                            var question = ModelHelper.GetQuestionById(q.question_id.ToString());
                            var responses = form.UserResponses.Where(x => x.question_id == question.question_id).OrderBy(x => x.question_group); //get all responses in case of multiple controllers type (eg. multiple dropdown, multiple radio button,..)
                            // group var is used for UI to group questions inside a div (bs-callout)
                            Guid? group = question.question_type_id == (int)QuestionType.MultipleDropdownList ? Guid.NewGuid() : default(Guid);

                            if (responses.Count() > 0)
                            {
                                foreach (var response in responses)
                                {
                                    List<Questions> subQuestions = ModelHelper.GetSubquestions(question);
                                    AddQuestionToStep(form, question, newStep, response, group);
                                    foreach (var sq in subQuestions)
                                    {
                                        var subQuestionReponse = form.UserResponses.Where(x => x.question_id == sq.question_id && x.question_group == response.question_group).FirstOrDefault();
                                        if (sq != null)
                                        {
                                            AddQuestionToStep(form, sq, newStep, subQuestionReponse, group);
                                        }
                                    }
                                    group = Guid.NewGuid();
                                }

                            } else
                            {
                                List<Questions> subQuestions = ModelHelper.GetSubquestions(question);
                                AddQuestionToStep(form, question, newStep, null, group);
                                foreach (var sq in subQuestions)
                                {
                                    if (sq != null)
                                    {
                                        AddQuestionToStep(form, sq, newStep, null, group);
                                    }
                                }
                                group = Guid.NewGuid();
                            } 
                        }
                        wizard.WizardSteps.Add(newStep);
                    }
                    FindControlRecursive(CurrentPage, "wizardPh").Controls.Add(wizard);
                }
            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

        public void AddQuestionToStep(Forms form, Questions question, WizardStepBase newStep, UserResponses response, Guid? group)
        {
            Control control;
            Guid? selectedValue = (response != null && response?.response_choice_id != Guid.Empty) ? response.response_choice_id : null;
            var question_id = response == null ? question.question_id + "_1" : question.question_id + "_" + response.question_group;
            string cssClass = group != Guid.Empty ? "group_" + group : "";
            var responsesChoice = question.ReponseChoices.Where(x => !x.isObsolete).OrderBy(x => x.ordre).ToList();
            var responsesChoiceToDisable = question.ReponseChoices.Where(x => x.disabled); // add logic to dynamic controls (viewstate) too!!!
            string questionNote = String.Format("<span class=\"glyphicon glyphicon-question-sign help text-primary\"></span></br><span class=\"questionHint\">{0}</span>", question.Note);

            try
            {
                switch (question.question_type_id)
                {
                    case (int)QuestionType.RadioButtonList:
                        int responseIndex = response != null ? responsesChoice.FindIndex(x => x.response_choice_id == response.response_choice_id) : -1;
                        control = new WetRadioButtonList
                        {
                            LabelText = question.Label + questionNote,
                            DataSource = responsesChoice,
                            DataValueField = "response_choice_id",
                            DataTextField = "Text",
                            ID = question_id,
                            IsRequired = question.IsMandatory,
                            Visible = !question.IsConditional,
                            SelectedIndex = responseIndex,
                            Enabled = !ReadOnly,
                            CssClass = cssClass + " form-group",

                        };
                        if (ModelHelper.QuestionHasCondition(question.question_id.ToString()))
                        {
                            (control as WetRadioButtonList).SelectedIndexChanged += Ddl_Index_changed;
                            (control as WetRadioButtonList).AutoPostBack = true;
                        }
                        (control as WetRadioButtonList).DataBind();
                        control.EnableViewState = false;
                        newStep.Controls.Add(control as WetRadioButtonList);
                        break;

                    case (int)QuestionType.DropdownList:
                        control = new WetDropDownList
                        {
                            LabelText = question.Label + questionNote,
                            DataSource = responsesChoice,
                            DataValueField = "response_choice_id",
                            DataTextField = "Text",
                            ID = question_id,
                            IsRequired = question.IsMandatory,
                            Visible = !question.IsConditional,
                            SelectedValue = selectedValue?.ToString(),
                            Enabled = !ReadOnly,
                            CssClass = cssClass,
                        };
                        if (ModelHelper.QuestionHasCondition(question.question_id.ToString()))
                        {
                            (control as WetDropDownList).SelectedIndexChanged += Ddl_Index_changed;
                            (control as WetDropDownList).AutoPostBack = true;
                        }
                        (control as WetDropDownList).DataBind();

                        // disable items
                        foreach (var responseChoice in responsesChoiceToDisable)
                        {
                            var dropdownItem = (control as WetDropDownList).Items.FindByValue(responseChoice.response_choice_id.ToString());
                            if(dropdownItem != null)
                            {
                                dropdownItem.Attributes.Add("disabled", "disabled");
                            }
                        }
                        control.EnableViewState = false;
                        newStep.Controls.Add(control as WetDropDownList);
                        break;

                    case (int)QuestionType.TextBox:
                        control = new WetTextBox
                        {
                            LabelText = question.Label + questionNote,
                            ID = question_id,
                            IsRequired = question.IsMandatory,
                            Visible = !question.IsConditional,
                            Text = response?.devlopment.ToString(),
                            Enabled = !ReadOnly,
                            CssClass = cssClass
                        };
                        control.EnableViewState = false;
                        newStep.Controls.Add(control as WetTextBox);
                        break;

                    case (int)QuestionType.TextArea:
                        control = new WetTextBox
                        {
                            LabelText = question.Label + questionNote,
                            ID = question_id,
                            IsRequired = question.IsMandatory,
                            Visible = !question.IsConditional,
                            Text = response?.devlopment.ToString(),
                            Enabled = !ReadOnly,
                            CssClass = cssClass,
                            TextMode = TextBoxMode.MultiLine,
                            Rows = 10
                        };
                        control.EnableViewState = false;
                        newStep.Controls.Add(control as WetTextBox);
                        break;

                    case (int)QuestionType.DatePicker:
                        control = new WetTextBox
                        {
                            LabelText = question.Label + questionNote,
                            ID = question_id,
                            IsRequired = question.IsMandatory,
                            Visible = !question.IsConditional,
                            Text = response?.devlopment.ToString(),
                            IsDate = true,
                            Enabled = !ReadOnly,
                            CssClass = cssClass
                        };
                        control.EnableViewState = false;
                        newStep.Controls.Add(control as WetTextBox);
                        break;

                    case (int)QuestionType.CheckBoxList:
                        var multipleResponses = form.UserResponses.Where(x => x.question_id == question.question_id && response != null ? x.question_group == response.question_group : true);
                        control = new WetCheckBoxList
                        {
                            LabelText = question.Label + questionNote,
                            DataSource = responsesChoice,
                            DataValueField = "response_choice_id",
                            DataTextField = "Text",
                            ID = question_id,
                            IsRequired = question.IsMandatory,
                            Visible = !question.IsConditional,
                            Enabled = !ReadOnly,
                            CssClass = cssClass,
                        };
                        control.EnableViewState = false;
                        (control as WetCheckBoxList).DataBind();
                        foreach (ListItem item in (control as WetCheckBoxList).Items)
                        {
                            item.Selected = multipleResponses.Any(x => x.response_choice_id.ToString() == item.Value);
                        }
                        newStep.Controls.Add(control as WetCheckBoxList);
                        break;

                    case (int)QuestionType.MultipleDropdownList:
                        if (response?.question_group > 1)
                        {
                            control = (UCRemovableDropdownList)CurrentPage.LoadControl("~/UserControls/UCRemovableDropdownList.ascx");
                            (control as UCRemovableDropdownList).ID = question_id;
                            (control as UCRemovableDropdownList).wetRemoveDDL.ToolTip = "Remove " + question.Label;
                            (control as UCRemovableDropdownList).wetDDL.ID = question_id;
                            (control as UCRemovableDropdownList).wetDDL.LabelText = question.Label + questionNote;
                            (control as UCRemovableDropdownList).wetDDL.DataSource = question.ReponseChoices.OrderBy(x => x.ordre).ToList();
                            (control as UCRemovableDropdownList).wetDDL.DataValueField = "response_choice_id";
                            (control as UCRemovableDropdownList).wetDDL.DataTextField = "Text";
                            (control as UCRemovableDropdownList).wetDDL.SelectedValue = selectedValue?.ToString();
                            (control as UCRemovableDropdownList).wetDDL.IsRequired = question.IsMandatory;
                            (control as UCRemovableDropdownList).wetDDL.Enabled = !ReadOnly;
                            (control as UCRemovableDropdownList).wetRemoveDDL.Visible = !ReadOnly;
                            (control as UCRemovableDropdownList).Visible = !question.IsConditional;
                            (control as UCRemovableDropdownList).RemoveDDLClick += UcRemovableDDL_RemoveDDL;
                            (control as UCRemovableDropdownList).wetDDL.CssClass = cssClass;
                            if (ModelHelper.QuestionHasCondition(question.question_id.ToString()))
                            {
                                (control as UCRemovableDropdownList).wetDDL.SelectedIndexChanged += Ddl_Index_changed;
                                (control as UCRemovableDropdownList).wetDDL.AutoPostBack = true;
                            }

                            (control as UCRemovableDropdownList).wetDDL.DataBind();

                            control.EnableViewState = false;
                            foreach (var responseChoice in responsesChoiceToDisable)
                            {
                                var dropdownItem = (control as UCRemovableDropdownList).wetDDL.Items.FindByValue(responseChoice.response_choice_id.ToString());
                                if (dropdownItem != null)
                                {
                                    dropdownItem.Attributes.Add("disabled", "disabled");
                                }
                            }
                            newStep.Controls.Add(control as UCRemovableDropdownList);
                        }
                        else
                        {
                            control = (UCMultipleDropdownList)CurrentPage.LoadControl("~/UserControls/UCMultipleDropdownList.ascx");
                            (control as UCMultipleDropdownList).WetAddDDL.ToolTip = "Add additional " + question.Label;
                            (control as UCMultipleDropdownList).ID = question_id;
                            (control as UCMultipleDropdownList).WetDDL.ID = question_id;
                            (control as UCMultipleDropdownList).WetDDL.LabelText = question.Label + questionNote;
                            (control as UCMultipleDropdownList).WetDDL.IsRequired = question.IsMandatory;
                            (control as UCMultipleDropdownList).WetDDL.DataSource = question.ReponseChoices.OrderBy(x => x.ordre).ToList();
                            (control as UCMultipleDropdownList).WetDDL.DataValueField = "response_choice_id";
                            (control as UCMultipleDropdownList).WetDDL.DataTextField = "Text";
                            (control as UCMultipleDropdownList).WetDDL.SelectedValue = selectedValue?.ToString();
                            (control as UCMultipleDropdownList).WetDDL.Enabled = !ReadOnly;
                            (control as UCMultipleDropdownList).WetAddDDL.Visible = !ReadOnly;
                            (control as UCMultipleDropdownList).Visible = !question.IsConditional;
                            (control as UCMultipleDropdownList).AddDDLClick += UcMultipleDDL_AddDDL;
                            (control as UCMultipleDropdownList).WetDDL.CssClass = cssClass;
                            if (ModelHelper.QuestionHasCondition(question.question_id.ToString()))
                            {
                                (control as UCMultipleDropdownList).WetDDL.SelectedIndexChanged += Ddl_Index_changed;
                                (control as UCMultipleDropdownList).WetDDL.AutoPostBack = true;
                            }
                            (control as UCMultipleDropdownList).WetDDL.DataBind();
                            control.EnableViewState = false;
                            foreach (var responseChoice in responsesChoiceToDisable)
                            {
                                var dropdownItem = (control as UCMultipleDropdownList).WetDDL.Items.FindByValue(responseChoice.response_choice_id.ToString());
                                if (dropdownItem != null)
                                {
                                    dropdownItem.Attributes.Add("disabled", "disabled");
                                }
                            }
                            newStep.Controls.Add(control as UCMultipleDropdownList);
                        }
                        break;
                }
            } catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            } 
        }

        public void Load_Dynamic_Controls()
        {
            try
            {
                if (GetCurrentPageViewState()["dynamic_controls"] != null)
                {
                    foreach (var control in ViewStateControls)
                    {
                        if(FindControlRecursive(CurrentPage, control.Id) == null) // check if control already exists
                        {
                            switch (control.Type.ToLower()) // build controls retrieved in ViewState
                            {
                                case "wetdropdownlist":
                                    WetDropDownList ddl = new WetDropDownList
                                    {
                                        ID = control.Id,
                                        LabelText = control.LabelText,
                                        DataSource = control.DataSource,
                                        DataValueField= "response_choice_id",
                                        DataTextField = "Text",
                                        Visible = control.Visible,
                                        IsRequired = control.IsRequired,
                                        CssClass = control.CssClass
                                    };
                                    ddl.SelectedIndexChanged += Ddl_Index_changed;
                                    ddl.AutoPostBack = true;
                                    ddl.DataBind();
                                    foreach (var responseChoice in control.DisabledResponsesChoice)
                                    {
                                        var dropdownItem = ddl.Items.FindByValue(responseChoice.response_choice_id.ToString());
                                        if (dropdownItem != null)
                                        {
                                            dropdownItem.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    _wizard.WizardSteps[control.StepIndex].Controls.AddAt(control.Index, ddl);
                                    break;
                                case "wetradiobuttonlist":
                                    WetRadioButtonList rbl = new WetRadioButtonList
                                    {
                                        ID = control.Id,
                                        LabelText = control.LabelText,
                                        DataSource = control.DataSource,
                                        DataValueField = "response_choice_id",
                                        DataTextField = "Text",
                                        Visible = control.Visible,
                                        IsRequired = control.IsRequired,
                                        CssClass = control.CssClass

                                    };
                                    rbl.SelectedIndexChanged += Ddl_Index_changed;
                                    rbl.AutoPostBack = true;
                                    rbl.DataBind();
                                    _wizard.WizardSteps[control.StepIndex].Controls.AddAt(control.Index, rbl);
                                    break;
                                case "wetcheckboxlist":
                                    WetCheckBoxList cbl = new WetCheckBoxList
                                    {
                                        ID = control.Id,
                                        LabelText = control.LabelText,
                                        DataSource = control.DataSource,
                                        DataValueField = "response_choice_id",
                                        DataTextField = "Text",
                                        Visible = control.Visible,
                                        IsRequired = control.IsRequired,
                                        CssClass = control.CssClass

                                    };
                                    cbl.DataBind();
                                    _wizard.WizardSteps[control.StepIndex].Controls.AddAt(control.Index, cbl);
                                    break;
                                case "usercontrols_ucremovabledropdownlist_ascx":
                                    UCRemovableDropdownList DDL;
                                    DDL = (UCRemovableDropdownList)CurrentPage.LoadControl("~/UserControls/UCRemovableDropdownList.ascx");
                                    DDL.Visible = control.Visible;
                                    DDL.ID = control.Id;
                                    DDL.wetDDL.LabelText = control.LabelText;
                                    DDL.wetDDL.DataSource = control.DataSource;
                                    DDL.wetRemoveDDL.ToolTip = "Remove " + control.ToolTip;
                                    DDL.wetDDL.DataValueField = "response_choice_id";
                                    DDL.wetDDL.DataTextField = "Text";
                                    DDL.wetDDL.ID = control.Id;
                                    DDL.wetDDL.IsRequired = control.IsRequired;
                                    DDL.wetDDL.CssClass = control.CssClass;
                                    DDL.wetDDL.DataBind();
                                    DDL.RemoveDDLClick += UcRemovableDDL_RemoveDDL;
                                    DDL.wetDDL.SelectedIndexChanged += Ddl_Index_changed;
                                    DDL.wetDDL.AutoPostBack = true;
                                    foreach (var responseChoice in control.DisabledResponsesChoice)
                                    {
                                        var dropdownItem = DDL.wetDDL.Items.FindByValue(responseChoice.response_choice_id.ToString());
                                        if (dropdownItem != null)
                                        {
                                            dropdownItem.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    _wizard.WizardSteps[control.StepIndex].Controls.AddAt(control.Index, DDL);
                                    break;
                                case "wettextbox":
                                    WetTextBox wetTextBox = new WetTextBox
                                    {
                                        ID = control.Id,
                                        LabelText = control.LabelText,
                                        Visible = control.Visible,
                                        IsDate = control.IsDate,
                                        IsRequired = control.IsRequired,
                                        CssClass = control.CssClass
                                    };
                                    _wizard.WizardSteps[control.StepIndex].Controls.AddAt(control.Index, wetTextBox);
                                    break;
                            }
                        }
                    }
                }

            } catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }            
        }

        public void Ddl_Index_changed(object sender, EventArgs e)
        {
            try
            {                
                Guid selectedValue = Guid.Empty; // the user response for the question source
                // get the question source ID in order to extract from database all the conditions related to this question.
                string question_source_id = String.Empty;

                var senderQuestionGroup = ((Control)sender).ID.Substring(36, ((Control)sender).ID.Length - 36); // eg: _1

                // the control that we need to hide or show, depending on the condition of the source question
                Control targetCtrl = new Control();
                bool conditionSatisfied = false;
                Questions targetQuestion;

                if (sender is WetDropDownList)
                {
                    selectedValue = String.IsNullOrEmpty((sender as WetDropDownList).SelectedValue) ? Guid.Empty : new Guid((sender as WetDropDownList).SelectedValue);
                    question_source_id = (sender as WetDropDownList).ID.Substring(0, 36);
                }
                else if (sender is WetRadioButtonList)
                {
                    selectedValue = String.IsNullOrEmpty((sender as WetRadioButtonList).SelectedValue) ? Guid.Empty : new Guid((sender as WetRadioButtonList).SelectedValue);
                    question_source_id = (sender as WetRadioButtonList).ID.Substring(0, 36);
                }

                var conditions = ModelHelper.GetConditionsByQuestionId(new Guid(question_source_id));

                if (_wizard != null && (GetCurrentPageViewState()["form"] as Forms) != null)
                {
                    Questions questionSource = ModelHelper.GetQuestionById(question_source_id);
                    Forms form = GetCurrentPageViewState()["form"] as Forms;

                    // iterate over the question on which the conditions will be applied
                    foreach (var cond in conditions.Where(x => x.action_id != (int)Condition.Finish).DistinctBy(x => x.dest_question_id))
                    {
                        conditionSatisfied = false;

                        // the target question object
                        targetQuestion = ModelHelper.GetQuestionById(cond.dest_question_id.ToString());

                        // reset target control
                        targetCtrl = null;
                        targetCtrl = WizardHelper.GetControlByQuestionId(_wizard, cond.dest_question_id.ToString(), senderQuestionGroup);


                        // count the number of conditions for each target question
                        int conditionsCount = conditions.Where(x => x.dest_question_id == cond.dest_question_id && x.action_id != (int)Condition.Finish).Count();
                        
                        for (int i = 0; i < conditionsCount && !conditionSatisfied; ++i)
                        {
                            // condition obj
                            var condition = conditions.Where(x => x.dest_question_id.ToString() == cond.dest_question_id.ToString().Substring(0, 36) && x.action_id != (int)Condition.Finish).ElementAt(i);
                            conditionSatisfied = false;

                            switch (condition.action_id)
                            {
                                case (int)Condition.Show:
                                    if (targetCtrl != null)
                                    {
                                        conditionSatisfied = selectedValue == condition.source_response_choice_id;
                                        targetCtrl.Visible = conditionSatisfied;
                                        List<Questions> subQuestions = ModelHelper.GetSubquestions(targetQuestion);
                                        foreach (var sq in subQuestions)
                                        {
                                            var sqControl = WizardHelper.GetControlByQuestionId(_wizard, sq.question_id.ToString(), senderQuestionGroup);
                                            if (!(conditionSatisfied) && sqControl != null)
                                            {
                                                sqControl.Visible = false;
                                                WizardHelper.ResetControlValue(sqControl);
                                            }
                                        }

                                        if (ModelHelper.QuestionHasCondition(condition.dest_question_id.ToString().Substring(0, 36)))
                                        {
                                            // add Event Handler if the question has conditions
                                            if (targetCtrl is WetDropDownList)
                                            {
                                                ((WetDropDownList)targetCtrl).SelectedIndexChanged += Ddl_Index_changed;
                                                ((WetDropDownList)targetCtrl).AutoPostBack = true;
                                            } else if (targetCtrl is UCMultipleDropdownList)
                                            {
                                                ((UCMultipleDropdownList)targetCtrl).WetDDL.SelectedIndexChanged += Ddl_Index_changed;
                                                ((UCMultipleDropdownList)targetCtrl).WetDDL.AutoPostBack = true;
                                            } else if (targetCtrl is WetRadioButtonList)
                                            {
                                                ((WetRadioButtonList)targetCtrl).SelectedIndexChanged += Ddl_Index_changed;
                                                ((WetRadioButtonList)targetCtrl).AutoPostBack = true;
                                            }

                                        }
                                    }
                                    break;
                                case (int)Condition.EnableResponseChoice:
                                    if (targetCtrl != null)
                                    {
                                        if (targetCtrl is WetDropDownList)
                                        {
                                            conditionSatisfied = selectedValue != condition.source_response_choice_id;
                                            foreach (ListItem item in (targetCtrl as WetDropDownList).Items)
                                            {
                                                if (item.Value == condition.dest_response_choice_id.ToString())
                                                {
                                                    if (conditionSatisfied)
                                                    {
                                                        item.Attributes.Remove("disabled");      
                                                    }                                                  
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case (int)Condition.ChangeAcceptanceStatus:

                                    
                                    if (selectedValue == condition.source_response_choice_id)
                                    {
                                        string responseAlias = ModelHelper.GetResponseChoices(questionSource.question_id).SingleOrDefault(x => x.response_choice_id == selectedValue).tagId;


                                        if (responseAlias == FormStatus.Accepted)
                                        {
                                            form.AcceptStatusId = (int)IJPCodes.FormStatusCode.Accepted;
                                        } else if (responseAlias == FormStatus.NotAccepted)
                                        {
                                            form.AcceptStatusId = (int)IJPCodes.FormStatusCode.NotAccepted;
                                        }
                                        else if (responseAlias == FormStatus.PendingInformation)
                                        {
                                            form.AcceptStatusId = (int)IJPCodes.FormStatusCode.PendingInformation;
                                        }

                                        conditionSatisfied = true;

                                    } else
                                    {
                                        form.AcceptStatusId = null;
                                    }


                                    break;
                                case (int)Condition.ChangeCompletionStatus:


                                    if (selectedValue == condition.source_response_choice_id)
                                    {
                                        string responseAlias = ModelHelper.GetResponseChoices(questionSource.question_id).SingleOrDefault(x => x.response_choice_id == selectedValue)?.tagId;


                                        if (responseAlias == FormStatus.Completed)
                                        {
                                            form.ComplStatusId = (int)IJPCodes.FormStatusCode.Completed;
                                        }
                                        else if (responseAlias == FormStatus.NotCompleted)
                                        {
                                            form.ComplStatusId = (int)IJPCodes.FormStatusCode.NotCompleted;
                                        }
                                        else if (responseAlias == FormStatus.Ongoing)
                                        {
                                            form.ComplStatusId = (int)IJPCodes.FormStatusCode.Ongoing;
                                        }

                                        conditionSatisfied = true;
                                    } else
                                    {
                                        form.ComplStatusId = null;
                                    }
                                    break;
                            }

                            // clear user input/selection before hide
                            // hide only if none of the conditions is satisfied and the action is "Show"
                            if (!conditionSatisfied && condition.action_id == (int)Condition.Show && i == conditionsCount - 1)
                            {
                                WizardHelper.ResetControlValue(targetCtrl);
                            }
                        }
                    }
                    form.Save();
                }
            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

        public void UcMultipleDDL_AddDDL(object sender, AddDDLEventArgs e)
        {
            
            string questionSourceId = e.UC.WetDDL.ID;
            Questions questionSource = ModelHelper.GetQuestionById(questionSourceId);
            var activeStep = _wizard.ActiveStep; // current step
            bool questionHasCondition = ModelHelper.QuestionHasCondition(questionSource.question_id.ToString());
            var GROUP = Guid.NewGuid();
            string questionNote = String.Format("<span class=\"glyphicon glyphicon-question-sign help text-primary\"></span></br><span class=\"questionHint\">{0}</span>", questionSource.Note);
            var responsesChoice = questionSource.ReponseChoices.Where(x => !x.isObsolete).OrderBy(x => x.ordre).ToList();
            var responsesChoiceToDisable = questionSource.ReponseChoices.Where(x => x.disabled);

            UCRemovableDropdownList DDL = (UCRemovableDropdownList)CurrentPage.LoadControl("~/UserControls/UCRemovableDropdownList.ascx");
            DDL.wetDDL.LabelText = questionSource.Label + questionNote;
            DDL.wetRemoveDDL.ToolTip = "Remove " + questionSource.Label;
            DDL.wetDDL.DataSource = responsesChoice;
            DDL.wetDDL.DataValueField = "response_choice_id";
            DDL.wetDDL.DataTextField = "Text";
            DDL.wetDDL.IsRequired = questionSource.IsMandatory;

            // increment the question group
            DDL.wetDDL.ID = WizardHelper.IncrementQuestionIdGuid(_wizard, questionSource.question_id.ToString());
            DDL.wetDDL.SelectedIndexChanged += Ddl_Index_changed;
            DDL.wetDDL.AutoPostBack = true;
            DDL.wetDDL.DataBind();
            DDL.RemoveDDLClick += UcRemovableDDL_RemoveDDL;

            DDL.ID = DDL.wetDDL.ID;
            DDL.wetDDL.CssClass = "group_" + GROUP;

            foreach (var responseChoice in responsesChoiceToDisable)
            {
                var dropdownItem = DDL.wetDDL.Items.FindByValue(responseChoice.response_choice_id.ToString());
                if (dropdownItem != null)
                {
                    dropdownItem.Attributes.Add("disabled", "disabled");
                }
            }

            var senderIndex = activeStep.Controls.IndexOf(e.UC);

            senderIndex++;
            activeStep.Controls.AddAt(senderIndex, DDL);

            ViewStateControls.Add(new ViewStateControl()
            {
                Id = DDL.wetDDL.ID,
                StepIndex = _wizard.ActiveStepIndex,
                Type = DDL.GetType().Name,
                LabelText = DDL.wetDDL.LabelText,
                ToolTip = questionSource.Label,
                DataSource = DDL.wetDDL.DataSource,
                DisabledResponsesChoice = responsesChoiceToDisable.ToList(),
                HasCondition = questionHasCondition,
                IsRequired = DDL.wetDDL.IsRequired,
                Visible = DDL.wetDDL.Visible,
                CssClass = DDL.wetDDL.CssClass,
                Index = senderIndex
            });
            

            List<IWet> subQuestionsControls = WizardHelper.GetSubQuestionsControls(DDL.wetDDL.ID);
            

            foreach (var control in subQuestionsControls)
            {
                control.CssClass = DDL.wetDDL.CssClass;
                senderIndex++;
                activeStep.Controls.AddAt(senderIndex, control as Control);

                responsesChoice = ModelHelper.GetQuestionById(control.ID).ReponseChoices.Where(x => !x.isObsolete).ToList();
                responsesChoiceToDisable = responsesChoice.Where(x => x.disabled);

                ViewStateControls.Add(new ViewStateControl()
                {
                    Id = control.ID,
                    Visible = control.Visible,
                    StepIndex = _wizard.ActiveStepIndex,
                    Type = control.GetType().Name,
                    LabelText = control.LabelText,
                    //ToolTip = control., To fix
                    IsDate = control.GetType().Name.ToLower() == "wettextbox" ? (control as WetTextBox).IsDate : false,
                    DataSource = responsesChoice,
                    DisabledResponsesChoice = responsesChoiceToDisable.ToList(),
                    IsRequired = control.IsRequired,
                    CssClass = DDL.wetDDL.CssClass,
                    Index = senderIndex
                });
            }
        }

        public void UcRemovableDDL_RemoveDDL(object sender, RemoveDDLEventArgs e)
        {
            var activeStep = _wizard.ActiveStep; // current step
            var incrementNumber = e.UC.wetDDL.ID.Substring(36, e.UC.wetDDL.ID.Length - 36);
            var senderQuestion = ModelHelper.GetQuestionById(e.UC.wetDDL.ID.Substring(0, 36));
            var subQuestionsIds = ModelHelper.GetSubquestions(senderQuestion).Select(x => x.question_id);
            foreach(var subQuestion in subQuestionsIds)
            {
                ViewStateControls.Remove(ViewStateControls.SingleOrDefault(x => x.Id == subQuestion + incrementNumber));
                activeStep.Controls.Remove(activeStep.FindControl(subQuestion + incrementNumber));
            }
            activeStep.Controls.Remove(e.UC);
            ViewStateControls.Remove(ViewStateControls.SingleOrDefault(x => x.Id == e.UC.wetDDL.ID));
            WizardHelper.SaveWizardResponses(_wizard, GetCurrentPageViewState()["form"] as Forms, false);
        }

        public void TriggerWizardEvents(Guid? formId)
        {
            if (_wizard != null)
            {
                foreach (WizardStepBase step in _wizard.WizardSteps)
                {
                    foreach (var control in step.Controls) //Add other controls
                    {
                        if (control is WetDropDownList || control is WetRadioButtonList)
                        {
                            this.Ddl_Index_changed(control, EventArgs.Empty);
                        } else if (control is UCMultipleDropdownList)
                        {
                            this.Ddl_Index_changed((control as UCMultipleDropdownList).WetDDL, EventArgs.Empty);
                        } else if(control is UCRemovableDropdownList)
                        {
                            this.Ddl_Index_changed((control as UCRemovableDropdownList).wetDDL, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        
        public void Wizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            try
            {
                if (_wizard != null && (GetCurrentPageViewState()["form"] as Forms) != null && !ReadOnly)
                {
                    WizardHelper.SaveWizardResponses(_wizard, (GetCurrentPageViewState()["form"] as Forms), true);
                }
                else
                {
                    //blabla.. error
                }

            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

        public void Wizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            try
            {
                _wizard.ActiveStepIndex--;

                if (_wizard != null && (GetCurrentPageViewState()["form"] as Forms) != null && !ReadOnly)
                {
                    WizardHelper.SaveWizardResponses(_wizard, (GetCurrentPageViewState()["form"] as Forms), false);
                }
                else
                {
                    //blabla.. error
                }

            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

        public void Wizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {

            try
            {
                if (_wizard != null && (GetCurrentPageViewState()["form"] as Forms) != null && !ReadOnly)
                {
                    WizardHelper.SaveWizardResponses(_wizard, (GetCurrentPageViewState()["form"] as Forms), false);
                }
                else
                {
                    //blabla.. error
                }

            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

        //public string DownloadReferral(Wizard wizard)
        //{
        //    var csv = new StringBuilder();

           
        //    foreach (WizardStepBase step in wizard.WizardSteps)
        //    {
        //        foreach(var control in step.Controls)
        //        {
        //            if (control is WetDropDownList)
        //            {
        //                if((control as WetDropDownList).Visible)
        //                {
        //                    csv.AppendLine(String.Format("{0},{1}", (control as WetDropDownList).LabelText, (control as WetDropDownList).SelectedItem.Text));
        //                }
        //            }
        //            else if (control is WetCheckBoxList)
        //            {

        //                foreach (ListItem item in (control as WetCheckBoxList).Items)
        //                {
        //                    if (item.Selected)
        //                    {
                                
        //                    }
        //                }
        //            }
        //            else if (control is WetTextBox)
        //            {
                        
        //            }
        //            else if (control is WetRadioButtonList)
        //            {
                        
        //            }
        //            else if (control is UCMultipleDropdownList)
        //            {
                        
        //            }
        //            else if (control is UCRemovableDropdownList)
        //            {
                        
        //            }
        //        }
        //    }

        //    return csv.ToString();
        //}
    }
}