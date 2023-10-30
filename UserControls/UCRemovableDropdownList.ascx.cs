using IJPReporting.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WetControls.Controls;

namespace IJPReporting.UserControls
{
    public partial class UCRemovableDropdownList : System.Web.UI.UserControl
    {
        public delegate void RemoveDDLEventHandler(object sender, RemoveDDLEventArgs e);
        public event RemoveDDLEventHandler RemoveDDLClick;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public WetDropDownList wetDDL
        {
            get
            {
                return this.DDL;
            }
            set
            {
                this.DDL = value;
            }
        }

        public WetLinkButton wetRemoveDDL
        {
            get
            {
                return this.RemoveDDL;
            }
            set
            {
                this.RemoveDDL = value;
            }
        }

        protected void RemoveDDL_Click(object sender, EventArgs e)
        {
            RemoveDDLClick?.Invoke(sender, new RemoveDDLEventArgs(this));
        }
    }
}