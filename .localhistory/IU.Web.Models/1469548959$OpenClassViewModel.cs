using System;
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
}