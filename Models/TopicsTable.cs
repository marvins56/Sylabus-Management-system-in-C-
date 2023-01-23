//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TopicsTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TopicsTable()
        {
            this.subjecttopics = new HashSet<subjecttopic>();
            this.SubTopicsTables = new HashSet<SubTopicsTable>();
        }
    
        public int Topic_id { get; set; }
        public string Topic_Name { get; set; }
        public int Class_id { get; set; }
        public Nullable<bool> IsComplete { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public int Subject_id { get; set; }
        public int Term_id { get; set; }
        public int year_Id { get; set; }
        public int Week_id { get; set; }
        public string Overview { get; set; }
        public string File { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    
        public virtual ClassTable ClassTable { get; set; }
        public virtual SubjectTable SubjectTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<subjecttopic> subjecttopics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTopicsTable> SubTopicsTables { get; set; }
        public virtual Term Term { get; set; }
        public virtual WeeksTable WeeksTable { get; set; }
        public virtual Year Year { get; set; }
    }
}
