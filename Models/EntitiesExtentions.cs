using IJPReporting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace IJPReporting.Models
{

    [Serializable]
    public partial class Form_Type
    {
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string Name
        {
            get
            {
                return lang == "fr" ? this.FormTitle_fr : this.FormTitle_en;
            }
        }
    }

    [Serializable]
    public partial class Forms {
        ModelHelper helper = new ModelHelper();
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string FormName
        {
            get
            {
                return lang == "fr" ? this.Form_Type.FormTitle_fr : this.Form_Type.FormTitle_en;
            }
        }

        public bool Save()
        {
            try
            {
                using (var context = new BD_IJPReportingEntities())
                {
                    Forms form = helper.GetFormById(this.form_id);

                    if (form != null)
                    {
                        form.ClientFileNumberId = this.ClientFileNumberId;
                        form.date_updated = this.date_updated;
                        form.isFinal = this.isFinal;
                        form.ComplStatusId = this.ComplStatusId;
                        form.AcceptStatusId = this.AcceptStatusId;
                        form.RefDate = this.RefDate;
                        context.Entry(form).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        context.Forms.Add(this);
                    }
                    return context.SaveChanges() > 0;
                }
            } catch (Exception ex)
            {
                return false;
            }
            
        }
    }

    [Serializable]
    public partial class Files
    {
        public void Delete()
        {
            using (var context = new BD_IJPReportingEntities())
            {
                context.Files.Attach(this);
                context.Files.Remove(this);
                context.SaveChanges();
            }
        }
    }

    [Serializable]
    public partial class Questions {
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string Label
        {
            get
            {
                return lang == "fr" ? this.libelle_fr.Trim() : this.libelle_en.Trim();
            }
        }

        public string Note
        {
            get
            {
                return lang == "fr" ? this.Note_fr : this.Note_en;
            }
        }
    }

    [Serializable]
    public partial class ReponseChoices {
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string Text
        {
            get
            {
                return lang == "fr" ? this.text_fr : this.text_en;
            }
        }
    }

    [Serializable]
    public partial class Section { }

    [Serializable]
    public partial class UserResponses { }

    [Serializable]
    public partial class QuestionTypes { }
    [Serializable]
    public partial class Conditions { }
    [Serializable]
    public partial class Actions { }
    [Serializable]
    public partial class Operators { }

    [Serializable]
    public partial class SubQuestions { }
    [Serializable]
    public partial class AspNetUsers { }

    [Serializable]
    public partial class Regions {
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string Name
        {
            get
            {
                return lang == "fr" ? this.region_name_fr : this.region_name_en;
            }
        }
    }
    [Serializable]
    public partial class Status {
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string StatusName
        {
            get
            {
                var statusName = lang == "fr" ? this.Status_name_fr : this.Status_name_en;
                var NaN = lang == "fr" ? "S/O" : "N/A";
                return !String.IsNullOrEmpty(statusName) ? statusName : NaN;
            }
        }
    }
    [Serializable]
    public partial class Community {
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string Name
        {
            get
            {
                return lang == "fr" ? this.Community_name_fr : this.Community_name_en;
            }
        }
    }
    [Serializable]
    public partial class Programs {
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        public string Name
        {
            get
            {
                return lang == "fr" ? this.name_fr : this.name_fr;
            }
        }
    }
}