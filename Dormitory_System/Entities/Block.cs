using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class Block
    {
        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }
    }
}
