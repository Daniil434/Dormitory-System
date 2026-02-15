using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int RoomId { get; set; }
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }
        public int? AssignedToStaffId { get; set; }
        public Room Room { get; set; }
        public Staff AssignedStaff { get; set; }
    }
}
