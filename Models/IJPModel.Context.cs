﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BD_IJPReportingEntities : DbContext
    {
        public BD_IJPReportingEntities()
            : base("name=BD_IJPReportingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Actions> Actions { get; set; }
        public virtual DbSet<Community> Community { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Operators> Operators { get; set; }
        public virtual DbSet<Programs> Programs { get; set; }
        public virtual DbSet<QuestionTypes> QuestionTypes { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<ReponseChoices> ReponseChoices { get; set; }
        public virtual DbSet<UserResponses> UserResponses { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Forms> Forms { get; set; }
        public virtual DbSet<SubQuestions> SubQuestions { get; set; }
        public virtual DbSet<Conditions> Conditions { get; set; }
        public virtual DbSet<Form_Type> Form_Type { get; set; }
        public virtual DbSet<Section_Conditions> Section_Conditions { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
    }
}