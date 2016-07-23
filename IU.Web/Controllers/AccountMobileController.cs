using IU.Services;
using IU.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using IU.Domain;

namespace IU.Web.Controllers
{
  
    public class AccountMobileController : ApiController
    {
        private readonly UserService _userService = new UserService();

        // POST api/AccountMobile/Register
        /// <summary>
        /// Đăng ký bằng email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ResponseType(typeof(Register))]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Register(Register model)
        {
     
            var checkEmail =await _userService.checkEmailSync(model.UserName);
            if (checkEmail!=null)
            {
                return NotFound();
            }

            var signupUserModel = new AspNetUser
            {
                Id = Helper.GenerateRandomId(),
                Email = model.Email,
                EmailConfirmed = false,
                PasswordHash = Helper.GetHash(model.PasswordHash),
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                UserName = model.UserName
            };
          var insert =  _userService.InsertAsync(signupUserModel);
          return Ok(insert);
        }



        // GET api/TransCode/GetTransCode
        /// <summary>
        /// Get transportation forms
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<TransViewModel>))]
        [Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetTransCode()
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