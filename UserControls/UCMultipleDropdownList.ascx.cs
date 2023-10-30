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
    public partial class UCMultipleDropdownList : System.Web.UI.UserControl
    {
        public delegate void AddDDLEventHandler(object sender, AddDDLEventArgs e);
        public event AddDDLEventHandler AddDDLClick;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
        public WetDropDownList WetDDL
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

        public WetLinkButton WetAddDDL
        {
            get
            {
                return this.AddDDL;
            }
            set
            {
                this.AddDDL = value;
            }
        }
  

        protected void AddDDL_Click(object sender, EventArgs e)
        {           
            AddDDLClick?.Invoke(sender, new AddDDLEventArgs(this));
        }
    }
}