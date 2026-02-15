using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Dormitory_System.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int BlockId { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public Block Block { get; set; }
    }
}
