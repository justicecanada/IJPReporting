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

namespace IJPReporting.Helpers
{
    [Serializable]
    public class ModelHelper
    {
        public List<Form_Type> GetProgramsForms()
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Form_Type.ToList();
            }
        }

        public Form_Type GetFormTypeById(int? formTypeId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Form_Type.Include("Section.Questions.ReponseChoices").SingleOrDefault(x => x.FormTypeId == formTypeId);
            }
        }

        public Forms GetFormById(Guid? formId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Forms.Include("Form_Type.Section.Questions.ReponseChoices").Include("UserResponses").Include("Form_Type.Section.Questions.UserResponses").SingleOrDefault(x => x.form_id == formId);
            }
        }

        public Forms GetFormById(string fId)
        {
            Guid formId = new Guid(fId);
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Forms.Include("Form_Type.Section.Questions.ReponseChoices").Include("UserResponses").Include("Form_Type.Section.Questions.UserResponses").SingleOrDefault(x => x.form_id == formId);
            }
        }

        public Section GetSectionById(int? sectionId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Section.Include("Questions.ReponseChoices").SingleOrDefault(x => x.SectionId == sectionId);
            }
        }

        public Questions GetQuestionById(string questionId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                Guid qId = new Guid(questionId.Substring(0, 36));
                return context.Questions.Include("ReponseChoices").Include("Conditions").Include("SubQuestions").SingleOrDefault(x => x.question_id == qId);
            }
        }

        public Questions GetQuestionById(Guid questionId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Questions.Include("ReponseChoices").Include("Conditions").Include("SubQuestions").SingleOrDefault(x => x.question_id == questionId);
            }
        }

        public bool QuestionHasCondition(string questionId)
        {
            Guid qId = new Guid(questionId.Substring(0, 36));
            return this.GetQuestionById(questionId).Conditions.Where(x => !x.is_disabled).Any();
        }

        public List<Conditions> GetConditionsByQuestionId(Guid? questionId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Conditions.Where(x => x.source_question_id == questionId && !x.is_disabled).ToList();
            }
        }

        public List<Forms> GetFormsByUserId(string userId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Forms.Include("Form_Type").Where(x => x.creator_id == userId).OrderBy(x => x.date_creation).ToList();
            }
        }

        public string GetClientFileNumber(Guid? formId)
        {
            Forms form = this.GetFormById(formId);
            UserResponses fileNumberReponse = form.UserResponses.SingleOrDefault(x => x.question_id == new Guid("16F5FCA0-6561-431D-BAC3-E623FA3BEC3A"));
            return fileNumberReponse != null ? (!String.IsNullOrEmpty(fileNumberReponse.devlopment) ? fileNumberReponse.devlopment : String.Empty) : String.Empty;
        }

        public List<Guid> GetRelatedQuestionId(Guid? questionId)
        {
            return this.GetConditionsByQuestionId(questionId).DistinctBy(x => x.dest_question_id)
                        .Select(x => x.dest_question_id).ToList();
        }

        public List<Questions> GetSubquestions(Questions question)
        {
            List<Questions> questions = new List<Questions>();
            var subquestions = question.SubQuestions.OrderBy(x => this.GetQuestionById(x.SubQuestionId).NoQuestion);
            foreach (var qs in subquestions)
            {
                var qsObj = this.GetQuestionById(qs.SubQuestionId);
                questions.Add(qsObj);
                questions.AddRange(GetSubquestions(qsObj));
            }
            return questions;
        }

        public Files GetFileById(Guid fileId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.Files.SingleOrDefault(x => x.file_id == fileId);
            }
        }

        public List<ReponseChoices> GetResponseChoices(Guid questionId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.ReponseChoices.Where(x => x.question_id == questionId && !x.isObsolete && !x.disabled).ToList();
            }
        }

        public ReponseChoices GetReponseChoice(Guid? responseChoiceId)
        {
            using (var context = new BD_IJPReportingEntities())
            {
                return context.ReponseChoices.SingleOrDefault(x => x.response_choice_id == responseChoiceId);
            }
        }

        public List<Regions> GetRegionsByUser(ApplicationUser user)
        {
            List<Regions> regions = new List<Regions>();

            if (user != null)
            {
                List<int> regionIds = user?.RegionsIds;

                using (var context = new BD_IJPReportingEntities())
                {
                    foreach (var id in regionIds)
                    {
                        regions.Add(context.Regions.SingleOrDefault(x => x.region_id == id));
                    }
                }
            }
            return regions;
        }
    }
}