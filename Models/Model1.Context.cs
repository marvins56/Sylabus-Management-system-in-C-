﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMIS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SMISEntities : DbContext
    {
        public SMISEntities()
            : base("name=SMISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ClassTable> ClassTables { get; set; }
        public virtual DbSet<MidtermMarksTable> MidtermMarksTables { get; set; }
        public virtual DbSet<StudentsTable> StudentsTables { get; set; }
        public virtual DbSet<subject_class> subject_class { get; set; }
        public virtual DbSet<SubjectTable> SubjectTables { get; set; }
        public virtual DbSet<subjecttopic> subjecttopics { get; set; }
        public virtual DbSet<SubTopicsTable> SubTopicsTables { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
        public virtual DbSet<TopicsTable> TopicsTables { get; set; }
        public virtual DbSet<topicstat> topicstats { get; set; }
        public virtual DbSet<WeeksTable> WeeksTables { get; set; }
        public virtual DbSet<Year> Years { get; set; }
    }
}
