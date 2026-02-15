using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Хранит хеш
        public int RoleId { get; set; }
        public string RoleName { get; set; } // Для удобства при получении
        public bool IsActive { get; set; }
    }
}
