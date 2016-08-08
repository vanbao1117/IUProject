using IU.Services;
using IU.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace IU.Web.Controllers
{
    [Authorize]
    public class LecturerController : ApiController
    {
        // GET api/Lecturer/GetLecturerPreview
        /// <summary>
        /// Get GetLecturerPreview
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PreviewListViewModel))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetLecturerPreview(string classID, string subjectID)
        {
            try
            {
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var _Lecturers = await _LecturerService.GetLecturerPreviewSync(userName, classID, subjectID);
                    return Ok(_Lecturers);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Lecturer/GetLecturerPreview
        /// <summary>
        /// Get GetLecturerPreview
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(PreviewListViewModel))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetLecturerClassSubject(string classID, string subjectID)
        {
            try
            {
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var _Lecturers = await _LecturerService.GetLecturerPreviewSync(userName, classID, subjectID);
                    return Ok(_Lecturers);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }


        // GET api/Lecturer/GetLecturers
        /// <summary>
        /// Get class Lecturer
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserAttendancePagingViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetLecturers()
        {
            try
            {
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var _Lecturers = await _LecturerService.GetLecturersSync(userName);
                    return Ok(_Lecturers);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }


        // GET api/Lecturer/GetAttendanceTwoDaysBefore
        /// <summary>
        /// Get attendances
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserAttendanceViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAttendanceTwoDaysBefore()
        {
            try
            {
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    
                    var attendances = await _LecturerService.GetAttendancesSync(userName, 0);
                    return Ok(attendances);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }


        // GET api/Lecturer/GetAttendanceToDay
        /// <summary>
        /// Get attendances
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserAttendanceViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAttendanceToDay()
        {
            try
            {
                
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var attendances = await _LecturerService.GetAttendancesSync(userName, 1);
                    return Ok(attendances);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Lecturer/GetAttendancesNext
        /// <summary>
        /// Get attendances
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserAttendanceViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAttendancesNext()
        {
            try
            {
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var attendances = await _LecturerService.GetAttendancesSync(userName, 2);
                    return Ok(attendances);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Lecturer/GetTakeAttendances
        /// <summary>
        /// GetTakeAttendances
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserAttendanceViewModel>))]
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetTakeAttendances(UserAttendanceViewModel model)
        {
            try
            {
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var dateAttendance = model.DateStudy;
                    if (model.isAttendanced) dateAttendance = model.DateAttendance;
                    var attendances = await _LecturerService.GetTakeAttendancesSync(model.SubjectID, model.SemesterID, model.ClassID, dateAttendance, model.isAttendanced, model.SlotID);
                    return Ok(attendances);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // Post api/Lecturer/TakeAttendances
        /// <summary>
        /// GetTakeAttendances
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserAttendanceViewModel>))]
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> TakeAttendances(List<UserAttendanceViewModel> models)
        {
            try
            {
                using (LecturerService _LecturerService = new LecturerService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;

                    bool x = await _LecturerService.TakeAttendancesSync(models, userName);

                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

    }
}