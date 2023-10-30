using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;


namespace GCWebUsabilityTheme.MasterPages
{
    public partial class GCWebUsability : System.Web.UI.MasterPage
    {

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WetBoewConfiguration config = WetBoewConfiguration.GetConfiguration();

            //Read the site title and home page from the custom web.config section.
            LanguagesCollection myLanguagesSection = config.Languages as LanguagesCollection;
            string lang = ((BasePage)Page).Language;

           
            for (int i = 0; i < myLanguagesSection.Count; i++)
            {
                if (myLanguagesSection[i].Abbr == lang)
                {
                    HyperLinkSiteTitle.Text = string.Format("<span>{0}</span>", myLanguagesSection[i].SiteName);
                    HyperLinkSiteTitle.NavigateUrl = myLanguagesSection[i].HomePage;
                    break;
                }
            }

            //Hide the breadcrumbs depending on the setting of WetBoewGroup/WetBoew/@showBreadcrumbTrail in web.config.
            bool showBreadcrumbTrail = config.ShowBreadcrumbTrail;

            if (!showBreadcrumbTrail)
            {
                Breadcrumb.Visible = false;
            }

            //Hide the search depending on the setting of WetBoewGroup/WetBoew/@showSearch in web.config.
            bool showSearch = config.ShowSearch;

            if (!showSearch)
            {
                Search.Visible = false;
            }

            //Show a section menu if required.
            if (((BasePage)Page).ShowSectionMenu)
            {
                container.CssClass = "container";
                row.CssClass = "row";
                main.Attributes["class"] = "col-md-9 col-md-push-3";


                GCWebUsabilityTheme.Controls.SectionMenu SectionMenu1 = (GCWebUsabilityTheme.Controls.SectionMenu)LoadControl("~/GCWebUsabilityTheme/Controls/SectionMenu.ascx") as GCWebUsabilityTheme.Controls.SectionMenu;
                row.Controls.Add(SectionMenu1);
            }

        }

        protected string GetSignatureImage()
        {
            string lang = ((BasePage)Page).Language;

            if (lang == "fr")
            {
                return ResolveUrl("~/GCWebUsabilityTheme/wet-v4/dist/assets/sig-fr.svg");

            }
            else
            {
                return ResolveUrl("~/GCWebUsabilityTheme/wet-v4/dist/assets/sig-en.svg");
            }
        }

        protected string GetProtectedText()
        {
            string lang = ((BasePage)Page).Language;

            if (lang == "fr")
            {
                return "Protégé B";

            }
            else
            {
                return "Protected B";
            }
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void AcceptTC_Click(object sender, EventArgs e)
        {
            Session["SHOW_TC"] = false;
            Response.Redirect(ResolveUrl("~/"));
        }
    }
}