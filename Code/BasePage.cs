using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IJPReporting.Models;
using Owin;
using System.Configuration;

namespace GCWebUsabilityTheme
{
    public class BasePage : System.Web.UI.Page
    {
        #region Overridden Page Events
        protected override void InitializeCulture()
        {
            string userCulture = null;
            string pattern = "^\\S+-(\\S+).aspx\\S*$";
            Match m = default(Match);

            m = Regex.Match(Request.RawUrl, pattern, RegexOptions.IgnoreCase);

            //Given an arbitrary page such as "/SomeDir/SomePage-fra.aspx?id=123",
            //the above pattern should find 1 group: "fra".

            if (m.Success)
            {
                string lang = m.Groups[1].Value;

                switch (lang)
                {
                    case "e":
                    case "en":
                    case "eng":
                        userCulture = "en-CA";
                        break;
                    case "f":
                    case "fr":
                    case "fra":
                        userCulture = "fr-CA";
                        break;
                    default:
                        int langLength = (m.Groups[1].Value.Length);
                        userCulture = langLength == 2 ? string.Format("{0}-{1}", lang, lang.ToUpper()) : "en-CA";
                        break;
                }

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(userCulture);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(userCulture);
            }
            else
            {
                if (Request.Cookies["langCookie"] == null)
                {

                    // forcer la langue selon l'url de production par Jason Pesant 2016-12-04
                    //string originalString = Request.Url.OriginalString.ToLower();
                    //string urlEncan = "http://encan.gc.ca";
                    //string urlAuction = "http://auction.gc.ca";
                    //if (!IsPostBack && (originalString.StartsWith(urlAuction) || originalString.StartsWith(urlEncan)))
                    //{
                    //    if (originalString.StartsWith(urlAuction)) Request.UserLanguages[0] = "en-CA";
                    //    else if (originalString.StartsWith(urlEncan)) Request.UserLanguages[0] = "fr-CA";
                    //}

                    HttpCookie langCookie = new HttpCookie("langCookie");
                    langCookie.Value = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;
                    langCookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(langCookie);

                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCookie.Value);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(langCookie.Value);
                }
                else
                {
                    // ajustement de la langue par Jason Pesant 2016-12-05
                    // fr devient fr-FR mais devrait plutôt devenir fr-CA
                    if (Request.Cookies["langCookie"].Value == "fr") Request.Cookies["langCookie"].Value = "fr-CA";
                    // en devient en-US mais devrait plutôt devenir en-CA
                    if (Request.Cookies["langCookie"].Value == "en") Request.Cookies["langCookie"].Value = "en-CA";

                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request.Cookies["langCookie"].Value);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Request.Cookies["langCookie"].Value);
                }

            }
        }
        #endregion

        #region Public properties
        /// <summary>
        /// An entity primarily responsible for making the resource.
        /// </summary>
        public virtual string Creator
        {
            get
            {
                string str = ViewState["PageCreator"] as string;
                if (str == null)
                {
                    //REVIEW:  Edit as appropriate.  Should this be in the custom web.config section?
                    if (this.Language == "fr")
                    {
                        return "Gouvernement du Canada";
                    }
                    else
                    {
                        return "Government of Canada";
                    }
                }
                else
                {
                    return str;
                }
            }
            set
            {
                ViewState["PageCreator"] = value;
            }
        }

        /// <summary>
        /// Date of creation of the resource.
        /// </summary>
        /// <remarks>
        /// Default is file system's creation date.
        /// </remarks>
        public virtual string Created
        {
            get
            {
                string str = ViewState["PageDateCreated"] as string;
                if (str == null)
                {
                    System.IO.FileInfo objInfo = new System.IO.FileInfo(Server.MapPath(Request.ServerVariables.Get("SCRIPT_NAME")));
                    return String.Format("{0:yyyy-MM-dd}", objInfo.CreationTime.Date);
                }
                else
                    return str;
            }
            set
            {
                ViewState["PageDateCreated"] = value;
            }
        }

        /// <summary>
        /// Date on which the resource was changed.
        /// </summary>
        /// <remarks>
        /// Default is file system's last write time date.
        /// </remarks>
        public virtual string Modified
        {
            get
            {
                string str = ViewState["PageDateModified"] as string;
                if (str == null)
                {
                    string pageURL = Request.ServerVariables.Get("SCRIPT_NAME");
                    if (pageURL.Length > 5)
                    {
                        pageURL = (pageURL.Substring(pageURL.Length - 5) == ".aspx") ? pageURL : pageURL + ".aspx";
                    }
                    System.IO.FileInfo objInfo = new System.IO.FileInfo(Server.MapPath(pageURL));
                    objInfo.Refresh();
                    return String.Format("{0:yyyy-MM-dd}", objInfo.LastWriteTime.Date);
                }
                else
                    return str;
            }
            set
            {
                ViewState["PageDateModified"] = value;
            }
        }

        /// <summary>
        /// Date of formal issuance (e.g., publication) of the resource.
        /// </summary>
        /// <remarks>
        /// Default is Created date.
        /// </remarks>
        /// <see cref="Created"/>
        public virtual string Issued
        {
            get
            {
                string str = ViewState["PageDateIssued"] as string;
                if (str == null)
                {
                    return this.Created;
                }
                else
                {
                    return str;
                }
            }
            set
            {
                ViewState["PageDateIssued"] = value;
            }
        }

        /// <summary>
        /// A language of the resource.
        /// </summary>
        /// <remarks>
        /// Two-letter language. Default is English ("en").
        /// </remarks>
        public virtual string Language
        {
            get
            {
                string str = ViewState["PageLanguage"] as string;
                if (str == null)
                {
                    return Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                }
                else
                    return str;
            }
            set
            {
                ViewState["PageLanguage"] = value;
            }
        }

        /// <summary>
        /// Should the left-side secondary menu be shown?
        /// </summary>
        public virtual bool ShowSectionMenu
        {
            get
            {
                string str = ViewState["ShowSectionMenu"] as string;
                if (str == null)
                {
                    WetBoewConfiguration config = WetBoewConfiguration.GetConfiguration();
                    return config.ShowSectionMenu;
                }
                else
                {
                    return bool.Parse(str);
                }
            }
            set
            {
                ViewState["ShowSectionMenu"] = value.ToString();
            }
        }

        /// <summary>
        /// The topic of the resource.
        /// </summary>
        public virtual string Subject
        {
            get
            {
                string str = ViewState["PageSubject"] as string;
                if (str == null)
                {
                    //REVIEW:  Edit as appropriate.  Should this be in the custom web.config section?
                    if (this.Language == "fr")
                    {
                        return ConfigurationManager.AppSettings["SiteNameFR"]; 
                    }
                    else
                    {
                        return ConfigurationManager.AppSettings["SiteNameEN"];
                    }
                }
                else
                {
                    return str;
                }
            }
            set
            {
                ViewState["PageSubject"] = value;
            }
        }

        /// <summary>
        /// SuperAdmin in Identity
        /// </summary>
        public bool IsSuperAdmin
        {
            get
            {
                object isSuperAdmin = ViewState["isSuperAdmin"];
                if (isSuperAdmin == null)
                {
                    if (this.User != null && this.User.Identity.IsAuthenticated && this.User.Identity.GetUserId() != null)
                    {
                        ApplicationDbContext context = new ApplicationDbContext();
                        var userMgr = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                        if (userMgr.IsInRole(this.User.Identity.GetUserId(), "SuperAdmin"))
                        {
                            ViewState["isSuperAdmin"] = true;
                        }
                        else
                        {
                            ViewState["isSuperAdmin"] = false;
                        }
                    }
                    else
                    {
                        ViewState["isSuperAdmin"] = false;
                    }
                }

                return (bool)ViewState["isSuperAdmin"];
            }
        }
        //***************************************************************************** 
        //Traitement d'image
        //*****************************************************************************
        public void CreateThumb(int nWidth, string imgLoc, string imgDest, bool bubbleMark)
        {
            int thumbWidth = nWidth;

            System.Drawing.Image image = System.Drawing.Image.FromFile(imgLoc);

            int srcWidth = image.Width;
            int srcHeight = image.Height;

            Decimal sizeRatio = ((Decimal)srcHeight / srcWidth);
            int thumbHeight = Decimal.ToInt32(sizeRatio * thumbWidth);
            Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);

            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);

            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

            //écrire text        
            Graphics g = Graphics.FromImage(bmp);
            //Set quality to High
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int pWidth;
            int pHeight;

            if (bubbleMark)
            {
                //String to draw in Picture        
                if (nWidth == 150)
                {
                    pWidth = thumbWidth - 147;
                    pHeight = thumbHeight - 15;
                    g.DrawString("DevIT - QRO", new Font("Century Gothic", 10), new SolidBrush(Color.LightGray), pWidth, pHeight);
                }
                else
                {
                    pWidth = thumbWidth - 150;
                    pHeight = thumbHeight - 20;
                    g.DrawString("DevIT - QRO", new Font("Century Gothic", 10), new SolidBrush(Color.LightGray), pWidth, pHeight);
                }
            }

            bmp.Save(imgDest);

            bmp.Dispose();
            image.Dispose();
        }
        #endregion
    }
}