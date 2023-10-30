using IJPReporting.Managers;
using IJPReporting.Models;
using IJPReporting.UserControls;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WetControls.Controls;
using WetControls.Interfaces;
using static IJPReporting.Helpers.IJPCodes;

namespace IJPReporting.Helpers
{
    [Serializable]
    public class WizardHelper
    {
        ModelHelper ModelHelper = new ModelHelper();
        /// <summary>
        /// This function increment the group of a question.
        /// </summary>
        /// <param name="wizard"></param>
        /// <param name="questionId">the id of question to increment</param>
        /// <returns></returns>
        public string IncrementQuestionIdGuid(Wizard wizard, string questionId)
        {
            try
            {
                var id = String.Empty;
                for (int i = 0; i < wizard.WizardSteps.Count; ++i)
                {
                    var step = wizard.WizardSteps[i];

                    var last_question = step.Controls.OfType<Control>()
                                .Where(x => x is UCRemovableDropdownList ? ((UCRemovableDropdownList)x).wetDDL.ID.Substring(0, 36) == questionId : x.ID.Substring(0, 36) == questionId)
                                .OrderBy(x => x is UCRemovableDropdownList ? ((UCRemovableDropdownList)x).wetDDL.ID.Substring(36, ((UCRemovableDropdownList)x).wetDDL.ID.Length - 36) : x.ID.Substring(36, x.ID.Length - 36))
                                .Select(x => x is UCRemovableDropdownList ? ((UCRemovableDropdownList)x).wetDDL : x)
                                .LastOrDefault();
                    if (last_question != null)
                    {
                        if (last_question.ID.Length == 36)
                        {
                            id = String.Format("{0}_{1}", last_question.ID, 1);
                        }
                        else
                        {
                            var ind = last_question.ID.IndexOf("_") + 1;
                            Int32.TryParse(last_question.ID.Substring(ind, last_question.ID.Length - ind), out int integer);
                            if (integer > 0)
                            {
                                id = String.Format("{0}_{1}", questionId, integer + 1);

                            }
                            else
                            {
                                id = String.Format("{0}", questionId);
                            }
                        }
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
            return "";
        }

        public Control GetControlByQuestionId(Wizard wizard, string questionId, string questionGroup)
        {
            try
            {
                for (int j = 0; j < wizard.WizardSteps.Count; ++j)
                {
                    WizardStepBase step = wizard.WizardSteps[j];

                    var control = step.Controls.OfType<Control>().SingleOrDefault(x => x.ID.ToLower() == (questionId.ToLower() + questionGroup));

                    if (control != null)
                    {
                        return control;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
            return null;
        }

        public void ResetControlValue(Control ctrl)
        {
            try
            {
                if (ctrl != null)
                {
                    if (ctrl is WetTextBox)
                    {
                        ((WetTextBox)ctrl).Text = null;
                    }
                    else if (ctrl is WetRadioButtonList)
                    {
                        ((WetRadioButtonList)ctrl).ClearSelection();
                    }
                    else if (ctrl is WetDropDownList)
                    {
                        ((WetDropDownList)ctrl).ClearSelection();
                    }
                    else if (ctrl is WetCheckBoxList)
                    {
                        ((WetCheckBoxList)ctrl).ClearSelection();
                    }
                    else if (ctrl is UCMultipleDropdownList)
                    {
                        ((UCMultipleDropdownList)ctrl).WetDDL.ClearSelection();
                    }
                    else if (ctrl is UCRemovableDropdownList)
                    {
                        ((UCRemovableDropdownList)ctrl).wetDDL.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }

        }

        public void ResetControlValue(List<Control> ctrls)
        {
            try
            {
                if (ctrls != null)
                {
                    foreach (var ctrl in ctrls)
                    {
                        ResetControlValue(ctrl);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

        public void SaveWizardResponses(Wizard wizard, Forms form, bool finish)
        {
            try
            {
                using (var context = new BD_IJPReportingEntities())
                {
                    UserResponses response = new UserResponses();
                    List<UserResponses> responses = new List<UserResponses>();
                    Forms formObj = ModelHelper.GetFormById(form.form_id);
                    if (form != null && wizard != null)
                    {
                        context.Forms.Attach(formObj);
                        context.UserResponses.RemoveRange(formObj.UserResponses); // clear all response before saving again
                        context.SaveChanges();
                        foreach (WizardStep step in wizard.WizardSteps)
                        {
                            foreach (var control in step.Controls)
                            {
                                string question_id = ((Control)control).ID.Substring(0, 36);
                                if (question_id == "31d7ba05-342a-4482-95f5-b10f01705a91")
                                {

                                }
                                string question_group = ((Control)control).ID.Substring(37, ((Control)control).ID.Length - 37);
                                response = new UserResponses(); ;
                                responses = new List<UserResponses>();

                                if (control is WetDropDownList)
                                {
                                    response = new UserResponses
                                    {
                                        response_id = Guid.NewGuid(),
                                        question_id = new Guid(question_id),
                                        response_choice_id = ((WetDropDownList)control).SelectedValue != "" ? new Guid(((WetDropDownList)control).SelectedValue) : Guid.Empty,
                                        Form_Id = form.form_id,
                                    };
                                }
                                else if (control is WetCheckBoxList)
                                {
                                    
                                    foreach(ListItem item in (control as WetCheckBoxList).Items)
                                    {
                                        if (item.Selected)
                                        {
                                            responses.Add(new UserResponses
                                            {
                                                response_id = Guid.NewGuid(),
                                                question_id = new Guid(question_id),
                                                response_choice_id = new Guid(item.Value),
                                                Form_Id = form.form_id,
                                                question_group = Int32.Parse(question_group)
                                            });
                                        }
                                    }
                                }
                                else if (control is WetTextBox)
                                {
                                    response = new UserResponses
                                    {
                                        response_id = Guid.NewGuid(),
                                        question_id = new Guid(question_id),
                                        devlopment = ((WetTextBox)control).Text,
                                        Form_Id = form.form_id
                                    };
                                }
                                else if (control is WetRadioButtonList)
                                {
                                    response = new UserResponses
                                    {
                                        response_id = Guid.NewGuid(),
                                        question_id = new Guid(question_id),
                                        response_choice_id = ((WetRadioButtonList)control).SelectedValue != "" ? new Guid(((WetRadioButtonList)control).SelectedValue) : Guid.Empty,
                                        Form_Id = form.form_id
                                    };
                                }
                                else if (control is UCMultipleDropdownList)
                                {
                                    response = new UserResponses
                                    {
                                        response_id = Guid.NewGuid(),
                                        question_id = new Guid(question_id),
                                        response_choice_id = ((UCMultipleDropdownList)control).WetDDL.SelectedValue != "" ? new Guid(((UCMultipleDropdownList)control).WetDDL.SelectedValue) : Guid.Empty,
                                        Form_Id = form.form_id
                                    };
                                }
                                else if (control is UCRemovableDropdownList)
                                {
                                    response = new UserResponses
                                    {
                                        response_id = Guid.NewGuid(),
                                        question_id = new Guid(question_id),
                                        response_choice_id = ((UCRemovableDropdownList)control).wetDDL.SelectedValue != "" ? new Guid(((UCRemovableDropdownList)control).wetDDL.SelectedValue) : Guid.Empty,
                                        Form_Id = form.form_id
                                    };
                                }

                                if (response != null && response.response_id != Guid.Empty)
                                {
                                    response.question_group = Int32.Parse(question_group);
                                    context.UserResponses.Add(response);
                                    context.SaveChanges();
                                }

                                if (responses.Count > 0)
                                {
                                    context.UserResponses.AddRange(responses);
                                    context.SaveChanges();
                                }
                            }
                        }
                        if (finish)
                        {
                            formObj.isFinal = true;
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionIJP error = new ExceptionIJP();
                error.HandleException(ex);
            }
        }

        public List<IWet> GetSubQuestionsControls(string questionId)
        {
            string questionGroup = questionId.Substring(36, questionId.Length - 36);
            List<IWet> subQuestionsControls = new List<IWet>();
            Questions qst = ModelHelper.GetQuestionById(questionId);
            List<Questions> subQuestions = ModelHelper.GetSubquestions(qst);

            foreach (var question in subQuestions)
            {
                var questionType = question.question_type_id;

                switch (questionType)
                {
                    case (int)QuestionType.TextBox:
                        subQuestionsControls.Add(new WetTextBox
                        {
                            LabelText = question.Label,
                            ID = question.question_id + questionGroup,
                            Visible = !question.IsConditional,
                            IsRequired = question.IsMandatory
                        });
                        break;
                    case (int)QuestionType.DatePicker:
                        subQuestionsControls.Add(new WetTextBox
                        {
                            LabelText = question.Label,
                            ID = question.question_id + questionGroup,
                            Visible = !question.IsConditional,
                            IsDate = true,
                            IsRequired = question.IsMandatory

                        });
                        break;
                    case (int)QuestionType.RadioButtonList:
                        WetRadioButtonList rbl = new WetRadioButtonList
                        {
                            LabelText = question.Label,
                            ID = question.question_id + questionGroup,
                            Visible = !question.IsConditional,
                            DataSource = question.ReponseChoices.OrderBy(x => x.ordre).ToList(),
                            DataValueField = "response_choice_id",
                            DataTextField = "text_en",
                            IsRequired = question.IsMandatory
                        };
                        rbl.DataBind();
                        subQuestionsControls.Add(rbl);
                        break;
                    case (int)QuestionType.CheckBoxList:
                        WetCheckBoxList cbl = new WetCheckBoxList
                        {
                            LabelText = question.Label,
                            ID = question.question_id + questionGroup,
                            Visible = !question.IsConditional,
                            DataSource = question.ReponseChoices.OrderBy(x => x.ordre).ToList(),
                            DataValueField = "response_choice_id",
                            DataTextField = "text_en",
                            IsRequired = question.IsMandatory
                        };
                        cbl.DataBind();
                        subQuestionsControls.Add(cbl);
                        break;
                    case (int)QuestionType.DropdownList:
                        WetDropDownList ddl = new WetDropDownList
                        {
                            LabelText = question.Label,
                            ID = question.question_id + questionGroup,
                            Visible = !question.IsConditional,
                            DataSource = question.ReponseChoices.OrderBy(x => x.ordre).ToList(),
                            DataValueField = "response_choice_id",
                            DataTextField = "text_en",
                            IsRequired = question.IsMandatory
                        };
                        ddl.DataBind();
                        subQuestionsControls.Add(ddl);
                        break;
                }
            }
            return subQuestionsControls;
        }

        public int? GetQuestionStepIndex(Wizard wizard, string questionId)
        {
            try
            {
                int currentStep = 0;
                Questions question = ModelHelper.GetQuestionById(questionId);
                int questionTypeId = !String.IsNullOrEmpty(questionId) && questionId != Guid.Empty.ToString() && question != null ? question.question_type_id : -1;
                for (int j = 0; j < wizard.WizardSteps.Count; ++j)
                {
                    WizardStepBase step = wizard.WizardSteps[j];
                    if (questionTypeId == (int)IJPCodes.QuestionType.MultipleDropdownList)
                    {
                        for (int i = 0; i < step.Controls.OfType<UCMultipleDropdownList>().Count(); ++i)
                        {
                            UCMultipleDropdownList control = step.Controls.OfType<UCMultipleDropdownList>().ElementAt(i);
                            if (control.WetDDL.ID == questionId)
                            {
                                return currentStep;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < step.Controls.Count; ++i)
                        {
                            Control control = step.Controls[i];
                            if (control.ID == questionId)
                            {
                                return currentStep;
                            }
                        }
                    }

                    currentStep++;
                }
                return null;
            }
            catch
            {
                throw;
            }

        }

        public int? GetLastStepIndex(Wizard wizard, Guid? formId)
        {
            if (wizard != null && formId != Guid.Empty)
            {
                var form = ModelHelper.GetFormById(formId);
                for (int j = 0; j < wizard.WizardSteps.Count; ++j)
                {
                    WizardStepBase step = wizard.WizardSteps[j];
                    var controls = step.Controls.OfType<WetDropDownList>(); // ???
                    foreach (var control in step.Controls.OfType<WetDropDownList>())
                    {
                        var conditions = ModelHelper.GetConditionsByQuestionId(new Guid(control.ID.Substring(0, 36))).Where(x => x.action_id == (int)IJPCodes.Condition.Finish).ToList();
                        foreach (var condition in conditions)
                        {
                            if (control.SelectedValue == condition.source_response_choice_id.ToString())
                            {
                                return j;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public bool DisableStep(Wizard wizard, Guid FormId)
        {
            int? lastStepIndex = this.GetLastStepIndex(wizard, FormId);
            if (lastStepIndex != null)
            {
                for(int i = lastStepIndex.Value + 1; i < wizard.WizardSteps.Count; ++i)
                {
                    WizardStepBase step = wizard.WizardSteps[i];
                    foreach(var control in step.Controls.OfType<Control>())
                    {
                        this.ResetControlValue(control);
                    }
                }
                wizard.WizardSteps[lastStepIndex.Value].StepType = WizardStepType.Finish;
                return true;
            } else
            {
                foreach (WizardStepBase step in wizard.WizardSteps)
                {
                    step.StepType = WizardStepType.Auto;

                }           
            }

            return false;
        }
    }
}