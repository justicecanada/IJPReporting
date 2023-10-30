using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace IJPReporting.Logic
{
    [Serializable]
    public class ViewStateControl
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public int StepIndex { get; set; }
        public string LabelText { get; set; }
        public string ToolTip { get; set; }
        public bool IsRequired { get; set; }
        public object DataSource { get; set; }
        public bool HasCondition { get; set; }
        public bool Visible { get; set; }
        public bool IsDate { get; set; }
        public string CssClass { get; set; }
        public int Index { get; set; }
        public List<Models.ReponseChoices> DisabledResponsesChoice { get; set; }
    }
}