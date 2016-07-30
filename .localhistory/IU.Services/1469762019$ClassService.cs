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




        public async Task<ClassViewModel[]> GetList(string userName)
        {
            try
            {
                var test = ClassTBLRepository.FindAll().ToArray();
                var trans = await ClassTBLRepository.FindAllAsync();
                if (trans != null)
                {
                    return trans.ToArray().Select(t => new ClassViewModel() { Id = t.Id, ClassName = t.ClassName }).ToArray();
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
