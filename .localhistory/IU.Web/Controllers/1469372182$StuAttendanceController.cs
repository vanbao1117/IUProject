﻿using IU.Services;
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
                string userName = HttpContext.Current.User.Identity.Name;
                using (UserService _userService = new UserService())
                {
                    var currentUser = await _userService.FindUserSync(userName);
                    UserSemesterViewModel[] modelArray = null;
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