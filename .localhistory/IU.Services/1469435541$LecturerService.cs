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
        public async Task<List<UserSubjectViewModel>> GetLecturersSync()
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetLecturers());
            }
        }

        private List<UserSubjectViewModel> GetLecturers()
        {
            using (var context = new IUContext())
            {

                var subjects =
                 from subjectTBLs in context.LecturerTBLs
                 join classScheduleTBLs in context.ClassScheduleTBLs
                     on subjectTBLs.SubjectID equals classScheduleTBLs.SubjectID
                 join studentListTBLs in context.StudentListTBLs
                     on classScheduleTBLs.StudentListID equals studentListTBLs.StudentListID
                 join studentTBLs in context.StudentTBLs
                    on studentListTBLs.StudentID equals studentTBLs.StudentID
                 where studentTBLs.UserID == userId
                 select new UserSubjectViewModel() { SubjectID = subjectTBLs.SubjectID, SubjectName = subjectTBLs.SubjectName, UserId = userId, AbbreSubjectName = subjectTBLs.AbbreSubjectName };

                return subjects.Distinct().ToList();
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
