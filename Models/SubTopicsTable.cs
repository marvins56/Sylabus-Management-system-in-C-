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
    
    public partial class SubTopicsTable
    {
        public int SubTopics_id { get; set; }
        public int Topic_id { get; set; }
        public string SubTopic { get; set; }
        public string Overview { get; set; }
        public Nullable<bool> IsComplete { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public int Class_id { get; set; }
        public int year_Id { get; set; }
        public int Term_Id { get; set; }
        public string File { get; set; }
    
        public virtual ClassTable ClassTable { get; set; }
        public virtual TopicsTable TopicsTable { get; set; }
        public virtual Term Term { get; set; }
        public virtual Year Year { get; set; }
    }
}
