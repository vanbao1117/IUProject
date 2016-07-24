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
        private IRepository<StudentListTBL> ListStudentTBLRepository;

        public SubjectService()
        {
            //ListStudentTBLRepository = new Repository<StudentListTBL>();
            //var studentlist = await ListStudentTBLRepository.FindAllByAsync(ls => ls.StudentID == "");


            //using (var stx = new IUContext()){
            //    var studentClass = (from c in stx.AttendanceTBLs
            //                        where c.StudentListID == "SE03131"
            //                        select new { c.ClassID, c.SubjectID, c.Attendancer, c.SlotID, c.DateAttendance });
            //}
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
