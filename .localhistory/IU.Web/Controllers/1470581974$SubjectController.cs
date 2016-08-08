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