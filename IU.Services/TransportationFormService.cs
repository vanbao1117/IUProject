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
    public class TransportationFormService : IDisposable
    {
        private IRepository<AspTransport> TransportationFormRepository;
        private IRepository<TestTable> TestTableRepository;
        public TransportationFormService()
        {
            TransportationFormRepository = new Repository<AspTransport>();
            TestTableRepository = new Repository<TestTable>();
        }

        public async Task<string> Delete(string id)
        {
            try
            {
               
                var transPort = await TransportationFormRepository.FindOneByAsync(t=>t.Id == id);
                if (transPort != null)
                {
                    await TransportationFormRepository.DeleteAsync(transPort);
                    return transPort.Id;
                }
                    
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<TransViewModel> Create(TransViewModel model)
        {
            try
            {
                string _id = Helper.GenerateRandomId();
                await TransportationFormRepository.SaveAsync(new AspTransport() { Id = _id, TransCode=model.TransCode, TransName = model.TransName });
                model.Id = _id;
                return model;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<TransViewModel[]> GetList()
        {
            try
            {
                var test = TestTableRepository.FindAll().ToArray();
                var trans = await TransportationFormRepository.FindAllAsync();
                if (trans != null)
                {
                    return trans.ToArray().Select(t => new TransViewModel() { Id = t.Id, TransCode = t.TransCode, TransName = t.TransName }).ToArray();
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        #region Dispose
        ~TransportationFormService()
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
                TransportationFormRepository.Dispose();
                TransportationFormRepository = null;
            }
            // dispose the unmanaged objects
        }

        #endregion
    }
}
