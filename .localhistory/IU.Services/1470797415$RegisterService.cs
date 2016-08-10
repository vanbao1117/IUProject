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
    public class RegisterService : IDisposable
    {
        private IRepository<AcceptRegister> AcceptRegisterRepository;
        private IRepository<ClassScheduleTBL> ClassScheduleRepository;
        private IRepository<StudentListTBL> StudentListRepository;

        public RegisterService()
        {
            AcceptRegisterRepository = new Repository<AcceptRegister>();
            ClassScheduleRepository = new Repository<ClassScheduleTBL>();
            StudentListRepository = new Repository<StudentListTBL>();
        }

        public async Task<OpenSubjectViewModel> UndoRegisterSync(OpenSubjectViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                try
                {
                    var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                    var student = context.StudentTBLs.Where(s => s.UserID == user.Id).FirstOrDefault();

                    var acceptRegister = AcceptRegisterRepository.FindAllBy(a => a.StudentID == student.StudentID && a.OpenSubjectID == model.OpenSubjectID).FirstOrDefault();

                    if (acceptRegister != null)
                    {
                        await AcceptRegisterRepository.DeleteAsync(acceptRegister);
                    }

                    var nextAcceptRegister = AcceptRegisterRepository.FindAllBy(a => a.OrderNum == acceptRegister.OrderNum + 1).FirstOrDefault();
                    //Accept next student
                    if (nextAcceptRegister != null)
                        createClassSchedule(nextAcceptRegister);

                    return model;
                }
                catch (Exception ex) { }
                return null;
            }
        }

        public async Task<OpenSubjectViewModel> AcceptRegisterSync(OpenSubjectViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                try
                {
                    var acceptRegister = AcceptRegisterRepository.FindAll().OrderByDescending(o => o.OrderNum).Take(1).FirstOrDefault();
                    int orderNum = 1;
                    if (acceptRegister != null)
                    {
                        orderNum = acceptRegister.OrderNum.HasValue ? acceptRegister.OrderNum.Value + 1 : 1;
                    }

                    var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                    var student = context.StudentTBLs.Where(s => s.UserID == user.Id).FirstOrDefault();

                    var check = context.AcceptRegisters.Where(a => a.OpenSubjectID == model.OpenSubjectID && a.StudentID == student.StudentID).FirstOrDefault();
                    if (check != null)
                    {
                        return null;
                    }

                    //Check limited
                    var limited = checkLimit(model.OpenClassID);
                    if (limited) return null;

                    string _id = Helper.GenerateRandomId();
                    await AcceptRegisterRepository.SaveAsync(new AcceptRegister() { AcceptRegisterID = _id, OpenClassID = model.OpenClassID, OpenSubjectID = model.OpenSubjectID, OrderNum = orderNum, RegisterDate = DateTime.Now, StudentID = student.StudentID, Accepted = false });

                    var accept = AcceptRegisterRepository.FindOneBy(a => a.AcceptRegisterID == _id);

                    createClassSchedule(accept);

                    return model;
                }
                catch (Exception ex) { }
                return null;
            }
        }


        private void createClassSchedule(AcceptRegister accept)
        {
            using (var context = new IUContext())
            {
                var openClass = context.OpenClassTBLs.Where(o => o.OpenClassID == accept.OpenClassID).FirstOrDefault();
                var openSubject = context.OpenSubjectTBLs.Where(o => o.OpenSubjectID == accept.OpenSubjectID).FirstOrDefault();
                var classID = openClass.ClassID;
                var RoomId = openClass.RoomID;
                var slotIDs = openClass.SlotID;
                var lecturerID = openSubject.LecturerID;
                var semesterID = openClass.SemesterID;
                var studentID = accept.StudentID;
                var studentListId = Helper.GenerateRandomId();

                StudentListRepository.Save(new StudentListTBL() { ClassID = classID, SemesterID = semesterID, StudentID = studentID, StudentListID = studentListId });

                foreach (string slotID in slotIDs.Split('-'))
                {
                    ClassScheduleRepository.Save(new ClassScheduleTBL() { ClassScheduleID = Helper.GenerateRandomId(), ClassID = classID, DateStudy = openSubject.StartDate.Value, LecturerID = lecturerID, ModeID = openSubject.ModeID, RoomID = RoomId, SlotID = slotID, StudentListID = studentListId, SubjectID = openSubject.SubjectID });
                }
            }
        }

        public async Task<List<OpenSubjectViewModel>> GetRegisterDataSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetRegisterData());
            }
        }

        private List<OpenSubjectViewModel> GetRegisterData()
        {
            using (var context = new IUContext())
            {

                var openSubjects = from openSubject in context.OpenSubjectTBLs
                           join subjects in context.SubjectTBLs
                           on openSubject.SubjectID equals subjects.SubjectID
                                   select new OpenSubjectViewModel() { OpenSubjectID = openSubject.OpenSubjectID,SubjectID = openSubject.SubjectID,  OpenClassID = openSubject.OpenClassID,
                                     SubjectCode = subjects.AbbreSubjectName,
                                     SubjectName = subjects.SubjectName,
                                     Select = false,
                                     Active = false,
                                     Credit = openSubject.Credit, Cost = openSubject.Cost, CreatedDate = openSubject.CreatedDate,
                                     Creater = openSubject.Creater,Accepted = false
                           };

                

                List<OpenSubjectViewModel> lsOpenSubject = new List<OpenSubjectViewModel>();
                int index = 0;
                foreach (OpenSubjectViewModel model in openSubjects.ToArray())
                {
                    model.Select = checkExitOpenSubject(model.OpenSubjectID) && index == 0 ? true : false;
                    model.Active = checkExitOpenSubject(model.OpenSubjectID) && index == 0 ? true : false;
                    model.Accepted = GetAcceptRegister(model.OpenClassID) == null ? false : GetAcceptRegister(model.OpenClassID).Accepted.Value;
                    model.Status = GetAcceptRegister(model.OpenClassID) == null ? "" : GetAcceptRegister(model.OpenClassID).Accepted.Value ? "Accepted" : "Pending";

                    var openClass = context.OpenClassTBLs.Where(o => o.OpenClassID == model.OpenClassID).ToArray()
                        .Select(o => new OpenClassViewModel() { Active = checkExitOpenClass(o.OpenClassID) && index == 0 ? true : false, ClassID = o.ClassID, ClassName = GetClass(o.ClassID).ClassName, CreatedDate = o.CreatedDate, Creater = o.Creater, OpenClassID = o.OpenClassID, RoomID = o.RoomID, RoomName = o.RoomID, Select = checkExitOpenClass(o.OpenClassID) && index == 0 ? true : false, SlotID = o.SlotID, SlotName = o.SlotID, StartDate = GetClass(o.ClassID).StartDate, Accepted = GetAcceptRegister(model.OpenClassID) == null ? false : GetAcceptRegister(model.OpenClassID).Accepted.Value, Status = GetAcceptRegister(model.OpenClassID) == null ? "" : GetAcceptRegister(model.OpenClassID).Accepted.Value ? "Accepted" : "Pending" }).ToList();

                    model.ChooseClass = openClass;
                    lsOpenSubject.Add(model);
                    index++;
                }

                return lsOpenSubject;
            }
        }

        private bool checkExitOpenSubject(string oPenSubjectID)
        {
            using (var context = new IUContext())
            {
                var openSubject = context.AcceptRegisters.Where(o => o.OpenSubjectID == oPenSubjectID).FirstOrDefault();
                if (openSubject != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool checkExitOpenClass(string openClassID)
        {
            using (var context = new IUContext())
            {
                var openSubject = context.AcceptRegisters.Where(o => o.OpenClassID == openClassID).FirstOrDefault();
                if (openSubject != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool checkLimit(string openClassID)
        {
            using (var context = new IUContext())
            {
                var currentCount = context.AcceptRegisters.Where(a => a.OpenClassID == openClassID).ToArray().Count();
                var _class = context.OpenClassTBLs.Where(o => o.OpenClassID == openClassID).FirstOrDefault();
                if (_class != null && _class.Limit == currentCount)
                {
                    return true;
                }
                else
                {
                    var currentDate = DateTime.Now;
                    _class = context.OpenClassTBLs.Where(o => o.OpenClassID == openClassID && o.Deadline.Value < currentDate).FirstOrDefault();
                    if (_class != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }



        private AcceptRegister GetAcceptRegister(string openClassID)
        {
            using (var context = new IUContext())
            {
                var acceptRegister = context.AcceptRegisters.Where(o => o.OpenClassID == openClassID).FirstOrDefault();
                return acceptRegister;
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
        private string GetSlotbyID(string SlotID)
        {
            using (var context = new IUContext())
            {
                return context.SlotTBLs.Where(s => s.SlotID == SlotID).FirstOrDefault().SlotTime;
            }
        }

        #region Dispose
        ~RegisterService()
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
