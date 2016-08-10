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
    public class SubjectService : IDisposable
    {
        private IRepository<LecturerTBL> LecturerTBLRepository;
        private IRepository<ClassTBL> ClassRepository;
        private IRepository<OpenClassTBL> OpenClassRepository;
        private IRepository<OpenSubjectTBL> OpenSubjectTBLRepository;
        private IRepository<ClassScheduleTBL> ClassScheduleTBLRepository;
        private IRepository<StudentListTBL> StudentListTBLRepository;
        private IRepository<StudentTBL> StudentTBLRepository;
        private IRepository<AspNetUser> AccountRepository;
        private IRepository<SemesterClassSubjectTBL> SemesterClassSubjectTBLRepository;

        public SubjectService()
        {
            LecturerTBLRepository = new Repository<LecturerTBL>();
            ClassRepository = new Repository<ClassTBL>();
            OpenClassRepository = new Repository<OpenClassTBL>();
            OpenSubjectTBLRepository = new Repository<OpenSubjectTBL>();
            ClassScheduleTBLRepository = new Repository<ClassScheduleTBL>();
            StudentListTBLRepository = new Repository<StudentListTBL>();
            AccountRepository = new Repository<AspNetUser>();
            StudentTBLRepository = new Repository<StudentTBL>();
            SemesterClassSubjectTBLRepository = new Repository<SemesterClassSubjectTBL>();
        }

        public async Task<bool> CreateBisSync(BisViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => CreateBis(model, userName));
            }
        }

        private bool CreateBis(BisViewModel model, string userName)
        {
            try
            {
                string classID = Helper.GenerateRandomId();
                ClassRepository.Save(new ClassTBL() { ClassID = classID, ClassName = model.ClassName, CreateDate = DateTime.Now, Creater = userName, StartDate = model.StartDate });
                string openClassID = Helper.GenerateRandomId();
                OpenClassRepository.Save(new OpenClassTBL() { OpenClassID = openClassID, ClassID = classID, CreatedDate = DateTime.Now, Creater = userName, Deadline = model.Deadline, Limit = model.Limit, RoomID = GetRoom(model.RoomID), SemesterID = model.SemesterID, SlotID = string.Join("-", model.SlotIDs) });
                OpenSubjectTBLRepository.Save(new OpenSubjectTBL() { OpenSubjectID = Helper.GenerateRandomId(), OpenClassID = openClassID, LecturerID = model.LecturerID, ModeID = model.ModeID, SubjectID = model.SubjectID, StartDate = model.StartDate, Cost = 0, CreatedDate = DateTime.Now, Creater = userName, Credit = 0 });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }



        public async Task<bool> CreateLecturerSync(LecturerViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => CreateLecturer(model, userName));
            }
        }

        private bool CreateLecturer(LecturerViewModel model, string userName)
        {
            try
            {
                var signupUserModel = new AspNetUser
                {
                    Id = Helper.GenerateRandomId(),
                    Email = model.LecturerEmail,
                    EmailConfirmed = true,
                    PasswordHash = Helper.GetHash(model.Password),
                    PhoneNumber = model.LecturerPhone,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    UserName = model.LecturerID
                };
                
                AccountRepository.Save(signupUserModel);

                var lecture = new LecturerTBL() { LecturerID = Helper.GenerateRandomId(), UserID = signupUserModel.Id, LecturerBirth = DateTime.Now, LecturerEmail = model.LecturerEmail, LecturerGender = true, LecturerName = model.LecturerName, LecturerPhone = model.LecturerPhone };

                LecturerTBLRepository.Save(lecture);
                using (var context = new IUContext())
                {
                    var subject = context.SubjectTBLs.Where(s => s.SubjectID == model.SubjectID).FirstOrDefault();
                    var updateLecture =context.LecturerTBLs.Include("SubjectTBLs").Where(l => l.LecturerID == lecture.LecturerID).FirstOrDefault();
                    updateLecture.SubjectTBLs.Add(subject);
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> CreateStudentSync(StudentViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => CreateStudent(model, userName));
            }
        }

        private bool CreateStudent(StudentViewModel model, string userName)
        {
            try
            {
                var signupUserModel = new AspNetUser
                {
                    Id = Helper.GenerateRandomId(),
                    Email = model.StudentEmail,
                    EmailConfirmed = true,
                    PasswordHash = Helper.GetHash(model.Password),
                    PhoneNumber = model.StudentPhone,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    UserName = model.StudentID
                };
                
                AccountRepository.Save(signupUserModel);
                var student = new StudentTBL() { StudentID = Helper.GenerateRandomId(), ParentPhone = model.ParentPhone, StudentBirth = DateTime.Now, StudentCode = model.StudentID, StudentEmail = model.StudentEmail, StudentGender = true, StudentName = model.StudentName, StudentPhone = model.StudentPhone, UserID = signupUserModel.Id };

                StudentTBLRepository.Save(student);

                StudentListTBLRepository.Save(new StudentListTBL() { StudentListID = Helper.GenerateRandomId(), ClassID = model.ClassID, SemesterID = GetCurrentSemester().SemesterID, StudentID = student.StudentID });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> UpdateClassScheduleSync(ClassScheduleViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => UpdateClassSchedule(model, userName));
            }
        }

        private bool UpdateClassSchedule(ClassScheduleViewModel model, string userName)
        {
            try
            {
                var studentLists = StudentListTBLRepository.FindAllBy(s => s.ClassID == model.ClassID && s.SemesterID == model.SemesterID).ToList();
                foreach (StudentListTBL std in studentLists)
                {
                    var classSchedule = ClassScheduleTBLRepository.FindOneBy(c => c.ClassID == model.ClassID && c.StudentListID == std.StudentListID);
                    classSchedule.LecturerID = model.LecturerID;
                    classSchedule.ClassID = model.ClassID;
                    classSchedule.SubjectID = model.SubjectID;
                    classSchedule.SlotID = model.SlotID;
                    classSchedule.RoomID = model.RoomID;
                    ClassScheduleTBLRepository.Update(classSchedule);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
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

        public async Task<bool> CreateClassScheduleSync(ClassScheduleViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => CreateClassSchedule(model, userName));
            }
        }

        private bool CreateClassSchedule(ClassScheduleViewModel model, string userName)
        {
            try
            {
                DateTime[] studyDates;
                using (var context = new IUContext())
                {
                    var mode = context.ModeTBLs.Where(m => m.ModeID == model.ModeID).FirstOrDefault();
                    var sm = GetCurrentSemester();
                    studyDates = Helper.GetStudyDays(sm.StartDate.Value, sm.EndDate.Value, mode.Mode, model.BlogID);
                }

                

                var studentLists = StudentListTBLRepository.FindAllBy(s => s.ClassID == model.ClassID && s.SemesterID == model.SemesterID).ToList();
                foreach(StudentListTBL studentList in studentLists){
                    var classSchedule = new ClassScheduleTBL() { ClassScheduleID = Helper.GenerateRandomId(), ClassID = model.ClassID, RoomID = model.RoomID, SlotID = model.SlotID, SubjectID = model.SubjectID, LecturerID = model.LecturerID, StudentListID = studentList.StudentListID, IsAttendance = false, ModeID = model.ModeID, Blog = 1, DateStudy = model.DateStudy };
                    ClassScheduleTBLRepository.Save(classSchedule);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<List<ClassScheduleViewModel>> GetClassScheduleSync(string classID, string semesterID)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetClassSchedule(classID, semesterID));
            }
        }

        private List<ClassScheduleViewModel> GetClassSchedule(string classID, string semesterID)
        {
            try
            {
                using (var context = new IUContext())
                {
                    var sh = from t in (
                           from h in context.ClassScheduleTBLs
                           join u in context.StudentListTBLs on h.StudentListID equals u.StudentListID
                           where u.SemesterID == semesterID && u.ClassID == classID
                           select new {h,u}
                       )
                       group t by new { t.h.ClassID, t.h.LecturerID, t.h.SubjectID, t.h.RoomID, t.h.SlotID , t.h.DateStudy} into g
                       select new ClassScheduleViewModel(){
                            ClassID = classID,
                            LecturerID = g.Key.LecturerID,
                            SubjectID = g.Key.SubjectID,
                            RoomID = g.Key.RoomID,
                            SlotID = g.Key.SlotID ,
                            DateStudy = g.Key.DateStudy,
                            SemesterID = semesterID,
                            isCreate = false
                      };


                    List<ClassScheduleViewModel> lsData = new List<ClassScheduleViewModel>();
                    if(sh != null){
                        lsData.AddRange(sh.ToArray());
                    }

                    string[] lsSubjects = lsData.Select(s => s.SubjectID).ToArray();

                    var newSH = from h in context.SemesterClassSubjectTBLs
                                where h.SemesterID == semesterID && h.ClassID == classID
                                && !lsSubjects.Contains(h.SubjectID)
                                select new ClassScheduleViewModel()
                                {
                                    ClassID = h.ClassID,
                                    SubjectID = h.SubjectID,
                                    SemesterID = h.SemesterID,
                                    DateStudy = DateTime.Now,
                                    isCreate = true
                                };
                    if (newSH != null)
                    {
                        lsData.AddRange(newSH.ToArray());
                    }

                    return lsData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<ModeViewModel>> GetAllModesSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllModes());
            }
        }

        private List<ModeViewModel> GetAllModes()
        {
            using (var context = new IUContext())
            {
                var modes = context.ModeTBLs.Select(s => new ModeViewModel() { ModeID = s.ModeID, Mode = s.Mode, Decription = s.Decription });
                return modes.ToList();
            }
        }

        public async Task<List<RoomViewModel>> GetAllRoomsSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllRooms());
            }
        }

        private string GetRoom(string id)
        {
            using (var context = new IUContext())
            {
                return context.RoomTBLs.Where(r => r.RoomID == id).FirstOrDefault().RomName;
            }
        }

        private List<RoomViewModel> GetAllRooms()
        {
            using (var context = new IUContext())
            {
                var rooms = context.RoomTBLs.Select(s => new RoomViewModel() { RoomID = s.RoomID, RomName = s.RomName, Decription = s.Decription });
                return rooms.ToList();
            }
        }

        public async Task<List<SlotViewModel>> GetAllSlotsSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllSlots());
            }
        }

        private List<SlotViewModel> GetAllSlots()
        {
            using (var context = new IUContext())
            {
                var slots = context.SlotTBLs.Select(s => new SlotViewModel() { SlotID = s.SlotID, SlotName = s.SlotName, SlotTime = s.SlotTime, NumOfSlot = s.NumOfSlot.Value, TotalSlot = s.TotalSlot.Value });
                return slots.ToList();
            }
        }

        public async Task<List<UserSubjectViewModel>> GetAllSubjectsSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllSubjects());
            }
        }

        private List<UserSubjectViewModel> GetAllSubjects()
        {
            using (var context = new IUContext())
            {
                var subjects = context.SubjectTBLs.Select(s => new UserSubjectViewModel() { SubjectID = s.SubjectID, SubjectName = s.SubjectName, AbbreSubjectName = s.AbbreSubjectName});
                return subjects.ToList();
            }
        }

        public async Task<List<UserSemesterViewModel>> GetAllSemesterSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllSemester());
            }
        }

        private List<UserSemesterViewModel> GetAllSemester()
        {
            using (var context = new IUContext())
            {
                var subjects = context.SemesterTBLs.Select(s => new UserSemesterViewModel() { SemesterID = s.SemesterID, SemesterName = s.SemesterName, SemesterCode = s.SemesterCode, StartDate = s.StartDate, EndDate = s.EndDate });
                return subjects.ToList();
            }
        }

        public async Task<List<ClassViewModel>> GetAllClassSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllClass());
            }
        }

        private List<ClassViewModel> GetAllClass()
        {
            using (var context = new IUContext())
            {
                var classes = context.ClassTBLs.Select(s => new ClassViewModel() { ClassID = s.ClassID, ClassName = s.ClassName });
                return classes.ToList();
            }
        }

        public async Task<List<LecturerViewModel>> GetAllLecturerSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetAllLecturer());
            }
        }

        private List<LecturerViewModel> GetAllLecturer()
        {
            using (var context = new IUContext())
            {
                var lecturers = context.LecturerTBLs.Select(s => new LecturerViewModel() { LecturerID = s.LecturerID, LecturerName = s.LecturerName, LecturerPhone = s.LecturerPhone, LecturerBirth = s.LecturerBirth, LecturerEmail = s.LecturerEmail, LecturerGender = s.LecturerGender, UserID = s.UserID });
                return lecturers.ToList();
            }
        }

        public async Task<List<UserSubjectViewModel>> GetSubjectByLecturerSync(string userId)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetSubjectByLecturer(userId));
            }
        }

        private List<UserSubjectViewModel> GetSubjectByLecturer(string userId)
        {
            using (var context = new IUContext())
            {
                var lecturer = context.LecturerTBLs.Include("SubjectTBLs").Where(l => l.UserID == userId).FirstOrDefault();
                var subjects = lecturer.SubjectTBLs.Select(s => new UserSubjectViewModel() { SubjectID = s.SubjectID, SubjectName = s.SubjectName, AbbreSubjectName = s.AbbreSubjectName, UserId = userId });
                return subjects.ToList();
            }
        }

        public async Task<List<UserSubjectViewModel>> GetSubjectByStudentSync(string userId)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetSubjectByStudent(userId));
            }
        }

        private List<UserSubjectViewModel> GetSubjectByStudent(string userId)
        {
            using (var context = new IUContext())
            {

                var subjects =
                 from subjectTBLs in context.SubjectTBLs
                 join classScheduleTBLs in context.ClassScheduleTBLs
                     on subjectTBLs.SubjectID equals classScheduleTBLs.SubjectID
                 join studentListTBLs in context.StudentListTBLs
                     on classScheduleTBLs.StudentListID equals studentListTBLs.StudentListID
                 join studentTBLs in context.StudentTBLs
                    on studentListTBLs.StudentID equals studentTBLs.StudentID
                 where studentTBLs.UserID == userId
                 select new UserSubjectViewModel() { SubjectID = subjectTBLs.SubjectID, SubjectName = subjectTBLs.SubjectName, UserId = userId, AbbreSubjectName = subjectTBLs.AbbreSubjectName  };

                return subjects.Distinct().ToList();
            }
        }

        #region Dispose
        ~SubjectService()
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
