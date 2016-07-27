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

        public RegisterService()
        {
            AcceptRegisterRepository = new Repository<AcceptRegister>();
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
                    string _id = Helper.GenerateRandomId();
                    await AcceptRegisterRepository.SaveAsync(new AcceptRegister() { AcceptRegisterID = _id, OpenClassID = model.OpenClassID, OpenSubjectID = model.OpenSubjectID, OrderNum = orderNum, RegisterDate = DateTime.Now, StudentID = student.StudentID });
                    return model;
                }
                catch (Exception ex) { }
                return null;
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

                var openSubjects = (from openSubject, subjects in context.OpenSubjectTBLs
                           join subjects in context.SubjectTBLs
                           on openSubject.SubjectID equals subjects.SubjectID
                                    select openSubject).Select(openSubject => new OpenSubjectViewModel()
                                    {
                                        OpenSubjectID = openSubject.OpenSubjectID,
                                        SubjectID = openSubject.SubjectID,
                                        OpenClassID = openSubject.OpenClassID,
                                     SubjectCode = subjects.AbbreSubjectName,
                                     SubjectName = subjects.SubjectName,
                                     Select = checkExitOpenSubject(openSubject.OpenSubjectID),
                                     Active = checkExitOpenSubject(openSubject.OpenSubjectID),
                                     Credit = openSubject.Credit, Cost = openSubject.Cost, CreatedDate = openSubject.CreatedDate,
                                     Creater = openSubject.Creater, Accepted = GetAcceptRegister(openSubject.OpenClassID).Accepted.Value
                           });

                List<OpenSubjectViewModel> lsOpenSubject = new List<OpenSubjectViewModel>();
                foreach (OpenSubjectViewModel model in openSubjects.ToArray())
                {
                    var openClass = context.OpenClassTBLs.Where(o => o.OpenClassID == model.OpenClassID).ToArray()
                        .Select(o => new OpenClassViewModel() { Active = checkExitOpenClass(o.OpenClassID), ClassID = o.ClassID, ClassName = GetClass(o.ClassID).ClassName, CreatedDate = o.CreatedDate, Creater = o.Creater, OpenClassID = o.OpenClassID, RoomID = o.RoomID, RoomName = o.RoomID, Select = checkExitOpenClass(o.OpenClassID), SlotID = o.SlotID, SlotName = GetSlotbyID(o.SlotID), StartDate = GetClass(o.ClassID).StartDate, Accepted = GetAcceptRegister(o.OpenClassID).Accepted.Value }).ToList();

                    model.ChooseClass = openClass;
                    lsOpenSubject.Add(model);
                }

                return lsOpenSubject;
            }
        }

        private bool checkExitOpenSubject(string oPenSubjectID)
        {
            using (var context = new IUContext())
            {
                var openSubject = context.OpenSubjectTBLs.Where(o=>o.OpenSubjectID == oPenSubjectID);
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
                var openSubject = context.AcceptRegisters.Where(o => o.OpenClassID == openClassID);
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
