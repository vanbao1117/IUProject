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
    public class StudentClassService : IDisposable
    {
        private IRepository<StudentListTBL> ListStudentTBLRepository;

        public StudentClassService()
        {
            //ListStudentTBLRepository = new Repository<StudentListTBL>();
            //var studentlist = await ListStudentTBLRepository.FindAllByAsync(ls => ls.StudentID == "");


            //using (var stx = new IUContext()){
            //    var studentClass = (from c in stx.AttendanceTBLs
            //                        where c.StudentListID == "SE03131"
            //                        select new { c.ClassID, c.SubjectID, c.Attendancer, c.SlotID, c.DateAttendance });
            //}
        }

       


        #region Dispose
        ~StudentClassService()
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
                ListStudentTBLRepository.Dispose();
                ListStudentTBLRepository = null;
            }
            // dispose the unmanaged objects
        }

        #endregion
    }
}
