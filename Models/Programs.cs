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
    
    public partial class Programs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Programs()
        {
            this.Forms = new HashSet<Forms>();
        }
    
        public int programId { get; set; }
        public string name_fr { get; set; }
        public string name_en { get; set; }
        public int community_Id { get; set; }
        public int region_id { get; set; }
        public bool isDeleted { get; set; }
    
        public virtual Community Community { get; set; }
        public virtual Regions Regions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Forms> Forms { get; set; }
    }
}
