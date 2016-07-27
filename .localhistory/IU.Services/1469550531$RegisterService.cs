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
        private IRepository<StudentListTBL> ListStudentTBLRepository;

        public RegisterService()
        {
           
        }


        public async Task<List<UserSubjectViewModel>> GetRegisterDataSync(string userId)
        {
            using (var context = new IUContext())
            {
                return await Task.Run(() => GetRegisterData(userId));
            }
        }

        private List<OpenSubjectViewModel> GetRegisterData(string userId)
        {
            using (var context = new IUContext())
            {

                var openSubjects = from openSubject in context.OpenSubjectTBLs
                           join subjects in context.SubjectTBLs
                           on openSubject.SubjectID equals subjects.SubjectID
                                   select new OpenSubjectViewModel() { OpenSubjectID = openSubject.OpenSubjectID,SubjectID = openSubject.SubjectID,  OpenClassID = openSubject.OpenClassID,
                                     SubjectCode = subjects.AbbreSubjectName,
                                     Credit = openSubject.Credit, Cost = openSubject.Cost, CreatedDate = openSubject.CreatedDate,
                                     Creater = openSubject.Creater
                           };

                List<OpenSubjectViewModel> lsOpenSubject = new List<OpenSubjectViewModel>();
                foreach (OpenSubjectViewModel model in openSubjects.ToArray())
                {
                    var openClass = context.OpenClassTBLs.Where(o => o.OpenClassID == model.OpenClassID).ToArray()
                        .Select(o => new OpenClassViewModel() { }).ToList();

                    model.OpenClass = openClass;
                    lsOpenSubject.Add(model);
                }

                return subjects.Distinct().ToList();
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
