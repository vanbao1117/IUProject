using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class PreviewViewModel
    {
        public string Day { get; set; }
        public string Slot { get; set; }
        public string StudentName { get; set; }
        public string Status { get; set; }
    }

    public class PreviewListViewModel
    {
        public List<PreviewViewModel> Preview { get; set; }
        public string LectureID { get; set; }
        public string LectureNam { get; set; }
        public string ClassName { get; set; }
        public string ClassID { get; set; }

        public string SubjectName { get; set; }
        public string SubjectID { get; set; }
    }
}