using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IJPReporting.UserControls
{
    public partial class CKEditor : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // setter la langue du control
                ckEditor.Language = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                // toolbar
                ckEditor.config.toolbar = new object[]
            {
                new object[] { "Bold", "Italic", "Underline", "Strike", "-", "Subscript", "Superscript" },
                new object[] { "NumberedList", "BulletedList", "-", "Outdent", "Indent", "Blockquote" },
                new object[] { "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock" },
                new object[] { "Link", "Unlink" },
                new object[] { "Maximize", "ShowBlocks" },
                "/",
                new object[] { "Styles", "Format", "Font", "FontSize" },
                new object[] { "TextColor", "BGColor" }
            };
            }
        }

        public string Text
        {
            get { return ckEditor.Text; }
            set { ckEditor.Text = value; }
        }

        public int MaxLength
        {
            get { return ckEditor.MaxLength; }
            set { ckEditor.MaxLength = value; }
        }

        public Unit Width
        {
            get { return ckEditor.Width; }
            set { ckEditor.Width = value; }
        }

        public Unit Height
        {
            get { return ckEditor.Height; }
            set { ckEditor.Height = value; }
        }
    }
}