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

    
    public class UserAttendancePagingViewModel
    {
        public int TotalPages { set; get; }
        public string SubjectName { set; get; }
        public string SubjectCode { set; get; }
        public UserAttendanceViewModel[] Page { set; get; }
    }
}
