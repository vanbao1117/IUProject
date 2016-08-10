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
    public class ScheduleController : ApiController
    {
        // HttpPost api/Schedule/CreateStudent
        /// <summary>
        /// CreateStudent
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CreateStudent(StudentViewModel model)
        {
            try
            {
                using (SubjectService _SubjectService = new SubjectService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    await _SubjectService.CreateStudentSync(model, userName);
                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }


        // HttpPost api/Schedule/CreateLecturer
        /// <summary>
        /// CreateStudent
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CreateLecturer(LecturerViewModel model)
        {
            try
            {
                using (SubjectService _SubjectService = new SubjectService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    await _SubjectService.CreateLecturerSync(model, userName);
                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Schedule/CreateBis
        /// <summary>
        /// CreateBis
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CreateBis(BisViewModel model)
        {
            try
            {
                using (SubjectService _SubjectService = new SubjectService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    await _SubjectService.CreateBisSync(model, userName);
                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Schedule/UpdateClassSchedule
        /// <summary>
        /// UpdateClassSchedule
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> UpdateClassSchedule(ClassScheduleViewModel model)
        {
            try
            {
                using (SubjectService _SubjectService = new SubjectService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    if (model.isCreate)
                    {
                        await _SubjectService.CreateClassScheduleSync(model, userName);
                    }
                    else
                    {
                        await _SubjectService.UpdateClassScheduleSync(model, userName);
                    }
                    
                    return Ok();

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Schedule/GetClassSchedule
        /// <summary>
        /// Get GetClassSchedule
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(ClassScheduleViewModel))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetClassSchedule(string classID, string semesterID)
        {
            try
            {
                using (SubjectService _SubjectService = new SubjectService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var classs = await _SubjectService.GetClassScheduleSync(classID, semesterID);
                    return Ok(classs);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Schedule/getClass
        /// <summary>
        /// Get class Schedule
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(ClassViewModel))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> getClass()
        {
            try
            {
                using (ClassService _ClassService = new ClassService())
                {
                    string userName = HttpContext.Current.User.Identity.Name;
                    var classs = await _ClassService.GetLecuterClassList(userName);
                    return Ok(classs);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Schedule/GetAllClassScheduleSync
        /// <summary>
        /// Get class Schedule
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(ClassSchedulePageViewModel))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllClassScheduleSync(int pageNumber, int pageSize = 20, string abbreSubjectName = "ALL")
        {
            try
            {
                string userName = HttpContext.Current.User.Identity.Name;
                using (ScheduleClassService _ScheduleClassService = new ScheduleClassService())
                {
                    if (abbreSubjectName.Equals("ALL"))
                    {
                        var _ScheduleClass = await _ScheduleClassService.GetAllClassScheduleSync(pageNumber, pageSize, userName);
                        return Ok(_ScheduleClass);
                    }
                    else
                    {
                        var _ScheduleClass = await _ScheduleClassService.GetClassScheduleSync(pageNumber, pageSize, userName, abbreSubjectName);
                        return Ok(_ScheduleClass);
                    }
                    
                }
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