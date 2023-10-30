using IJPReporting.Code;
using IJPReporting.Helpers;
using IJPReporting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IJPReporting
{
    /// <summary>
    /// Description résumée de API
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Pour autoriser l'appel de ce service Web depuis un script à l'aide d'ASP.NET AJAX, supprimez les marques de commentaire de la ligne suivante. 
    [System.Web.Script.Services.ScriptService]
    public class API : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public void GetForms(object parameters)
        {
            DataTableParameters dtp = DataTableParameters.Get(parameters);

            int total = 0;
            int filetered = 0;
            string direction = dtp.Order.Values.First().Direction;  // direction du trie (ASC ou DESC)
            int column = dtp.Order.Values.First().Column;  // index de la colonne à trier
            string searchedValue = dtp.SearchValue.Trim().ToLower(); // search
            string lang = dtp.Language; // lang
            int serviceTypeId = dtp.ServiceTypeId;
            int acceptanceStatusId = dtp.AcceptanceStatusId;
            int completionStatusId = dtp.CompletionStatusId;
            string startRefDate = dtp.FromRefDate;
            string endRefDate = dtp.ToRefDate;
            int regionId = dtp.RegionId;
            int communityId = dtp.CommunityId;
            int programId = dtp.ProgramId;

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            IQueryable<Models.Forms> query;

            List<Models.Forms> forms;

            // set culture
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(lang == "fr" ? "fr-CA" : "en-CA");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;

            try
            {
                HttpContext ctx = HttpContext.Current;
                var currentUserId = ctx?.User?.Identity?.GetUserId();

                var currentUser = manager.FindById(User.Identity.GetUserId());
                var currentUserRole = currentUser.Roles?.First()?.RoleId;
                var currentUserRegion = currentUser.RegionsIds;
                var currentUserProgram = currentUser.ProgramId;
                var currentUserCommunity = currentUser.CommunityId;
                

                using (var context = new Models.BD_IJPReportingEntities())
                {
                    if(currentUserRole == RolesCode.Admin)
                    {
                        query = context.Forms.Include("Form_Type").Include("UserResponses.Questions").Include("Status1").Include("Status2").Include("AspNetUsers").Where(x => x.isDisabled == false && x.isObsolete == false).AsQueryable();

                    }
                    else if (currentUserRole == RolesCode.Program)
                    {
                        query = context.Forms.Include("Form_Type").Include("UserResponses.Questions").Include("Status1").Include("Status2").Include("AspNetUsers").Where(x => x.isDisabled == false && x.isObsolete == false && (x.program_id == currentUserProgram && x.community_id == currentUserCommunity)).AsQueryable();

                    } else if(currentUserRole == RolesCode.RecipientUmbOrg)
                    {
                        query = context.Forms.Include("Form_Type").Include("UserResponses.Questions").Include("Status1").Include("Status2").Include("AspNetUsers").Where(x => x.isDisabled == false && x.isObsolete == false && (x.community_id == currentUserCommunity)).AsQueryable();
                    } else if(currentUserRole == RolesCode.RecipientPT || currentUserRole == RolesCode.IJPRegionalCoordinator)
                    {
                        query = context.Forms.Include("Form_Type").Include("UserResponses.Questions").Include("Status1").Include("Status2").Include("AspNetUsers").Where(x => x.isDisabled == false && x.isObsolete == false && (x.Regions.Any(y => currentUserRegion.Contains(y.region_id)))).AsQueryable();
                    }
                    else
                    {
                        query = context.Forms.Include("Form_Type").Include("UserResponses.Questions").Include("Status1").Include("Status2").Include("AspNetUsers").Where(x => x.isDisabled == false && x.isObsolete == false && x.creator_id == currentUserId).AsQueryable();
                    }

                    //service type
                    if(serviceTypeId > 0)
                    {
                        query = query.Where(x => x.FormType_id == serviceTypeId);
                    }

                    //acceptance status
                    if (acceptanceStatusId > 0)
                    {
                        query = query.Where(x => x.AcceptStatusId == acceptanceStatusId);
                    }

                    //completion status
                    if (completionStatusId > 0)
                    {
                        query = query.Where(x => x.ComplStatusId == completionStatusId);
                    }


                    //start referral date
                    if (!String.IsNullOrEmpty(startRefDate) && startRefDate != "0")
                    {
                        DateTime.TryParse(startRefDate, out DateTime parsedStartRefDate);
                        query = query.Where(x => x.RefDate >= parsedStartRefDate);
                    }

                    //end referral date
                    if (!String.IsNullOrEmpty(endRefDate) && endRefDate != "0")
                    {
                        DateTime.TryParse(endRefDate, out DateTime parsedEndRefDate);
                        parsedEndRefDate.AddDays(1).AddTicks(-1);
                        query = query.Where(x => x.RefDate <= parsedEndRefDate);
                    }

                    //region
                    if (regionId > 0)
                    {
                        query = query.Where(x => x.Regions.Any(y => y.region_id == regionId));
                    }

                    //community
                    if (communityId > 0)
                    {
                        query = query.Where(x => x.community_id == communityId);
                    }

                    //program
                    if (programId > 0)
                    {
                        query = query.Where(x => x.program_id == programId);
                    }

                    ////archive
                    //if (true) //not archive
                    //{
                    //    query = query.Where(x => x.ComplStatusId != (int)IJPCodes.FormStatusCode.Completed && x.ComplStatusId != (int)IJPCodes.FormStatusCode.NotCompleted);
                    //}

                    total = query.Count();

                    if (!string.IsNullOrEmpty(searchedValue))
                    {
                        query = query.Where(x => (lang == "fr" ? x.Form_Type.FormTitle_fr.ToLower().Contains(searchedValue) : x.Form_Type.FormTitle_en.ToLower().Contains(searchedValue) ) 
                                                || x.ClientFileNumberId.ToLower().Contains(searchedValue)
                                                || x.UserResponses.Where(y => y.Questions.TagId == "ref_dat").FirstOrDefault().devlopment.Contains(searchedValue)
                                                || (lang == "fr" ? x.Status2.Status_name_fr.ToLower().Contains(searchedValue) : x.Status2.Status_name_en.ToLower().Contains(searchedValue))
                                                || (lang == "fr" ? x.Status1.Status_name_fr.ToLower().Contains(searchedValue) : x.Status1.Status_name_en.ToLower().Contains(searchedValue))
                                                || x.AspNetUsers.LastName.ToLower().Contains(searchedValue) || x.AspNetUsers.FirstName.ToLower().Contains(searchedValue));
                    }

                    switch (column)
                    {
                        case 0:
                            query = query.OrderByDescending(x => x.date_creation);
                            break;
                        case 1:
                            query = direction == "asc" ? query.OrderByDescending(x => lang == "fr" ? x.Form_Type.FormTitle_fr : x.Form_Type.FormTitle_en) : query.OrderBy(x => lang == "fr" ? x.Form_Type.FormTitle_fr : x.Form_Type.FormTitle_en);
                            break;
                        case 2:
                            query = direction == "asc" ? query.OrderByDescending(x => x.ClientFileNumberId) : query.OrderBy(x => x.ClientFileNumberId);
                            break;
                        case 3:
                            query = direction == "asc" ? query.OrderByDescending(x => x.UserResponses.Where(y => y.Questions.TagId == "ref_dat").FirstOrDefault().devlopment) : query.OrderBy(x => x.UserResponses.Where(y => y.Questions.TagId == "ref_dat").FirstOrDefault().devlopment);
                            break;
                        case 4:
                            query = direction == "asc" ? query.OrderByDescending(x => lang == "fr" ? x.Status2.Status_name_fr : x.Status2.Status_name_en) : query.OrderBy(x => lang == "fr" ? x.Status2.Status_name_fr : x.Status2.Status_name_en);
                            break;
                        case 5:
                            query = direction == "asc" ? query.OrderByDescending(x => lang == "fr" ? x.Status1.Status_name_fr : x.Status1.Status_name_en) : query.OrderBy(x => lang == "fr" ? x.Status1.Status_name_fr : x.Status1.Status_name_en);
                            break;
                        case 6:
                            query = direction == "asc" ? query.OrderByDescending(x => x.AspNetUsers.LastName)  : query.OrderBy(x => x.AspNetUsers.LastName);
                            break;
                    }


                    // count des données filtrés
                    filetered = query.Count();

                    // paging
                    query = query.Skip(dtp.Start).Take(dtp.Length);

                    forms = query.ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }

            var resultSet = new DataTableResultSet()
            {
                draw = dtp.Draw,
                recordsTotal = total,/* total number of records in table */
                recordsFiltered = filetered/* number of records after search - box filtering is applied */
            };
            List<string> columns;

            foreach(Models.Forms form in forms)
            {
                columns = new List<string>
                {
                    form.form_id.ToString(),
                    form.FormName,
                    form.ClientFileNumberId,
                    form.RefDate.ToShortDateString(),
                    !String.IsNullOrEmpty(form.Status2?.StatusName) ? form.Status2?.StatusName : "N/A", // Acceptance
                    !String.IsNullOrEmpty(form.Status1?.StatusName) ? form.Status1?.StatusName : "N/A", // Completion,
                    manager.FindById(form.creator_id.ToString()).FirstName + ", " + manager.FindById(form.creator_id.ToString()).LastName,
                    ""
                };

                resultSet.data.Add(columns);
            }
            SendResponse(HttpContext.Current.Response, resultSet);
        }

        private void SendResponse(HttpResponse response, DataTableResultSet result)
        {
            response.Clear();
            response.Headers.Add("X-Content-Type-Options", "nosniff");
            response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            response.ContentType = "application/json; charset=utf-8";
            response.Write(result.ToJSON());
            response.Flush();
            response.End();
        }

        private void SendResponse(HttpResponse response, CsvResultSet result)
        {
            response.Clear();
            response.Headers.Add("X-Content-Type-Options", "nosniff");
            response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            response.ContentType = "application/json; charset=utf-8";
            response.Write(result.ToJSON());
            response.Flush();
            response.End();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public string UpdateCFN(string formId, string cfn)
        {
            Guid formID = new Guid(formId);
            if(formID != Guid.Empty && !String.IsNullOrEmpty(cfn))
            {
                ModelHelper helper = new ModelHelper();
                Models.Forms form = helper.GetFormById(new Guid(formId));
                form.ClientFileNumberId = cfn;
                return form.Save() ? JsonConvert.SerializeObject("true") : JsonConvert.SerializeObject("false");
            }
            return JsonConvert.SerializeObject("false");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public string UpdateRefDate(string formId, string refDate)
        {
            Guid formID = new Guid(formId);
            if (formID != Guid.Empty && !String.IsNullOrEmpty(refDate))
            {
                ModelHelper helper = new ModelHelper();
                Models.Forms form = helper.GetFormById(new Guid(formId));
                DateTime.TryParse(refDate, out DateTime parsedRefDate);
                form.RefDate = parsedRefDate;
                return form.Save() ? JsonConvert.SerializeObject("true") : JsonConvert.SerializeObject("false");
            }
            return JsonConvert.SerializeObject("false");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public void DownloadReferral(string refId)
        {
            ModelHelper helper = new ModelHelper();
            Forms form = helper.GetFormById(refId);
            var csv = new StringBuilder();

            foreach (var section in form.Form_Type.Section.Where(x => !x.IsObsolete).OrderBy(x => x.Ordre))
            {
                foreach(var question in section.Questions.Where(x => x.FormTypeId == form.FormType_id && !x.isObsolete).OrderBy(x => x.NoQuestion))
                {
                    var responses = question.UserResponses.Where(x => x.Form_Id == form.form_id);
                    var response = responses.Count() > 0 ? question.UserResponses.Where(x => x.Form_Id == form.form_id).First() : null;
                    var responseChoice = response != null ? helper.GetReponseChoice(response.response_choice_id): null;
                    string responseChoiceLabel = "";

                    if(response != null && !String.IsNullOrEmpty(response.devlopment))
                    {
                        responseChoiceLabel = response.devlopment;
                    } else if(response != null && responseChoice != null)
                    {
                        responseChoiceLabel = responseChoice.Text;
                    }else
                    {
                        responseChoiceLabel = "N/A";
                    }
                    csv.AppendLine(String.Format("{0}{2}{1}", question.Label, responseChoiceLabel, Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "fr" ? ";" : ","));
                }
            }
            //return csv.ToString();
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ContentType = "text/csv";
            //HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.csv", form.FormName));
            //HttpContext.Current.Response.Write(csv);
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();

            var resultSet = new CsvResultSet()
            {
                data = csv.ToString(),
                fileName = form.FormName + ".csv"
            };

            //var csvObj = new Dictionary<string, string>();
            //csvObj.Add("csv", csv.ToString());
            //csvObj.Add("fileName", form.FormName);

            //return JsonConvert.SerializeObject(resultSet);
            SendResponse(HttpContext.Current.Response, resultSet);
        }
    }
}
