using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class Violation
    {
        public int ViolationId { get; set; }
        public int StudentId { get; set; }
        public string ViolationType { get; set; }
        public DateTime ViolationDate { get; set; }
        public string ActionTaken { get; set; }
        public int? RecordedByStaffId { get; set; }
        public Student Student { get; set; }
        public Staff RecordedStaff { get; set; }
    }
}
