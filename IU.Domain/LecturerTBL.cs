//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IU.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class LecturerTBL
    {
        public LecturerTBL()
        {
            this.LecturerScheduleTBLs = new HashSet<LecturerScheduleTBL>();
            this.SubjectTBLs = new HashSet<SubjectTBL>();
            this.FeedBackTBLs = new HashSet<FeedBackTBL>();
        }
    
        public string LecturerID { get; set; }
        public string LecturerName { get; set; }
        public bool LecturerGender { get; set; }
        public System.DateTime LecturerBirth { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerEmail { get; set; }
        public string UserID { get; set; }
    
        public virtual ICollection<LecturerScheduleTBL> LecturerScheduleTBLs { get; set; }
        public virtual ICollection<SubjectTBL> SubjectTBLs { get; set; }
        public virtual ICollection<FeedBackTBL> FeedBackTBLs { get; set; }
    }
}
