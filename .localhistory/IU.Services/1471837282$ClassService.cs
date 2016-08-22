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
    public class ClassService : IDisposable
    {
        private IRepository<ClassTBL> ClassTBLRepository;
        private IRepository<StudentListTBL> StudentListTBLRepository;
        private IRepository<OpenClassTBL> OpenClassTBLRepository;
        public ClassService()
        {
            ClassTBLRepository = new Repository<ClassTBL>();
            StudentListTBLRepository = new Repository<StudentListTBL>();
            OpenClassTBLRepository = new Repository<OpenClassTBL>();
        }

        public async Task<List<OpenClassViewModel>> GetOpenClass()
        {
            try
            {
                var sem = GetCurrentSemester();
                using (var context = new IUContext())
                {
                    var openclass =
                     (from openClassTBLs in context.OpenClassTBLs
                      join classTBLs in context.ClassTBLs
                        on openClassTBLs.ClassID equals classTBLs.ClassID
                      where openClassTBLs.SemesterID == sem.SemesterID
                      select new OpenClassViewModel() { OpenClassID = openClassTBLs.OpenClassID, ClassID = classTBLs.ClassID, ClassName = classTBLs.ClassName });
                    return openclass.ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return new List<OpenClassViewModel>();
        }

        public async Task<List<StudentInOpenClassViewModel>> GetStudentInOpenClass(string classID)
        {
            try
            {
                using (var context = new IUContext())
                {
                    var students =
                     (from studentTBLs in context.StudentTBLs
                      join acceptRegisters in context.AcceptRegisters
                          on studentTBLs.StudentID equals acceptRegisters.StudentID
                      join openClassTBLs in context.OpenClassTBLs
                        on acceptRegisters.OpenClassID equals openClassTBLs.OpenClassID
                      join classTBLs in context.ClassTBLs
                        on openClassTBLs.ClassID equals classTBLs.ClassID
                      where openClassTBLs.ClassID == classID 
                      select new StudentInOpenClassViewModel() { StudentID = studentTBLs.StudentID, ClassID = openClassTBLs.ClassID, ClassName = classTBLs.ClassName, StudentName = studentTBLs.StudentName, Status = acceptRegisters.Accepted == true? "Accepted" : "Pending", StudentBirth = studentTBLs.StudentBirth.Value, StudentCode = studentTBLs.StudentCode });
                    return students.ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return new List<StudentInOpenClassViewModel>();
        }

        public async Task<bool> ChangeClass(string oldClassID, string studentID, string newClassID)
        {
            try
            {
                SemesterTBL sem = GetCurrentSemester();
                var student = StudentListTBLRepository.FindOneBy(s => s.ClassID == oldClassID && s.StudentID == studentID && s.SemesterID == sem.SemesterID);
                if (student != null)
                {
                    StudentListTBLRepository.Delete(student);
                }

                StudentListTBLRepository.Save(new StudentListTBL() { StudentListID = Helper.GenerateRandomId(), ClassID = newClassID, SemesterID = sem.SemesterID, StudentID = studentID });
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
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

        public async Task<List<StudentViewModel>> GetStudentInClass(string classID)
        {
            try
            {
                using (var context = new IUContext())
                {
                    SemesterTBL sem = GetCurrentSemester();
                    var students =
                     (from studentTBLs in context.StudentTBLs
                      join studentListTBLs in context.StudentListTBLs
                          on studentTBLs.StudentID equals studentListTBLs.StudentID
                      where studentListTBLs.ClassID == classID && studentListTBLs.SemesterID == sem.SemesterID
                      select new StudentViewModel() { StudentID = studentTBLs.StudentID, ClassID = studentListTBLs.ClassID, StudentPhone = studentTBLs.StudentPhone, StudentName = studentTBLs.StudentName, StudentGender = studentTBLs.StudentGender.Value, StudentEmail = studentTBLs.StudentEmail, StudentBirth = studentTBLs.StudentBirth.Value, ParentPhone = studentTBLs.ParentPhone});
                    return students.ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return new List<StudentViewModel>();
        }

        public async Task<List<ClassViewModel>> GetAllClassList()
        {
            try
            {
                return ClassTBLRepository.FindAll().Select(c => new ClassViewModel() { ClassID = c.ClassID, ClassName = c.ClassName, IsMain = c.IsMainClass.Value }).ToList();
            }
            catch (Exception ex)
            {
            }
            return new List<ClassViewModel>();
        }

        public async Task<ClassViewModel[]> GetLecuterClassList(string userName)
        {
            try
            {
                using (var context = new IUContext())
                {
                    var usr = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                    var lecture = context.LecturerTBLs.Where(l => l.UserID == usr.Id).FirstOrDefault();
                    var classs =
                       (from tblClass in context.ClassTBLs
                        join tblLecuterClass in context.LecturerScheduleTBLs
                            on tblClass.ClassID equals tblLecuterClass.ClassID
                        where tblLecuterClass.LecturerID == lecture.LecturerID
                        select new ClassViewModel() { ClassID = tblClass.ClassID, ClassName = tblClass.ClassName });
                    return classs.ToArray();
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        #region Dispose
        ~ClassService()
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
                // dispose the managed objects
                ClassTBLRepository.Dispose();
                ClassTBLRepository = null;
            }
            // dispose the unmanaged objects
        }

        #endregion
    }
}
