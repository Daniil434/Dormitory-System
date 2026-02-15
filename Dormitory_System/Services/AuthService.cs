using Dormitory_System.Entities;
using Dormitory_System.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;

        public AuthService()
        {
            // Внедрение зависимости (для простоты пока инициализируем)
            _userRepository = new UserRepository();
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            // 1. Получаем пользователя из БД (асинхронно)
            User dbUser = _userRepository.GetUserByUsername(username);

            if (dbUser == null)
            {
                return null; // Пользователь не найден
            }

            // 2. Проверяем хеш пароля
            bool isValid = BCrypt.Net.BCrypt.Verify(password, dbUser.PasswordHash);

            if (isValid && dbUser.IsActive)
            {
                // Пароль совпадает и учетная запись активна
                return dbUser;
            }
            else
            {
                // Пароль не совпадает или учетная запись заблокирована
                return null;
            }
        }
    }

    public static class AuthorizationService
    {
        public static bool HasRole(User user, string roleName)
        {
            return user?.RoleName == roleName;
        }

        public static bool CanAddUsers(User user)
        {
            return user?.RoleName == "Администратор";
        }

        public static bool CanEditUsers(User user)
        {
            return user?.RoleName == "Администратор";
        }

        public static bool CanDeleteUsers(User user)
        {
            return user?.RoleName == "Администратор";
        }

        public static bool CanBackupDatabase(User user)
        {
            return user?.RoleName == "Администратор";
        }

        public static bool CanRestoreDatabase(User user)
        {
            return user?.RoleName == "Администратор";
        }

        public static bool CanViewLog(User user)
        {
            return user?.RoleName == "Администратор";
        }

        public static bool CanExecuteQueries(User user)
        {
            return user?.RoleName == "Администратор";
        }
    }
}
