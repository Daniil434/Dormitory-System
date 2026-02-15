using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string PassportData { get; set; }
        public string DepartmentName { get; set; }
        public int Course { get; set; }
        public string ContactPhone { get; set; }
    }
}
