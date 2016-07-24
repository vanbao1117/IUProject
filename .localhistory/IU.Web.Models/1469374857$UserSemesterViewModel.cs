using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Web.Models
{
    public class UserSemesterViewModel
    {
        public string SemesterID { get; set; }
        public string SemesterName { get; set; }
        public string SemesterCode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }

    public class UserAttendanceViewModel
    {
        public string AttendanceID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string SemesterID { get; set; }
        public string StudentListID { get; set; }
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string SlotID { get; set; }
        public string Attendancer { get; set; }
        public string AttendancerName { get; set; }
        public System.DateTime DateAttendance { get; set; }
        public bool Attendance { get; set; }
        public string RoomID { get; set; }
    }
}
