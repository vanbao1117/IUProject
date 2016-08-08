using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Web.Models
{
    public class ClassScheduleViewModel
    {
        public string ClassScheduleID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string RoomID { get; set; }
        public string SlotID { get; set; }
        public string LecturerID { get; set; }
        public string Lecturer { get; set; }
        public string StudentListID { get; set; }
        public string StudentName { get; set; }
        public string StudentID { get; set; }
        public string UserID { get; set; }
        public System.DateTime DateStudy { get; set; }
        public string FormattedDateStudye
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", DateStudy);
            }
        }
    }


    public class ClassSchedulePageViewModel
    {
        public List<ClassScheduleViewModel> ClassSchedules { get; set; }
        public int TotalPages { get; set; }
    }
}
