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
    
    public partial class ClassScheduleTBL
    {
        public string ClassScheduleID { get; set; }
        public string ClassID { get; set; }
        public string SubjectID { get; set; }
        public string RoomID { get; set; }
        public string SlotID { get; set; }
        public string LecturerID { get; set; }
        public string StudentListID { get; set; }
        public System.DateTime DateStudy { get; set; }
        public Nullable<int> ModeID { get; set; }
        public Nullable<bool> IsAttendance { get; set; }
    }
}
