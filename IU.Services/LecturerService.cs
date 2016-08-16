using IU.Domain;
using IU.Services.Repositories;
using IU.Services.Utilities;
using IU.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace IU.Services
{
    public class LecturerService : IDisposable
    {
        private IRepository<AttendanceTBL> AttendanceTBLRepository;
        private IRepository<AspNetUser> AspNetUserRepository;
        private IRepository<ClassScheduleTBL> ClassScheduleTBLRepository;

        public LecturerService()
        {
            AttendanceTBLRepository = new Repository<AttendanceTBL>();
            AspNetUserRepository = new Repository<AspNetUser>();
            ClassScheduleTBLRepository = new Repository<ClassScheduleTBL>();
        }

        public async Task<LectureClassSubjectViewModel> GetLectureClassSubjectSync(string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetLectureClassSubject(userName));
            }
        }

        private LectureClassSubjectViewModel GetLectureClassSubject(string userName)
        {
            AspNetUserViewModel userInfo = null;
            using (UserService service = new UserService())
            {
                userInfo = service.GetUserInfo(userName);
            }

            using (var context = new IUContext())
            {
                if (userInfo.AccountType.Equals("Admin"))
                {
                    var _subjects = context.SubjectTBLs.ToList();

                    var subjects = _subjects.Select(s => new UserSubjectViewModel() { SubjectID = s.SubjectID, SubjectName = s.SubjectName, AbbreSubjectName = s.AbbreSubjectName}).ToList();

                    var classs = context.ClassTBLs.ToList();
                    var lecClass = classs.Select(c => new ClassViewModel() { ClassID = c.ClassID, ClassName = GetClass(c.ClassID).ClassName }).ToList();

                    var lectures = context.LecturerTBLs.Select(l => new LecturerViewModel() { LecturerID = l.LecturerID, LecturerBirth = l.LecturerBirth, LecturerEmail = l.LecturerEmail, LecturerGender = l.LecturerGender, LecturerName = l.LecturerName, LecturerPhone = l.LecturerPhone, UserID = l.UserID }).ToList();

                    LectureClassSubjectViewModel model = new LectureClassSubjectViewModel() { LectureClass = lecClass, LectureSubject = subjects, Lectures = lectures };
                    return model;
                }
                else
                {
                    var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                    var lectures = context.LecturerTBLs.Include("SubjectTBLs").Include("LecturerScheduleTBLs").Where(l => l.UserID == user.Id).FirstOrDefault();

                    var subjects = lectures.SubjectTBLs.Select(s => new UserSubjectViewModel() { SubjectID = s.SubjectID, SubjectName = s.SubjectName, AbbreSubjectName = s.AbbreSubjectName, UserId = user.Id }).ToList();

                    var classs = lectures.LecturerScheduleTBLs.Where(c => c.SemesterID == GetCurrentSemester().SemesterID).ToList();
                    var lecClass = classs.GroupBy(n => new { n.ClassID}).Distinct().Select(c => new ClassViewModel() { ClassID = c.Key.ClassID, ClassName = GetClass(c.Key.ClassID).ClassName }).ToList();


                    LectureClassSubjectViewModel model = new LectureClassSubjectViewModel() { LectureClass = lecClass, LectureSubject = subjects };
                    return model;
                }
               
            }
        }

        public async Task<PreviewListViewModel> GetLecturerPreviewSync(string userName, string classID, string subjectID, string lectureID)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetLecturerPreview(userName, classID, subjectID, lectureID));
            }
        }

        private PreviewListViewModel GetLecturerPreview(string userName, string classID, string subjectID, string lectureID)
        {
            AspNetUserViewModel userInfo = null;
            using (UserService service = new UserService())
            {
                userInfo = service.GetUserInfo(userName);
            }

            using (var context = new IUContext())
            {
                var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                //var lecture = context.LecturerTBLs.Where(l => l.UserID == user.Id).FirstOrDefault();

                List<AttendanceTBL> attandances = new List<AttendanceTBL>();

                if (userInfo.AccountType.Equals("Admin") && lectureID != null)
                {
                    var userID = context.LecturerTBLs.Where(l => l.LecturerID == lectureID).FirstOrDefault().UserID;
                    attandances = AttendanceTBLRepository.FindAllBy(a => a.ClassID == classID && a.SubjectID == subjectID && a.Attendancer == userID).ToList();
                }
                else
                {
                    attandances = AttendanceTBLRepository.FindAllBy(a => a.ClassID == classID && a.SubjectID.Trim() == subjectID && a.Attendancer == user.Id).ToList();
                }

                var previewViewModels2 = context.ClassScheduleTBLs.Where(c => c.ClassID.Replace("\r\n", string.Empty) == classID.Replace("\r\n", string.Empty)
                    && c.SubjectID.Replace("\r\n", string.Empty) == subjectID.Replace("\r\n", string.Empty)).OrderBy(p => p.DateStudy).GroupBy(n => new { n.DateStudy, n.SlotID })
                            .Select(a => new PreviewViewModel()
                            {
                                Day = a.Key.DateStudy.Day + "/" + a.Key.DateStudy.Month,
                                Slot = a.Key.SlotID
                            }).ToList().Select(a => new PreviewViewModel()
                            {
                                Day = a.Day,
                                Slot = GetSlot(a.Slot)
                            }).ToList();

                var previewViewModels = attandances.Select(a => new PreviewViewModel() 
                { Day = a.DateAttendance.Day + "/" + a.DateAttendance.Month, Slot = GetSlot(a.SlotID), Status = (a.Attendance == true ? "P" : "A"),
                    StudentName = GetStudent(a.StudentListID).StudentName }).OrderBy(p => p.Day).ToList();


                var rows = previewViewModels
                    .GroupBy(lab =>
                            new
                            {
                                StudentName = lab.StudentName
                            }
                            )
                    .Select(labtests =>
                            new
                            {
                                StudentName = labtests.Key.StudentName
                            }
                );

                var query = previewViewModels2.GroupBy(n => n.Day,
                (key, values) => new { Day = key, Count = values.Count() }).ToList();

                List<PreviewViewModel> lisWithCount = new List<PreviewViewModel>();
                foreach (PreviewViewModel m in previewViewModels2)
                {
                    var countDay = query.Where(q => q.Day == m.Day).FirstOrDefault();
                    var existDayInList = lisWithCount.Where(e => e.Day == m.Day).FirstOrDefault();
                    if (existDayInList == null && countDay != null)
                    {
                        m.ColSpan = countDay.Count;
                        lisWithCount.Add(m);
                    }
                }

                PreviewListViewModel model = new PreviewListViewModel() { ClassID = classID, SubjectID = subjectID, Preview = lisWithCount, Data = new List<LecturePreviewViewModel>(), CurrentSemmeter = GetCurrentSemester().SemesterName };
                int index = 0;
                foreach (var studnt in rows)
                {
                    LecturePreviewViewModel row = null;
                    
                    var _cols = previewViewModels.Where(p => p.StudentName == studnt.StudentName).OrderBy(p=>p.Day).ToList();
                    if (index == 0)
                    {
                        List<PreviewViewModel> ls = new List<PreviewViewModel>();
                        foreach (var modl in previewViewModels2)
                        {
                            if (modl.Day == _cols[0].Day)
                            {
                                break;
                            }
                            ls.Add(modl);
                        }
                        ls.AddRange(_cols);
                        row = new LecturePreviewViewModel() { Row = studnt.StudentName, Cols = ls };
                    }
                    else
                    {
                        row = new LecturePreviewViewModel() { Row = studnt.StudentName, Cols = _cols };
                    }
                    
                    
                   
                    
                    model.Data.Add(row);
                }

                
                return model;
            }
        }


        

        public async Task<bool> TakeAttendancesSync(List<UserAttendanceViewModel> models, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => TakeAttendances(models, userName));
            }
        }

        private bool TakeAttendances(List<UserAttendanceViewModel> models, string userName)
        {
            var user = AspNetUserRepository.FindOneBy(u=>u.UserName == userName);
            foreach (UserAttendanceViewModel model in models)
            {
                if(model.isAttendanced){
                    var attendance = AttendanceTBLRepository.FindOneBy(a => a.AttendanceID == model.AttendanceID);
                    attendance.Note = model.Note;
                    attendance.DateAttendance = model.DateStudy;
                    attendance.Attendance = model.Present;
                    AttendanceTBLRepository.Update(attendance);
                }
                else
                {
                    AttendanceTBLRepository.Save(new AttendanceTBL() { AttendanceID = Helper.GenerateRandomId(), Attendance = model.Present, Attendancer = user.Id.Replace("\r\n", string.Empty), ClassID = model.ClassID.Replace("\r\n", string.Empty), DateAttendance = model.DateStudy, Note = model.Note, RoomID = model.RoomID, SemesterID = model.SemesterID, SlotID = model.SlotID, SubjectID = model.SubjectID, StudentListID = model.StudentListID });

                    var classTbl = ClassScheduleTBLRepository.FindOneBy(c => c.SubjectID.Replace("\r\n", string.Empty) == model.SubjectID.Replace("\r\n", string.Empty) && c.StudentListID.Replace("\r\n", string.Empty) == model.StudentListID.Replace("\r\n", string.Empty) && c.DateStudy == model.DateStudy);
                    if (classTbl != null)
                    {
                        classTbl.IsAttendance = true;
                        ClassScheduleTBLRepository.Update(classTbl);
                    }
                    
                }

                
                
            }

            return true;
        }


        public async Task<List<UserAttendanceViewModel>> GetTakeAttendancesSync(string subjectID, string semesterID, string classID, DateTime dateStudy, bool isAttendanced, string slotID)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetTakeAttendances(subjectID, semesterID, classID, dateStudy, isAttendanced, slotID));
            }
        }


        private List<UserAttendanceViewModel> GetTakeAttendances(string subjectID, string semesterID, string classID, DateTime dateStudy, bool isAttendanced, string slotID)
        {
            List<UserAttendanceViewModel> lsAttendance = new List<UserAttendanceViewModel>();
            using (var context = new IUContext())
            {
                if (!isAttendanced)
                {
                    var attendances =
                      (from classScheduleTBLs in context.ClassScheduleTBLs
                       join classTBLs in context.ClassTBLs
                           on classScheduleTBLs.ClassID equals classTBLs.ClassID
                       join subjectTBLs in context.SubjectTBLs
                           on classScheduleTBLs.SubjectID equals subjectTBLs.SubjectID
                       join studentListTBLs in context.StudentListTBLs
                           on classScheduleTBLs.StudentListID equals studentListTBLs.StudentListID
                       join studentTBLs in context.StudentTBLs
                           on studentListTBLs.StudentID equals studentTBLs.StudentID
                       join aspNetUsers in context.AspNetUsers
                           on studentTBLs.UserID equals aspNetUsers.Id
                       where studentListTBLs.SemesterID == semesterID
                       && studentListTBLs.ClassID == classID
                       && classScheduleTBLs.SubjectID == subjectID
                       && classScheduleTBLs.DateStudy.Year == dateStudy.Year
                              && classScheduleTBLs.DateStudy.Month == dateStudy.Month
                              && classScheduleTBLs.DateStudy.Day == dateStudy.Day
                              && classScheduleTBLs.IsAttendance == false
                              && classScheduleTBLs.SlotID == slotID
                       select new UserAttendanceViewModel() { Avata = aspNetUsers.Image, ClassID = classID, StudentID = studentTBLs.StudentID, StudentName = studentTBLs.StudentName, UserID = studentTBLs.UserID, SlotID = classScheduleTBLs.SlotID, RoomID = classScheduleTBLs.RoomID, StudentListID = studentListTBLs.StudentListID, SemesterID = semesterID, SubjectID = classScheduleTBLs.SubjectID, ClassName = classTBLs.ClassName, SubjectName = subjectTBLs.SubjectName, SubjectCode = subjectTBLs.AbbreSubjectName, DateStudy = classScheduleTBLs.DateStudy, isAttendanced = false });
                            return attendances.ToList();
                }
                else
                {
                    var attendances =
                      (from attendanceTBLs in context.AttendanceTBLs
                       join classTBLs in context.ClassTBLs
                           on attendanceTBLs.ClassID equals classTBLs.ClassID
                       join subjectTBLs in context.SubjectTBLs
                           on attendanceTBLs.SubjectID equals subjectTBLs.SubjectID
                       join studentListTBLs in context.StudentListTBLs
                           on attendanceTBLs.StudentListID equals studentListTBLs.StudentListID
                       join studentTBLs in context.StudentTBLs
                           on studentListTBLs.StudentID equals studentTBLs.StudentID
                       join aspNetUsers in context.AspNetUsers
                           on studentTBLs.UserID equals aspNetUsers.Id
                       where studentListTBLs.SemesterID == semesterID
                       && studentListTBLs.ClassID == classID
                       && attendanceTBLs.SubjectID == subjectID
                       && attendanceTBLs.DateAttendance.Year == dateStudy.Year
                              && attendanceTBLs.DateAttendance.Month == dateStudy.Month
                              && attendanceTBLs.DateAttendance.Day == dateStudy.Day
                              && attendanceTBLs.SlotID == slotID
                       select new UserAttendanceViewModel() { Avata = aspNetUsers.Image, ClassID = classID, StudentID = studentTBLs.StudentID, StudentName = studentTBLs.StudentName, UserID = studentTBLs.UserID, SlotID = attendanceTBLs.SlotID, RoomID =attendanceTBLs.RoomID, StudentListID = studentListTBLs.StudentListID, SemesterID = semesterID, SubjectID = attendanceTBLs.SubjectID, ClassName = classTBLs.ClassName, SubjectName = subjectTBLs.SubjectName, SubjectCode = subjectTBLs.AbbreSubjectName, isAttendanced = true, Attendance = attendanceTBLs.Attendance, Present = attendanceTBLs.Attendance, DateStudy = dateStudy, Note = attendanceTBLs.Note, AttendanceID = attendanceTBLs.AttendanceID, Attendancer = attendanceTBLs.Attendancer, DateAttendance = attendanceTBLs.DateAttendance });
                    return attendances.ToList();
                }
               
            }
        }

        public async Task<List<UserAttendanceViewModel>> GetAttendancesSync(string userName, int type)
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
                       && x.DateAttendance.Day == date.Day).GroupBy(x => new { x.SubjectID, x.ClassID, x.RoomID, x.SlotID }, (key, group) => new
                       {
                           ClassID = key.ClassID,
                           RoomID = key.RoomID,
                           SubjectID = key.SubjectID,
                           SlotID = key.SlotID,
                           Result = group.ToList()
                       }).ToList().Distinct();

                    var attendances = results.ToList().Select(a => new UserAttendanceViewModel() { SubjectName = GetSubjectName(a.SubjectID), SubjectID = a.SubjectID, ClassName = GetClass(a.ClassID).ClassName, ClassID = a.ClassID, RoomID = GetRoom(a.RoomID), SlotID = a.SlotID, SlotTime = GetSlotbyID(a.SlotID), SemesterID = GetCurrentSemester().SemesterID, isAttendanced = true, DateAttendance = date });

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

        private string GetRoom(string id)
        {
            using (var context = new IUContext())
            {
                if (id.Length == 3) return id;
                return context.RoomTBLs.Where(r => r.RoomID == id).FirstOrDefault().RomName;
            }
        }

        private List<UserAttendanceViewModel> GetClassSchedule(string lecturerID, string[] studentListIDs, DateTime[] dates, IUContext context)
        {
            List<UserAttendanceViewModel> lsAttendance = new List<UserAttendanceViewModel>();
            var semesterID = GetCurrentSemester().SemesterID;
            foreach (DateTime date in dates)
            {
                var results = context.ClassScheduleTBLs.Where(x => x.LecturerID == lecturerID && studentListIDs.Contains(x.StudentListID) && x.DateStudy.Year == date.Year
                   && x.DateStudy.Month == date.Month
                   && x.DateStudy.Day == date.Day
                   && x.IsAttendance == true
                   ).GroupBy(x => new { x.SubjectID, x.ClassID, x.RoomID, x.SlotID, x.DateStudy }, (key, group) => new
                   {
                       ClassID = key.ClassID,
                       RoomID = key.RoomID,
                       SubjectID = key.SubjectID,
                       SlotID = key.SlotID,
                       DateStudy = key.DateStudy,
                       Result = group.ToList()
                   }).ToList().Distinct();

                if (results != null)
                {
                    if (results.ToArray().Count() == 0)
                    {
                        results = context.ClassScheduleTBLs.Where(x => x.LecturerID == lecturerID && studentListIDs.Contains(x.StudentListID) && x.DateStudy.Year == date.Year
                           && x.DateStudy.Month == date.Month
                           && x.DateStudy.Day == date.Day
                           && x.IsAttendance == false
                           ).GroupBy(x => new { x.SubjectID, x.ClassID, x.RoomID, x.SlotID, x.DateStudy }, (key, group) => new
                           {
                               ClassID = key.ClassID,
                               RoomID = key.RoomID,
                               SubjectID = key.SubjectID,
                               SlotID = key.SlotID,
                               DateStudy = key.DateStudy,
                               Result = group.ToList()
                           }).ToList().Distinct();

                        var attendances = results.ToList().Select(a => new UserAttendanceViewModel() { SubjectName = GetSubjectName(a.SubjectID), SubjectID = a.SubjectID, ClassName = GetClass(a.ClassID).ClassName, ClassID = a.ClassID, RoomID = GetRoom(a.RoomID), SlotID = a.SlotID, SlotTime = GetSlotbyID(a.SlotID), SemesterID = GetCurrentSemester().SemesterID, isAttendanced = false, DateStudy = a.DateStudy });
                        if (attendances != null)
                            lsAttendance.AddRange(attendances.ToArray());
                    }
                    
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
                List<string> slNa = new List<string>();
                if(SlotID.IndexOf("-")>=0){
                    foreach(string sl in SlotID.Split('-')){
                        var slot = context.SlotTBLs.Where(s => s.SlotID == sl).FirstOrDefault();
                        slNa.Add(slot.SlotName);
                    }
                    return string.Join("-", slNa);
                }
                else
                {
                    var slot = context.SlotTBLs.Where(s => s.SlotID == SlotID).FirstOrDefault();
                    return slot.SlotName;
                }
                
            }
        }
        private string GetSlot(string SlotID)
        {

            return SlotID;
            //using (var context = new IUContext())
            //{
            //    return context.SlotTBLs.Where(s => s.SlotID == SlotID).FirstOrDefault().SlotName;
            //}
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
                 select studentTBLs).FirstOrDefault();

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
