using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Configuration;
using IJPReporting.Models;
using System.Collections.Generic;
using System.Linq;

namespace IJPReporting.Models
{
    // You can add User data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public partial class ApplicationUser : IdentityUser
    {
        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public DateTime LastPasswordChange { get; set; }
        public int? ProgramId { get; set; }
        public int? CommunityId { get; set; }

        //public virtual ICollection<Regions> Regions { get; set; }

        //public int test
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //}

        public List<int> RegionsIds
        {
            get
            {
                using (var context = new BD_IJPReportingEntities())
                {
                    //return context.UsersRegions.Where(w => w.userId == this.Id).Join(context.Regions, x => x.region_id, y => y.region_id, (x, y) => x.Regions).ToList();
                    return context.AspNetUsers.SingleOrDefault(x => x.Id == this.Id).Regions.Select(x => x.region_id).ToList();

                }
            }
        }

        public string RoleName
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    var currentUserRoleId = this.Roles.Count > 0 ? this.Roles.ElementAt(0).RoleId : "0";
                    var role = context.Roles.SingleOrDefault(x => x.Id == currentUserRoleId);
                    return role != null ? role.Name : String.Empty;
                }
            }
        }
    }

    

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}

#region Helpers
namespace IJPReporting
{
    public static class IdentityHelper
    {
        // Used for XSRF when linking external logins
        public const string XsrfKey = "XsrfId";

        public const string ProviderNameKey = "providerName";
        public static string GetProviderNameFromRequest(HttpRequest request)
        {
            return request.QueryString[ProviderNameKey];
        }

        public const string CodeKey = "code";
        public static string GetCodeFromRequest(HttpRequest request)
        {
            return request.QueryString[CodeKey];
        }

        public const string UserIdKey = "userId";
        public static string GetUserIdFromRequest(HttpRequest request)
        {
            return HttpUtility.UrlDecode(request.QueryString[UserIdKey]);
        }

        public static string GetResetPasswordRedirectUrl(string code, HttpRequest request)
        {
            var absoluteUri = "/" + ConfigurationManager.AppSettings["rootProjet"] + "/Account/ResetPassword?" + CodeKey + "=" + HttpUtility.UrlEncode(code);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        public static string GetUserConfirmationRedirectUrl(string code, string userId, HttpRequest request)
        {
            var absoluteUri = "/" + ConfigurationManager.AppSettings["rootProjet"] + "/Account/Confirm?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
            return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
        }

        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("~/");
            }
        }
    }
}
#endregion
