using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class Settlement
    {
        public int SettlementId { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public Student Student { get; set; }
        public Room Room { get; set; }
    }
}
