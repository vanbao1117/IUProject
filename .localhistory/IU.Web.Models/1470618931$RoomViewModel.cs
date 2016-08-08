using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Web.Models
{
    public class RoomViewModel
    {
        public string RoomID { get; set; }
        public string RomName { get; set; }
        public string Decription { get; set; }
    }

    public class SlotViewModel
    {
        public string SlotID { get; set; }
        public string SlotTime { get; set; }
        public string SlotName { get; set; }
        public int NumOfSlot { get; set; }
        public int TotalSlot { get; set; }
    }

    public class ModeViewModel
    {
        public string ModeID { get; set; }
        public string Mode { get; set; }
        public string Decription { get; set; }
    }
}
