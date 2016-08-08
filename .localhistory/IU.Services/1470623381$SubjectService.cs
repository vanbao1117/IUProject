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
        public SubjectService()
        {
            LecturerTBLRepository = new Repository<LecturerTBL>();
            ClassRepository = new Repository<ClassTBL>();
            OpenClassRepository = new Repository<OpenClassTBL>();
        }

        public async Task<bool> CreateBisSync(BisViewModel model)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => CreateBis(model));
            }
        }

        private bool CreateBis(BisViewModel model)
        {
            foreach(string slotID in model.SlotIDs)
            string classID = Helper.GenerateRandomId();
            string openClassID = Helper.GenerateRandomId();
            ClassRepository.Save(new ClassTBL() { ClassID = classID, ClassName = model.ClassName });
            OpenClassRepository.Save(new OpenClassTBL() { OpenClassID = openClassID, ClassID = classID, CreatedDate = DateTime.Now, Creater = "", Deadline = model.Deadline, Limit = model.Limit, RoomID = model.RoomID, SemesterID = model.SemesterID, SlotID = model.SlotID1 });
            using (var context = new IUContext())
            {
                
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
