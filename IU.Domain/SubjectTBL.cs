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
    
    public partial class SubjectTBL
    {
        public SubjectTBL()
        {
            this.AttendanceTBLs = new HashSet<AttendanceTBL>();
            this.LecturerScheduleTBLs = new HashSet<LecturerScheduleTBL>();
            this.OpenSubjectTBLs = new HashSet<OpenSubjectTBL>();
            this.LecturerTBLs = new HashSet<LecturerTBL>();
        }
    
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string AbbreSubjectName { get; set; }
    
        public virtual ICollection<AttendanceTBL> AttendanceTBLs { get; set; }
        public virtual ICollection<LecturerScheduleTBL> LecturerScheduleTBLs { get; set; }
        public virtual ICollection<OpenSubjectTBL> OpenSubjectTBLs { get; set; }
        public virtual ICollection<LecturerTBL> LecturerTBLs { get; set; }
    }
}
