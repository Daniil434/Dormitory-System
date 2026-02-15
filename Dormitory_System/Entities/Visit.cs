using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class Visit
    {
        public int VisitId { get; set; }
        public string GuestName { get; set; }
        public int HostStudentId { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public Student HostStudent { get; set; }
    }
}
