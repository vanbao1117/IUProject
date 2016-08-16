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
    public class SubjectController : ApiController
    {
        // GET api/Subject/GetAllModes
        /// <summary>
        /// Get GetAllModes
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ModeViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllModes()
        {
            try
            {
                List<ModeViewModel> modelArray = null;
                using (SubjectService _subjectService = new SubjectService())
                {
                    modelArray = await _subjectService.GetAllModesSync();
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetAllSlots
        /// <summary>
        /// Get GetAllSlots
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<SlotViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllSlots()
        {
            try
            {
                List<SlotViewModel> modelArray = null;
                using (SubjectService _subjectService = new SubjectService())
                {
                    modelArray = await _subjectService.GetAllSlotsSync();
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetAllRooms
        /// <summary>
        /// Get GetAllRooms
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<RoomViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllRooms()
        {
            try
            {
                List<RoomViewModel> modelArray = null;
                using (SubjectService _subjectService = new SubjectService())
                {
                    modelArray = await _subjectService.GetAllRoomsSync();
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetAllLecturer
        /// <summary>
        /// Get GetAllLecturer
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<LecturerViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllLecturer()
        {
            try
            {
                List<LecturerViewModel> modelArray = null;
                using (SubjectService _subjectService = new SubjectService())
                {
                    modelArray = await _subjectService.GetAllLecturerSync();
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetAllClass
        /// <summary>
        /// Get GetAllClass
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ClassViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllClass()
        {
            try
            {
                List<ClassViewModel> modelArray = null;
                using (SubjectService _subjectService = new SubjectService())
                {
                    modelArray = await _subjectService.GetAllClassSync();
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetSubjects
        /// <summary>
        /// Get GetSubjects
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserSubjectViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetSubjects()
        {
            try
            {
                List<UserSubjectViewModel> modelArray = null;
                using (SubjectService _subjectService = new SubjectService())
                {
                    modelArray = await _subjectService.GetAllSubjectsSync();
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetAllSemester
        /// <summary>
        /// Get GetSubjects
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserSemesterViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetAllSemester()
        {
            try
            {
                List<UserSemesterViewModel> modelArray = null;
                using (SubjectService _subjectService = new SubjectService())
                {
                    modelArray = await _subjectService.GetAllSemesterSync();
                }
                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetSubjectByLecturer
        /// <summary>
        /// Get subject flow student
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserSubjectViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetSubjectByLecturer()
        {
            try
            {
                List<UserSubjectViewModel> modelArray = null;
                string userName = HttpContext.Current.User.Identity.Name;
                using (UserService _userService = new UserService())
                {
                    var currentUser = await _userService.FindUserSync(userName);
                    using (SubjectService _subjectService = new SubjectService())
                    {
                        modelArray = await _subjectService.GetSubjectByLecturerSync(currentUser.Id);
                    }
                }

                return Ok(modelArray);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + ex.StackTrace);
            }

        }

        // GET api/Subject/GetSubjectByStudent
        /// <summary>
        /// Get subject flow student
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<UserSubjectViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetSubjectByStudent()
        {
            try
            {
                List<UserSubjectViewModel> modelArray = null;
                string userName = HttpContext.Current.User.Identity.Name;
                using (UserService _userService = new UserService())
                {
                    var currentUser = await _userService.FindUserSync(userName);
                    using (SubjectService _subjectService = new SubjectService())
                    {
                        modelArray = await _subjectService.GetSubjectByStudentSync(currentUser.Id);
                    }
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