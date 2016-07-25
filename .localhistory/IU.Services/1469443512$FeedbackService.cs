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
    public class FeedbackService : IDisposable
    {
        private IRepository<FeedBackTBL> FeedbackRepository;
        public FeedbackService()
        {
            FeedbackRepository = new Repository<FeedBackTBL>();
        }
        public async Task<FeedbackViewModel> SubmitFeedbackSync(FeedbackViewModel model, string userId)
        {
            using (var context = new IUContext())
            {
                string _id = Helper.GenerateRandomId();
                await FeedbackRepository.SaveAsync(new FeedBackTBL() { Id = _id, });
                return model;
            }
        }


        #region Dispose
        ~FeedbackService()
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
