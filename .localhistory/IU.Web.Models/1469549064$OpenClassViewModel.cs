using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class OpenClassViewModel
    {
        public string OpenClassID { get; set; }
        public string ClassID { get; set; }
        public string RoomID { get; set; }
        public string SlotID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Creater { get; set; }
    }

    public class OpenSubjectViewModel
    {
        public string OpenSubjectID { get; set; }
        public string SubjectID { get; set; }
        public string SubjectCode { get; set; }
        public string OpenClassID { get; set; }
        public List<OpenClassViewModel> OpenClassID { get; set; }
        public Nullable<int> Credit { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Creater { get; set; }
    }
}