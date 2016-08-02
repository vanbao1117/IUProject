using IU.Domain;
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
    public class LecturerService : IDisposable
    {
        public async Task<List<UserAttendanceViewModel>> GetTakeAttendancesSync(string subjectID, string semesterID, string classID)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetTakeAttendances(subjectID, semesterID, classID));
            }
        }


        private List<UserAttendanceViewModel> GetTakeAttendances(string subjectID, string semesterID, string classID)
        {
            List<UserAttendanceViewModel> lsAttendance = new List<UserAttendanceViewModel>();
            using (var context = new IUContext())
            {
                var attendances =
               (from classScheduleTBLs in context.ClassScheduleTBLs
                join studentListTBLs in context.StudentListTBLs
                    on classScheduleTBLs.StudentListID equals studentListTBLs.StudentListID
                join studentTBLs in context.StudentTBLs
                    on studentListTBLs.StudentID equals studentTBLs.StudentID
                join aspNetUsers in context.AspNetUsers
                    on studentTBLs.UserID equals aspNetUsers.Id
                where studentListTBLs.SemesterID == semesterID
                && studentListTBLs.ClassID == classID
                && classScheduleTBLs.SubjectID == subjectID
                select new UserAttendanceViewModel() { Avata = aspNetUsers.Image, ClassID = classID, StudentID = studentTBLs.StudentID, StudentName = studentTBLs.StudentName, UserID = studentTBLs.UserID, SlotID = classScheduleTBLs.SlotID, RoomID = classScheduleTBLs.RoomID, StudentListID = studentListTBLs.StudentListID, SemesterID = semesterID, SubjectID = classScheduleTBLs.SubjectID  });
            }
        }

        public async Task<List<UserAttendanceViewModel>> GeAttendancesSync(string userName, int type)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAttendances(userName, type));
            }
        }

        private List<UserAttendanceViewModel> GetAttendances(string userName, int type)
        {
            List<UserAttendanceViewModel> lsAttendance = new List<UserAttendanceViewModel>();
            using (var context = new IUContext())
            {
                List<DateTime> dates = new List<DateTime>();
                if (type == 0)
                {
                    dates.AddRange(Helper.GetTwoDaysBefore());
                }
                else if (type == 1)
                {
                    dates.Add(DateTime.Now);
                    //Get more from class schedule
                }
                else if (type == 2)
                {
                    DateTime now = DateTime.Now;
                    DateTime one = now.AddDays(1);
                    DateTime two = now.AddDays(2);
                    dates.Add(one);
                    dates.Add(two);

                    //Get from class schedule
                }
                
                var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                var semesterID = GetCurrentSemester().SemesterID;
                foreach (DateTime date in dates)
                {
                    var results = context.AttendanceTBLs.Where(x => x.Attendancer == user.Id && x.SemesterID == semesterID && x.DateAttendance.Year == date.Year
                       && x.DateAttendance.Month == date.Month
                       && x.DateAttendance.Day == date.Day).GroupBy(x => new { x.SubjectID, x.ClassID, x.RoomID, x.SlotID, x.Note }, (key, group) => new
                       {
                           ClassID = key.ClassID,
                           RoomID = key.RoomID,
                           SubjectID = key.SubjectID,
                           SlotID = key.SlotID,
                           Note = key.Note,
                           Result = group.ToList()
                       }).ToList().Distinct();

                    var attendances = results.ToList().Select(a => new UserAttendanceViewModel() { SubjectName = GetSubjectName(a.SubjectID), SubjectID = a.SubjectID, ClassName = GetClass(a.ClassID).ClassName, ClassID = a.ClassID, RoomID = a.RoomID, SlotID = a.SlotID, SlotTime = GetSlotbyID(a.SlotID), Note = a.Note, SemesterID = GetCurrentSemester().SemesterID, isAttendanced = true });

                    lsAttendance.AddRange(attendances.ToArray());
                }

                if (type == 1 || type == 0)
                {
                    var lecture = GetLecturer(user.Id);
                    string[] studentlistIds = GetStudentList();
                    List<UserAttendanceViewModel> lsClassSchedule = GetClassSchedule(lecture.LecturerID, studentlistIds, dates.ToArray(), context);
                    if (lsClassSchedule.Count > 0) lsAttendance.AddRange(lsClassSchedule);
                }
                else if (type == 2)
                {
                    var lecture = GetLecturer(user.Id);
                    string[] studentlistIds = GetStudentList();
                    List<UserAttendanceViewModel> lsClassSchedule = GetClassSchedule(lecture.LecturerID, studentlistIds, dates.ToArray(), context);
                    if (lsClassSchedule.Count > 0) lsAttendance.AddRange(lsClassSchedule);
                }
                
            }

            return lsAttendance;
        }

        private List<UserAttendanceViewModel> GetClassSchedule(string lecturerID, string[] studentListIDs, DateTime[] dates, IUContext context)
        {
            List<UserAttendanceViewModel> lsAttendance = new List<UserAttendanceViewModel>();
            var semesterID = GetCurrentSemester().SemesterID;
            foreach (DateTime date in dates)
            {
                var results = context.ClassScheduleTBLs.Where(x => x.LecturerID == lecturerID && studentListIDs.Contains(x.StudentListID) && x.DateStudy.Year == date.Year
                   && x.DateStudy.Month == date.Month
                   && x.DateStudy.Day == date.Day).GroupBy(x => new { x.SubjectID, x.ClassID, x.RoomID, x.SlotID }, (key, group) => new
                   {
                       ClassID = key.ClassID,
                       RoomID = key.RoomID,
                       SubjectID = key.SubjectID,
                       SlotID = key.SlotID,
                       Note = string.Empty,
                       Result = group.ToList()
                   }).ToList().Distinct();

                if (results != null)
                {
                    var attendances = results.ToList().Select(a => new UserAttendanceViewModel() { SubjectName = GetSubjectName(a.SubjectID), SubjectID = a.SubjectID, ClassName = GetClass(a.ClassID).ClassName, ClassID = a.ClassID, RoomID = a.RoomID, SlotID = a.SlotID, SlotTime = GetSlotbyID(a.SlotID), Note = a.Note, SemesterID = GetCurrentSemester().SemesterID, isAttendanced = false });
                    if (attendances != null)
                        lsAttendance.AddRange(attendances.ToArray());
                }
                
            }

            return lsAttendance;
        }

        private string[] GetStudentList()
        {
            using (var context = new IUContext())
            {
                var semesterID = GetCurrentSemester().SemesterID;

                var _class = context.StudentListTBLs.Where(c => c.SemesterID == semesterID).Select(s=>s.StudentListID);
                return _class.ToArray();
            }
        }

        private string GetSlotbyID(string SlotID)
        {
            using (var context = new IUContext())
            {
                return context.SlotTBLs.Where(s => s.SlotID == SlotID).FirstOrDefault().SlotTime;
            }
        }

        private ClassTBL GetClass(string ClassID)
        {
            using (var context = new IUContext())
            {
                var _class = context.ClassTBLs.Where(c => c.ClassID == ClassID).FirstOrDefault();
                if (_class != null) return _class;
            }
            return null;
        }

        private LecturerTBL GetLecturer(string UserID)
        {
            using (var context = new IUContext())
            {
                var _Lecturer = context.LecturerTBLs.Where(c => c.UserID == UserID).FirstOrDefault();
                return _Lecturer;
            }
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

        private SemesterTBL GetCurrentSemester()
        {
            using (var context = new IUContext())
            {
                var currentDate = DateTime.Now;
                var sem = context.SemesterTBLs.Where(obj => obj.StartDate <= currentDate && currentDate <= obj.EndDate);
                return sem.FirstOrDefault();
            }
        }

        public async Task<List<LecturerViewModel>> GetLecturersScheduleSync(string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetLecturersSchedule(userName));
            }
        }

        private List<LecturerViewModel> GetLecturersSchedule(string userName)
        {
            using (var context = new IUContext())
            {
                var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                var lecturer =
                 from lecturerTBLs in context.LecturerTBLs
                 where lecturerTBLs.UserID == user.Id
                 select new LecturerViewModel() { LecturerID = lecturerTBLs.LecturerID, LecturerBirth = lecturerTBLs.LecturerBirth, LecturerEmail = lecturerTBLs.LecturerEmail, LecturerGender = lecturerTBLs.LecturerGender, LecturerName = lecturerTBLs.LecturerName, LecturerPhone = lecturerTBLs.LecturerPhone, UserID = lecturerTBLs.UserID };

                return null;
            }
        }

        public async Task<List<LecturerViewModel>> GetLecturersSync(string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetLecturers(userName));
            }
        }

        private List<LecturerViewModel> GetLecturers(string userName)
        {
            using (var context = new IUContext())
            {
                var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                var lecturers =
                 from lecturerTBLs in context.LecturerTBLs
                 join classScheduleTBLs in context.ClassScheduleTBLs
                     on lecturerTBLs.LecturerID equals classScheduleTBLs.LecturerID
                 join studentListTBLs in context.StudentListTBLs
                     on classScheduleTBLs.StudentListID equals studentListTBLs.StudentListID
                 join studentTBLs in context.StudentTBLs
                    on studentListTBLs.StudentID equals studentTBLs.StudentID
                 where studentTBLs.UserID == user.Id

                 select new LecturerViewModel() { LecturerID = lecturerTBLs.LecturerID, LecturerBirth = lecturerTBLs.LecturerBirth, LecturerEmail = lecturerTBLs.LecturerEmail, LecturerGender = lecturerTBLs.LecturerGender, LecturerName = lecturerTBLs.LecturerName, LecturerPhone = lecturerTBLs.LecturerPhone, UserID = lecturerTBLs.UserID };

                return lecturers.Distinct().ToList();
            }
        }

        #region Dispose
        ~LecturerService()
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
            }
            // dispose the unmanaged objects
        }

        #endregion
    }
}
