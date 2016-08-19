using System;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class FeedbackViewModel
    {
        public string LecturerID { get; set; }
        public string Quality { get; set; }
        public string Attitude { get; set; }
        public string Satisfaction { get; set; }
        public string OnTime { get; set; }
        public string Comments { get; set; 
        public string Student { get; set; }
    }
}