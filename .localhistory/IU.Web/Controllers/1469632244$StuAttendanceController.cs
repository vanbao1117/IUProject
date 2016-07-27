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
    public class StuAttendanceController : ApiController
    {
        // GET api/StuAttendance/GetRegisterData
        /// <summary>
        /// Get class Register
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetRegisterData()
        {
            try
            {
                using (RegisterService _RegisterService = new RegisterService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    List<OpenSubjectViewModel> lsModel = await _RegisterService.GetRegisterDataSync();
                    return Ok(lsModel);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // POST api/StuAttendance/AcceptRegister
        /// <summary>
        /// submit class accept
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> AcceptRegister(OpenSubjectViewModel model)
        {
            try
            {
                using (RegisterService _RegisterService = new RegisterService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    OpenSubjectViewModel _model = await _RegisterService.AcceptRegisterSync(model, userName);
                    if (_model == null) return Ok(new OpenSubjectViewModel() { Error = true });
                    return Ok(_model);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // POST api/StuAttendance/UndoRegister
        /// <summary>
        /// submit class accept
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> UndoRegister(OpenSubjectViewModel model)
        {
            try
            {
                using (RegisterService _RegisterService = new RegisterService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    OpenSubjectViewModel _model = await _RegisterService.UndoRegisterSync(model, userName);
                    if (_model == null) return Ok(new OpenSubjectViewModel() { Error = true });
                    return Ok(_model);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }


        // GET api/StuAttendance/FeedbackByStudent
        /// <summary>
        /// Get class Attendance
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> FeedbackByStudent(FeedbackViewModel model)
        {
            try
            {
                using (FeedbackService _FeedbackService = new FeedbackService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    await _FeedbackService.SubmitFeedbackSync(model, userName);
                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/StuAttendance/GetAttendanceByStudent
        /// <summary>
        /// Get class Attendance
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserAttendancePagingViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAttendanceByStudent(int pageNumber, int pageSize = 20, string semesterCode = "", string subjectCode = "")
        {
            try
            {
                using (AttendanceService _AttendanceService = new AttendanceService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var _Attendances = await _AttendanceService.GetAttendanceByStudentSync(pageNumber, pageSize, userName, semesterCode, subjectCode);
                    return Ok(_Attendances);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/StuAttendance/GetSemesterByStudent
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserSemesterViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetSemesterByStudent()
        {
            try
            {
                UserSemesterViewModel[] modelArray = null;
                string userName = HttpContext.Current.User.Identity.Name;
                using (UserService _userService = new UserService())
                {
                    var currentUser = await _userService.FindUserSync(userName);
                    
                    using (AttendanceService myservice = new AttendanceService())
                    {
                        modelArray = await myservice.GetSemesterByStudentSync(currentUser.Id);
                    }
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/TransCode/GetTransCode
        /// <summary>
        /// Get transportation forms
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ClassViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetClassByLogonUser()
        {
            try
            {
                TransViewModel[] modelArray = null;
                using (TransportationFormService myservice = new TransportationFormService())
                {
                   modelArray = await myservice.GetList();
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // Post api/TransCode/CreateTransCode
        /// <summary>
        /// create transportation forms
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TransViewModel>))]
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CreateTransCode(TransViewModel model)
        {
            try
            {
                using (TransportationFormService myservice = new TransportationFormService())
                {
                    TransViewModel _newModel = await myservice.Create(model);
                    return Ok(_newModel);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // Post api/TransCode/DeleteTransCode/<id>
        /// <summary>
        /// create transportation forms
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TransViewModel>))]
        [Authorize]
        [System.Web.Http.HttpDelete]
        public async Task<IHttpActionResult> DeleteTransCode(string id)
        {
            try
            {
                using (TransportationFormService myservice = new TransportationFormService())
                {
                    await myservice.Delete(id);
                }

                return Ok(id);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

    }
}