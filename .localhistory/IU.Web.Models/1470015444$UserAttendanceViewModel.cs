using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Web.Models
{
    public class UserAttendanceViewModel
    {
        public string SubjectName { set; get; }
        public string SubjectID { set; get; }
        public string AbbreSubjectName { set; get; }
        public string UserId { set; get; }
        public string ClassName { set; get; }
        public string ClassID { set; get; }

        public string RomID { set; get; }
        public string SlotID { set; get; }
        public string SlotTime { set; get; }
        public string Note { set; get; }
    }
}
