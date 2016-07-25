using System;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class LecturerViewModel
    {
        public string Id { get; set; }
        public string AspNetUserId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}