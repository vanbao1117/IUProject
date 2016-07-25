using System;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class FeedbackViewModel
    {
        public string LecturerID { get; set; }
        public string LecturerName { get; set; }
        public bool LecturerGender { get; set; }
        public System.DateTime LecturerBirth { get; set; }
        public string LecturerPhone { get; set; }
        public string LecturerEmail { get; set; }
        public string UserID { get; set; }
    }
}