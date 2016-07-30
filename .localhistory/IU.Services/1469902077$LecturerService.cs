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

                return lecturers.Distinct().ToList();
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
