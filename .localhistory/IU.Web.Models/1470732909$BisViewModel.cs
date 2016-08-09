using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class BisViewModel
    {

        public string SemesterID { get; set; }
        public string ClassID { get; set; }
        public string SubjectID { get; set; }
        public string LecturerID{ get; set; }
        public string ClassName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string[] SlotIDs { get; set; }
        public string[] SlotNames { get; set; }
        public int ModeID { get; set; }
        public int Limit { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
    }


    public class StudentViewModel
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public bool StudentGender { get; set; }
        public System.DateTime StudentBirth { get; set; }
        public string StudentPhone { get; set; }
        public string StudentEmail { get; set; }
        public string StudentCourse { get; set; }
        public string ParentPhone { get; set; }
        public string Password { get; set; }
        public string ClassID { get; set; }
    }


    public class StudentViewModel
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public bool StudentGender { get; set; }
        public System.DateTime StudentBirth { get; set; }
        public string StudentPhone { get; set; }
        public string StudentEmail { get; set; }
        public string StudentCourse { get; set; }
        public string ParentPhone { get; set; }
        public string Password { get; set; }
        public string ClassID { get; set; }
    }
}