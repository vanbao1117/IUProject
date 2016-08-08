using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class TransViewModel
    {
        //{id:'', transCode:'', }
        public string Id { get; set; }
        public string TransCode { get; set; }
        public string TransName { get; set; }
    }
}