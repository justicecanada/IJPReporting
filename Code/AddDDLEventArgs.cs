using IJPReporting.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IJPReporting.Code
{
    public class AddDDLEventArgs : EventArgs
    {
        public UCMultipleDropdownList UC { get; private set; }

        public AddDDLEventArgs(UCMultipleDropdownList uc)
        {
            UC = uc;
        }
    }
}