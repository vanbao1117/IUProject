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
    
    public partial class ClassTBL
    {
        public ClassTBL()
        {
            this.AttendanceTBLs = new HashSet<AttendanceTBL>();
            this.LecturerScheduleTBLs = new HashSet<LecturerScheduleTBL>();
            this.StudentListTBLs = new HashSet<StudentListTBL>();
            this.OpenClassTBLs = new HashSet<OpenClassTBL>();
        }
    
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Creater { get; set; }
        public System.DateTime StartDate { get; set; }
    
        public virtual ICollection<AttendanceTBL> AttendanceTBLs { get; set; }
        public virtual ICollection<LecturerScheduleTBL> LecturerScheduleTBLs { get; set; }
        public virtual ICollection<StudentListTBL> StudentListTBLs { get; set; }
        public virtual ICollection<OpenClassTBL> OpenClassTBLs { get; set; }
    }
}
