//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IJPReporting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserResponses
    {
        public System.Guid response_id { get; set; }
        public System.Guid question_id { get; set; }
        public Nullable<System.Guid> response_choice_id { get; set; }
        public string devlopment { get; set; }
        public System.Guid CreatorId { get; set; }
        public System.Guid Form_Id { get; set; }
        public int question_group { get; set; }
    
        public virtual Forms Forms { get; set; }
        public virtual Questions Questions { get; set; }
    }
}
