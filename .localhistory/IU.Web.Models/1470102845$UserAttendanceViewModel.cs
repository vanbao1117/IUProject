using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Web.Models
{
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
        public string SlotTime { get; set; }
        public string Attendancer { get; set; }
        public string AttendancerName { get; set; }
        public System.DateTime DateAttendance { get; set; }
        public bool Attendance { get; set; }
        public string RoomID { get; set; }
        public string Note { get; set; }
        public string Avata { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string UserID { get; set; }
        public bool isAttendanced { get; set; }
    }
}
