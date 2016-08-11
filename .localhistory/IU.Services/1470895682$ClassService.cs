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
        public ClassService()
        {
            ClassTBLRepository = new Repository<ClassTBL>();
        }

        public async Task<List<ClassViewModel>> GetStudentInClass(string classID)
        {
            try
            {
                using (var context = new IUContext())
                {
                    var classs =
                     (from studentTBLs in context.StudentTBLs
                      join studentListTBLs in context.StudentListTBLs
                          on studentTBLs.StudentID equals studentListTBLs.StudentID
                      where studentListTBLs.ClassID == classID
                      select new StudentViewModel() { StudentID = studentTBLs.StudentID, ClassID = studentListTBLs.ClassID, StudentPhone = studentTBLs.StudentPhone, StudentName = studentTBLs.StudentName, StudentGender = studentTBLs.StudentGender, StudentEmail = studentTBLs.StudentEmail, StudentBirth = studentTBLs.StudentBirth, ParentPhone = studentTBLs.ParentPhone});
                }
               
                return classs.ToArray();
            }
            catch (Exception ex)
            {
            }
            return new List<ClassViewModel>();
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
