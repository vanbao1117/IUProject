using IU.Domain;
using IU.Services.Repositories;
using IU.Services.Utilities;
using IU.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace IU.Services
{
    public class ScheduleClassService : IDisposable
    {
        private IRepository<StudentListTBL> ListStudentTBLRepository;

        public ScheduleClassService()
        {
            
        }

        public async Task<ClassSchedulePageViewModel> GetClassScheduleSync(int pageNumber, int pageSize, string userName, string abbreSubjectName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetClassSchedule(pageNumber, pageSize, userName, abbreSubjectName));
            }
        }

        private ClassSchedulePageViewModel GetClassSchedule(int pageNumber, int pageSize, string userName, string abbreSubjectName)
        {
            using (var context = new IUContext())
            {
                int NumberOfItems = 0;
                var sem = GetCurrentSemester();
                var currentStudent = GetCurrentStudent(userName);

                var currentStudentList = GetCurrentStudentList(currentStudent, sem.SemesterID);

                var subject = GetSubjectByName(abbreSubjectName);

                var classSheduleTbl = context.ClassScheduleTBLs.Where(c => c.StudentListID == currentStudentList.StudentListID && c.SubjectID == subject.SubjectID);

                var firstPageData = Helper.PagedResult(classSheduleTbl, pageNumber, pageSize, classScheduleTBLs => classScheduleTBLs.DateStudy, false, out NumberOfItems);

                var firstPage = firstPageData.ToList().Select(f => new ClassScheduleViewModel() { ClassID = f.ClassID, ClassName = GetClassName(f.ClassID), ClassScheduleID = f.ClassScheduleID, LecturerID = f.LecturerID, Lecturer = GetLecturerName(f.LecturerID), DateStudy = f.DateStudy.ToString("dddd, dd MMMM yyyy");, RoomID = f.RoomID, SlotID = GetSlotbyID(f.SlotID), StudentID = GetStudent(f.StudentListID).StudentID, StudentListID = f.StudentListID, StudentName = GetStudent(f.StudentListID).StudentName, SubjectID = f.SubjectID, SubjectName = GetSubjectName(f.SubjectID) });

                int totalPage = (int)Math.Ceiling((double)NumberOfItems / (double)pageSize); ;

                ClassSchedulePageViewModel classSchedule = new ClassSchedulePageViewModel() { TotalPages = totalPage, ClassSchedules = firstPage.ToList() };

                return classSchedule;
            }
        }


        private string GetSlotbyID(string SlotID)
        {
            return SlotID;
            //using (var context = new IUContext())
            //{
            //    return context.SlotTBLs.Where(s => s.SlotID == SlotID).FirstOrDefault().SlotTime;
            //}
        }

        public async Task<ClassSchedulePageViewModel> GetAllClassScheduleSync(int pageNumber, int pageSize, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllClassSchedule(pageNumber, pageSize, userName));
            }
        }



        private ClassSchedulePageViewModel GetAllClassSchedule(int pageNumber, int pageSize, string userName)
        {
            using (var context = new IUContext())
            {
                int NumberOfItems = 0;
                var sem = GetCurrentSemester();
                var studentList = GetStudentList(sem, userName);

                var classSheduleTbl = context.ClassScheduleTBLs.Where(c => studentList.Contains(c.StudentListID));

                var firstPageData = Helper.PagedResult(classSheduleTbl, pageNumber, pageSize, classScheduleTBLs => classScheduleTBLs.DateStudy, false, out NumberOfItems);

                var firstPage = firstPageData.ToList().Select(f => new ClassScheduleViewModel() { ClassID = f.ClassID, ClassName = GetClassName(f.ClassID), ClassScheduleID = f.ClassScheduleID, LecturerID = f.LecturerID, Lecturer = GetLecturerName(f.LecturerID), DateStudy = f.DateStudy, RoomID = f.RoomID, SlotID = GetSlotbyID(f.SlotID), StudentID = GetStudent(f.StudentListID).StudentID, StudentListID = f.StudentListID, StudentName = GetStudent(f.StudentListID).StudentName, SubjectID = f.SubjectID, SubjectName = GetSubjectName(f.SubjectID) });

                int totalPage = (int)Math.Ceiling((double)NumberOfItems / (double)pageSize);

                ClassSchedulePageViewModel classSchedule = new ClassSchedulePageViewModel() { TotalPages = totalPage, ClassSchedules = firstPage.ToList() };

                return classSchedule;
            }
        }


        private StudentTBL GetCurrentStudent(string username)
        {
            using (var context = new IUContext())
            {
                var user = context.AspNetUsers.Where(u => u.UserName == username).FirstOrDefault();
                var student = context.StudentTBLs.Where(s => s.UserID == user.Id).FirstOrDefault();
                return student;
            }
        }

        private StudentListTBL GetCurrentStudentList(StudentTBL studentTBL, string semesterID)
        {
            using (var context = new IUContext())
            {
                var studentListTBL = context.StudentListTBLs.Where(u => u.StudentID == studentTBL.StudentID && u.SemesterID == semesterID).FirstOrDefault();
                return studentListTBL;
            }
        }

        private ClassTBL GetClass(string className)
        {
            using (var context = new IUContext())
            {
                var _class = context.ClassTBLs.Where(c => c.ClassName == className).FirstOrDefault();
                if (_class != null) return _class;
            }
            return null;
        }

        private SemesterTBL GetCurrentSemester()
        {
            using (var context = new IUContext())
            {
                var currentDate = DateTime.Now;
                var sem = context.SemesterTBLs.Where(obj => obj.StartDate <= currentDate && currentDate <= obj.EndDate);
                return sem.FirstOrDefault();
            }
        }

        private SubjectTBL GetSubjectByName(string abbreSubjectName)
        {
            using (var context = new IUContext())
            {
                var sub = context.SubjectTBLs.Where(obj => obj.AbbreSubjectName == abbreSubjectName);
                return sub.FirstOrDefault();
            }
        }

        private string[] GetStudentList(SemesterTBL sem, string userName)
        {
            using (var context = new IUContext())
            {
                var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                var student = context.StudentTBLs.Where(s=>s.UserID == user.Id).FirstOrDefault();
                var studentListTBL = context.StudentListTBLs.Where(obj => obj.SemesterID == sem.SemesterID && obj.StudentID == student.StudentID);
                return studentListTBL.Select(l=> l.StudentListID).ToArray();
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

        private string GetLecturerName(string LecturerID)
        {
            using (var context = new IUContext())
            {
                var _Lecturer = context.LecturerTBLs.Where(c => c.LecturerID == LecturerID).FirstOrDefault();
                if (_Lecturer != null) return _Lecturer.LecturerName;
            }
            return null;
        }

        private string GetSubjectName(string SubjectID)
        {
            using (var context = new IUContext())
            {
                var _Lecturer = context.SubjectTBLs.Where(c => c.SubjectID == SubjectID).FirstOrDefault();
                if (_Lecturer != null) return _Lecturer.SubjectName;
            }
            return null;
        }

        private StudentTBL GetStudent(string studentListID)
        {
            using (var context = new IUContext())
            {
                var student =
                (from studentTBLs in context.StudentTBLs
                join studentListTBLs in context.StudentListTBLs
                    on studentTBLs.StudentID equals studentListTBLs.StudentID
                 where studentListTBLs.StudentListID == studentListID
                 select studentTBLs).SingleOrDefault();

                return student;
            }
        }

        #region Dispose
        ~ScheduleClassService()
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
