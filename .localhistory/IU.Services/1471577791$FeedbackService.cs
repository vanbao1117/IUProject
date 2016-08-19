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
        public async Task<FeedbackViewModel> SubmitFeedbackSync(FeedbackViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                try { 
                     var user = context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                    var student = context.StudentTBLs.Where(s => s.UserID == user.Id).FirstOrDefault();
                    string _id = Helper.GenerateRandomId();
                    await FeedbackRepository.SaveAsync(new FeedBackTBL() { Id = _id, Attitude = model.Attitude, Comments = model.Comments, Lecturer = model.LecturerID, OnTime = model.OnTime, Quality = model.Quality, Satisfaction = model.Satisfaction, Student = student.StudentID, FeedbackDate = DateTime.Now } );
                    return model;
                }
                catch(Exception ex){}
                return null;  
            }
        }

        public async Task<List<FeedbackViewModel>> AdminViewFeedback(FeedbackViewModel model, string userName)
        {
            using (var context = new IUContext())
            {
                try
                {
                    var feedbacks = await FeedbackRepository.FindAllAsync();
                    if (feedbacks != null)
                    {
                        return feedbacks.ToList().Select(f => new FeedbackViewModel() { Attitude = f.Attitude, Comments = f.Comments, Lecturer = GetLectureName(f.Lecturer), OnTime = f.OnTime, Quality = f.Quality, Satisfaction = f.Satisfaction, Student = GetStudentName(f.Student), FeedbackDate = f.FeedbackDate.Value.ToString("dddd, dd MMMM yyyy") }).ToList();
                    } 
                }
                catch (Exception ex) { }
                return null;
            }
        }

        public string GetStudentName(string studentID)
        {
            using (var context = new IUContext())
            {
                return context.StudentTBLs.Where(s => s.StudentID == studentID).FirstOrDefault().StudentName;
            }
        }

        public string GetLectureName(string lecturerID)
        {
            using (var context = new IUContext())
            {
                return context.LecturerTBLs.Where(s => s.LecturerID == lecturerID).FirstOrDefault().LecturerName;
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
