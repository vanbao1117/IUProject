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
        
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    }
}
