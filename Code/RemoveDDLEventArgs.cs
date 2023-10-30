using IJPReporting.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IJPReporting.Code
{
    public class RemoveDDLEventArgs : EventArgs
    {
        public UCRemovableDropdownList UC { get; private set; }

        public RemoveDDLEventArgs(UCRemovableDropdownList uc)
        {
            UC = uc;
        }
    }
}