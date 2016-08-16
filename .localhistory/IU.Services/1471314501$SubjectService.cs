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
        private IRepository<LecturerScheduleTBL> LecturerScheduleTBLRepository;

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
            LecturerScheduleTBLRepository = new Repository<LecturerScheduleTBL>();
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
                ClassRepository.Save(new ClassTBL() { ClassID = classID, ClassName = model.ClassName, CreateDate = DateTime.Now, Creater = userName, StartDate = model.StartDate, IsMainClass = false });
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

        public async Task<bool> UpdateClassScheduleSync(UpdateClassSchedulePageViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => UpdateClassSchedule(model, userName));
            }
        }

        private bool UpdateClassSchedule(UpdateClassSchedulePageViewModel model, string userName)
        {
            try
            {
                
                var studentLists = StudentListTBLRepository.FindAllBy(s => s.ClassID == model.OldModel.ClassID && s.SemesterID == model.OldModel.SemesterID).ToList();

                if (!model.IsNewSchedule)
                {
                    //foreach (StudentListTBL std in studentLists)
                    //{
                        //var classSchedule = ClassScheduleTBLRepository.FindOneBy(c => c.LecturerID == model.OldModel.LecturerID && c.StudentListID == std.StudentListID
                        //    && c.ClassID == model.OldModel.ClassID
                        //    && c.SubjectID == model.OldModel.SubjectID
                        //    && c.SlotID == model.OldModel.SlotID1 + (model.OldModel.SlotID2.Equals("") ? "" : "-" + model.OldModel.SlotID2)
                        //    && c.RoomID == model.OldModel.RoomID
                        //    && c.Blog == model.OldModel.BlogID
                        //    && c.ModeID == model.OldModel.ModeID);

                       

                        //ClassScheduleTBLRepository.Delete(classSchedule);
                    //}

                    var classSchedules = ClassScheduleTBLRepository.FindAllBy(c => c.LecturerID == model.OldModel.LecturerID 
                            && c.ClassID.Replace("\r\n", string.Empty) == model.OldModel.ClassID.Replace("\r\n", string.Empty)
                            && c.SubjectID.Replace("\r\n", string.Empty) == model.OldModel.SubjectID.Replace("\r\n", string.Empty)
                            && c.RoomID.Replace("\r\n", string.Empty) == model.OldModel.RoomID.Replace("\r\n", string.Empty)
                            && c.Blog == model.OldModel.BlogID
                            && c.SlotID == model.OldModel.SlotID1 + (model.OldModel.SlotID2.Equals("") ? "" : "-" + model.OldModel.SlotID2)
                            && c.ModeID == model.OldModel.ModeID).ToList();
                    foreach (var classclassSchedule in classSchedules)
                    {
                        ClassScheduleTBLRepository.Delete(classclassSchedule);
                    }
                }
               

                CreateClassSchedule(model.NewModel, userName);
                
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
                LecturerScheduleTBLRepository.Save(new LecturerScheduleTBL() { LecturerSchedule = Helper.GenerateRandomId(), ClassID = model.ClassID, LecturerID = model.LecturerID, SemesterID = model.SemesterID, SubjectID = model.SubjectID, StartDate = DateTime.Now });
                using (var context = new IUContext())
                {
                    var mode = context.ModeTBLs.Where(m => m.ModeID == model.ModeID).FirstOrDefault();
                    var sm = GetCurrentSemester();
                    DateTime[] studyDates = Helper.GetStudyDays(sm.StartDate.Value, sm.EndDate.Value, mode.Mode, model.BlogID);

                    foreach (DateTime dateStudy in studyDates)
                    {
                        var studentLists = StudentListTBLRepository.FindAllBy(s => s.ClassID == model.ClassID && s.SemesterID == model.SemesterID).ToList();
                        foreach (StudentListTBL studentList in studentLists)
                        {
                            if (model.SlotID2 == null && model.SlotID1 == null) break;

                            if(model.SlotID2 == null){
                                var classSchedule = new ClassScheduleTBL() { ClassScheduleID = Helper.GenerateRandomId(), ClassID = model.ClassID.Replace("/r/n", string.Empty), RoomID = model.RoomID, SlotID = model.SlotID1, SubjectID = model.SubjectID.Replace("/r/n", string.Empty), LecturerID = model.LecturerID, StudentListID = studentList.StudentListID, IsAttendance = false, ModeID = model.ModeID, Blog = model.BlogID, DateStudy = dateStudy };
                                ClassScheduleTBLRepository.Save(classSchedule);
                            }else{
                                var classSchedule = new ClassScheduleTBL() { ClassScheduleID = Helper.GenerateRandomId(), ClassID = model.ClassID.Replace("/r/n", string.Empty), RoomID = model.RoomID, SlotID = GetSlot(model.SlotID1) + (model.SlotID2.Equals("") ? "" : "-" + GetSlot(model.SlotID2)), SubjectID = model.SubjectID.Replace("/r/n", string.Empty), LecturerID = model.LecturerID, StudentListID = studentList.StudentListID, IsAttendance = false, ModeID = model.ModeID, Blog = model.BlogID, DateStudy = dateStudy };
                                ClassScheduleTBLRepository.Save(classSchedule);
                            }
                           
                        }
                    }
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
                SemesterTBL sem = GetCurrentSemester();
                using (var context = new IUContext())
                {
                    var schedule = (from t in (
                           from h in context.ClassScheduleTBLs
                           join u in context.StudentListTBLs on h.StudentListID equals u.StudentListID
                           where u.SemesterID == semesterID && u.ClassID == classID
                           select new {h,u}
                       )
                       group t by new { t.h.ClassID, t.h.LecturerID, t.h.SubjectID, t.h.RoomID, t.h.SlotID, t.h.ModeID, t.h.Blog, t.h.DateStudy } into g
                       select g);

                    var sh = schedule.ToArray().Select(s=> new ClassScheduleViewModel(){
                            ClassID = classID,
                            LecturerID = s.Key.LecturerID,
                            SubjectID = s.Key.SubjectID,
                            RoomID = s.Key.RoomID,
                            SlotID1 = GetSlot(0, s.Key.SlotID),
                            SlotID2 = GetSlot(1, s.Key.SlotID),
                            SlotID = s.Key.SlotID,
                            ModeID = s.Key.ModeID.Value,
                            BlogID = s.Key.Blog.Value,
                            SemesterID = semesterID,
                            DateStudy = s.Key.DateStudy,
                            isCreate = false
                    }).GroupBy(x => x.SubjectID).Select(x => x.FirstOrDefault());


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

        private string GetSlot(int index, string slotIDs)
        {
            if (slotIDs.IndexOf("-") >= 0)
            {
                return slotIDs.Split('-')[index];
            }
            else
            {
                if (index == 0) return slotIDs;
                else return "";
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
                if (id.Length == 3) return id;
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

        private string GetSlot(string slotID)
        {
            using (var context = new IUContext())
            {
                return context.SlotTBLs.Where(s => s.SlotID == slotID).FirstOrDefault().SlotName;
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
