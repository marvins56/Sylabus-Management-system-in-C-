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
    
    public partial class subject_class
    {
        public int ID { get; set; }
        public int Subject_id { get; set; }
        public int Class_id { get; set; }
    
        public virtual ClassTable ClassTable { get; set; }
        public virtual SubjectTable SubjectTable { get; set; }
    }
}
