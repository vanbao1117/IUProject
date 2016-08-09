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
    
    public partial class SemesterTBL
    {
        public SemesterTBL()
        {
            this.AttendanceTBLs = new HashSet<AttendanceTBL>();
            this.LecturerScheduleTBLs = new HashSet<LecturerScheduleTBL>();
            this.StudentListTBLs = new HashSet<StudentListTBL>();
        }
    
        public string SemesterID { get; set; }
        public Nullable<int> SemesterNo { get; set; }
        public string SemesterName { get; set; }
        public string SemesterCode { get; set; }
        public string Blog1 { get; set; }
        public string Blog2 { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual ICollection<AttendanceTBL> AttendanceTBLs { get; set; }
        public virtual ICollection<LecturerScheduleTBL> LecturerScheduleTBLs { get; set; }
        public virtual ICollection<StudentListTBL> StudentListTBLs { get; set; }
    }
}
