using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Web.Models
{
    public class RefreshTokenViewModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string ClientId { get; set; }
        public System.DateTime IssuedUtc { get; set; }
        public System.DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
