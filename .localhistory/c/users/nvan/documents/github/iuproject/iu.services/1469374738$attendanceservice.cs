﻿using IU.Domain;
using IU.Services.Repositories;
using IU.Services.Utilities;
using IU.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IU.Services
{
    public class AttendanceService : IDisposable
    {
       
        public AttendanceService()
        {
            
        }

        public async Task<UserAttendanceViewModel[]> GetAttendanceByStudentSync(int pageNumber, int pageSize, string userId)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAttendanceByStudent(pageNumber, pageSize, userId));
            }
        }

        private UserAttendanceViewModel[] GetAttendanceByStudent(int pageNumber, int pageSize, string userId)
        {
            using (var context = new IUContext())
            {
                int NumberOfItems = 0;

                var attendances =
                 from attendanceTBLs in context.AttendanceTBLs
                 join studentListTBLs in context.StudentListTBLs
                     on attendanceTBLs.StudentListID equals studentListTBLs.StudentListID
                 join studentTBLs in context.StudentTBLs
                    on studentListTBLs.StudentID equals studentTBLs.StudentID
                 where studentTBLs.UserID == userId
                 select attendanceTBLs;

                var firstPageData = Helper.PagedResult(attendances, pageNumber, pageSize, attendance => attendance.DateAttendance, false, out NumberOfItems);

                var firstPage = firstPageData.Select(f => new UserAttendanceViewModel() { Attendance = f.Attendance, AttendanceID = f.AttendanceID, Attendancer = f.Attendancer, AttendancerName = GetLecturer(f.Attendancer).LecturerName, ClassID = f.ClassID, ClassName = GetClassName(f.ClassID), DateAttendance = f.DateAttendance, RoomID = f.RoomID, SemesterID = f.SemesterID, SlotID = f.SlotID, StudentListID = f.StudentListID, SubjectID = f.SubjectID, SubjectName = GetSubjectByName(f.SubjectID).SubjectName});

                return semesters.ToArray();
            }
        }


        private LecturerTBL GetLecturer(string userId)
        {
            using (var context = new IUContext())
            {
                var Lecturer = context.LecturerTBLs.Where(u => u.UserID == userId).FirstOrDefault();
                return Lecturer;
            }
        }

        private SubjectTBL GetSubjectByName(string subjectId)
        {
            using (var context = new IUContext())
            {
                var sub = context.SubjectTBLs.Where(obj => obj.SubjectID == subjectId);
                return sub.FirstOrDefault();
            }
        }

        private string GetClassName(string classId)
        {
            using (var context = new IUContext())
            {
                var _class = context.ClassTBLs.Where(c => c.ClassID == classId).FirstOrDefault();
                if (_class != null) return _class.ClassName;
            }
            return null;
        }

        public async Task<UserSemesterViewModel[]> GetSemesterByStudentSync(string userId)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetSemesterByStudent(userId));
            }
        }

        private UserSemesterViewModel[] GetSemesterByStudent(string userId)
        {
            using (var context = new IUContext())
            {

                var semesters =
                 from semesterTBLs in context.SemesterTBLs
                 join studentListTBLs in context.StudentListTBLs
                     on semesterTBLs.SemesterID equals studentListTBLs.SemesterID
                 join studentTBLs in context.StudentTBLs
                    on studentListTBLs.StudentID equals studentTBLs.StudentID
                 where studentTBLs.UserID == userId
                 select new UserSemesterViewModel() { SemesterID = semesterTBLs.SemesterID, SemesterName = semesterTBLs.SemesterName, StartDate = semesterTBLs.StartDate, EndDate = semesterTBLs.EndDate, SemesterCode = semesterTBLs.SemesterCode };

                return semesters.ToArray();
            }
        }

        #region Dispose
        ~AttendanceService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            // This method will remove current object from garbage collector's queue 
            // and stop calling finilize method twice 
        }    

        public void Dispose(bool disposer)
        {
            if (disposer)
            {
                //// dispose the managed objects
                //ListStudentTBLRepository.Dispose();
                //ListStudentTBLRepository = null;
            }
            // dispose the unmanaged objects
        }

        #endregion
    }
}