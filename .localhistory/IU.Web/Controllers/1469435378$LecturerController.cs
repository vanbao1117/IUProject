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

        // GET api/Lecturer/GetLecturers
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

        

    }
}