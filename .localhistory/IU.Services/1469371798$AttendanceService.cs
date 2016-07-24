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
    public class AttendanceService : IDisposable
    {
       
        public AttendanceService()
        {
            
        }


        public async Task<List<UserSubjectViewModel>> GetSemesterByStudentSync(string userId)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetSemesterByStudent(userId));
            }
        }

        private List<UserSubjectViewModel> GetSemesterByStudent(string userId)
        {
            using (var context = new IUContext())
            {

                var semesters =
                 from semesterTBLs in context.SemesterTBLs
                 join studentListTBLs in context.StudentListTBLs
                     on semesterTBLs.SemesterID equals studentListTBLs.SemesterID
                 join studentTBLs in context.StudentTBLs
                    on studentListTBLs.StudentID equals studentTBLs.StudentID
                 where studentTBLs.UserID == userId
                 select new UserSemesterViewModel() { SemesterID = semesterTBLs.SemesterID, SemesterName = semesterTBLs.SemesterName, StartDate = semesterTBLs.StartDate, EndDate = semesterTBLs.EndDate };

                return semesters.ToList();
            }
        }

        #region Dispose
        ~AttendanceService()
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
