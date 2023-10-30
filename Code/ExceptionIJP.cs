using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Text;

namespace IJPReporting
{
    public class ExceptionIJP
    {
        private const string APP_NAME = "IJP Reporting";

        public ExceptionIJP() { }

        public void HandleException(Exception ex)
        {
            try
            {
                HttpContext ctx = HttpContext.Current;

                if (ctx.User != null && ctx.User.Identity.IsAuthenticated)
                {
                    var userManager = ctx.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    string email = userManager.GetEmailAsync(ctx?.User?.Identity?.GetUserId())?.Result;
                    string authName = ctx.Request.ServerVariables["AUTH_USER"].ToString();
                    string serveur = ctx.Request.ServerVariables["HTTP_HOST"].ToString();
                    string url = ctx.Request.ServerVariables["URL"].ToString();
                    string fichier = url.Substring(url.LastIndexOf("/") + 1);
                    string parametres = ctx.Request.ServerVariables["QUERY_STRING"].ToString();
                    string navigateur = ctx.Request.ServerVariables["HTTP_USER_AGENT"].ToString();
                    string IP = ctx.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    StringBuilder content = new StringBuilder();
                    content.Append("<b>Serveur: </b>");
                    content.Append(serveur);
                    content.Append("<br /><b>Application: </b>");
                    content.Append(APP_NAME);
                    content.Append("<br /><b>URL: </b>");
                    content.Append(url);
                    content.Append("<br /><b>Paramètres: </b>");
                    content.Append(parametres);
                    content.Append("<br /><b>Navigateur: </b>");
                    content.Append(navigateur);
                    content.Append("<br /><b>Source d'erreur: </b>");
                    content.Append(ex.TargetSite.ToString());
                    content.Append("<br /><b>Description: </b>");
                    content.Append(ex.Message);
                    content.Append("<br /><b>Trace: </b>");
                    content.Append(ex.StackTrace + "<br />" + ex.Source);

                    if (ex is System.Data.Entity.Validation.DbEntityValidationException)
                    {
                        var dbEntity = ex as System.Data.Entity.Validation.DbEntityValidationException;

                        // Retrieve the error messages as a list of strings.
                        var errorMessages = dbEntity.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        content.Append("<br /><b>Erreur de validation Entity: </b>");
                        content.Append(fullErrorMessage);
                    }

                    content.Append("<br /><b>Formulaire: </b>");
                    content.Append(ctx.Request.Form.ToString());
                    content.Append("<br /><br /><b>Usager: </b>");
                    content.Append(authName);
                    content.Append("<br /><b>Addresse IP: </b>");
                    content.Append(IP);
                    content.Append("<br /><br /><b>Date: </b>");
                    content.Append(DateTime.Now.ToString());


                    IdentityMessage identityMessage = new IdentityMessage();
                    identityMessage.Destination = "sweb@justice.gc.ca";
                    identityMessage.Subject = "Bug: " + APP_NAME;
                    identityMessage.Body = content.ToString();
                    //#if !DEBUG
                    userManager.EmailService.Send(identityMessage);
                    //#endif
                }
            } catch (Exception)
            {

            }   
        }
    }
}