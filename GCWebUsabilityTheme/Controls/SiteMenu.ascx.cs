using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GCWebUsabilityTheme;
using System.Web.UI.HtmlControls;

namespace GCWebUsabilityTheme.Controls
{
    public partial class SiteMenu : System.Web.UI.UserControl
    {
        protected string myProvider;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //Set the sitemap provider.  Assuming that each provider is prefixed with the language abbreviation.
                string lang = ((BasePage)Page).Language;
                myProvider = string.Format("{0}HeaderSiteMapProvider", lang.ToUpper());
                SiteMapDataSource1.SiteMapProvider = myProvider;

                //Hide or show the site menu depending on the setting of WetBoewGroup/WetBoew/@showSiteMenu in web.config.
                WetBoewConfiguration config = WetBoewConfiguration.GetConfiguration();
                bool showSiteMenu = config.ShowSiteMenu;

                if (showSiteMenu)
                {
                    this.PlaceHolderSiteMenu.Visible = true;
                    this.PlaceHolderNoSiteMenu.Visible = false;
                }
                else
                {
                    this.PlaceHolderSiteMenu.Visible = false;
                    this.PlaceHolderNoSiteMenu.Visible = true;
                }
            }
        }

        //protected string SiteMenuFile
        //{
        //    get
        //    {
        //        //REVIEW:  Should this be added to the custom config section of web.config?
        //        return string.Format("GCWebUsabilityTheme/wet-v4/dist/ajax/sitemenu-{0}.html", ((BasePage)Page).Language);
        //    }
        //}

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

        //protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        SiteMapNode node = e.Item.DataItem as SiteMapNode;

        //        if (node.HasChildNodes)
        //        {
        //            PlaceHolder phSubMenu = (PlaceHolder)e.Item.FindControl("phSubMenu");
        //            phSubMenu.Controls.Add(this.CreateSubNavBarControl(node.ChildNodes));
        //        }
        //    }
        //}

        //private Control CreateSubNavBarControl(SiteMapNodeCollection nodes)
        //{
        //    HtmlGenericControl ul = new HtmlGenericControl("ul");
        //    ul.Attributes.Add("class", "sm list-unstyled");
        //    ul.Attributes.Add("role", "menu");

        //    HtmlGenericControl li;
        //    HtmlGenericControl a;

        //    foreach (SiteMapNode node in nodes)
        //    {
        //        li = new HtmlGenericControl("li");

        //        // formattage différent pour les sous-menu
        //        if (node.HasChildNodes)
        //        {
        //            li.Controls.Add(this.CreateSubNavBarDetailsControl(node.Title, node.ChildNodes));
        //        }
        //        else
        //        {
        //            a = new HtmlGenericControl("a");
        //            a.Attributes.Add("role", "menuitem");
        //            a.Attributes.Add("href", node.Url);
        //            a.InnerText = node.Title;

        //            li.Controls.Add(a);
        //        }
                
        //        ul.Controls.Add(li);
        //    }

        //    return ul;
        //}

        //private Control CreateSubNavBarDetailsControl(string title, SiteMapNodeCollection nodes)
        //{
        //    HtmlGenericControl details = new HtmlGenericControl("details");
        //    HtmlGenericControl summary = new HtmlGenericControl("summary");
        //    summary.InnerText = title;

        //    HtmlGenericControl ul = new HtmlGenericControl("ul");
        //    ul.Attributes.Add("role", "menu");

        //    HtmlGenericControl li;
        //    HtmlGenericControl a;
        //    foreach (SiteMapNode node in nodes)
        //    {
        //        a = new HtmlGenericControl("a");
        //        a.Attributes.Add("href", node.Url);
        //        a.InnerText = node.Title;

        //        li = new HtmlGenericControl("li");
        //        li.Controls.Add(a);

        //        ul.Controls.Add(li);
        //    }

        //    details.Controls.Add(summary);
        //    details.Controls.Add(ul);

        //    return details;
        //}
    }
}