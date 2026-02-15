using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string ContactPhone { get; set; }
        public ObservableCollection<Application> AssignedApplications { get; set; }
    }
}
