﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class PreviewViewModel
    {
        public string Day { get; set; }
        public string Slot { get; set; }
        public string StudentName { get; set; }
        public string Status { get; set; }
        public int ColSpan { get; set; }
    }

    public class PreviewListViewModel
    {
        public List<PreviewViewModel> Preview { get; set; }
        public List<LecturePreviewViewModel> Data { get; set; }
        public string LectureID { get; set; }
        public string LectureNam { get; set; }
        public string ClassName { get; set; }
        public string ClassID { get; set; }
        public string CurrentSemmeter { set; get; }
        public string SubjectName { get; set; }
        public string SubjectID { get; set; }
    }

    public class LectureClassSubjectViewModel
    {
        public List<ClassViewModel> LectureClass { get; set; }
        public List<UserSubjectViewModel> LectureSubject { get; set; }
        public ClassViewModel SelectedClass { get; set; }
        public UserSubjectViewModel SelectedSubject { get; set; }

        public List<LecturerViewModel> Lectures { get; set; }
    }

    public class LecturePreviewViewModel
    {
        public string Row { get; set; }
        public List<PreviewViewModel> Cols { get; set; }
        
    }
}