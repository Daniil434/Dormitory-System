using Dormitory_System.DAL;
using Dormitory_System.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Services
{
    public static class DatabaseInitializer
    {
        private static readonly UserRepository userRepository = new UserRepository();

        public static void Initialize()
        {
            try
            {
                // 1. Убедимся, что роли существуют
                EnsureRolesExist();

                // 2. Создаем тестовых пользователей
                CreateTestUsers();

                Console.WriteLine("Инициализация базы данных завершена успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
                throw;
            }
        }

        private static void EnsureRolesExist()
        {
            // Используем INSERT IGNORE для избежания дублирования
            string insertRolesSql = @"
                INSERT IGNORE INTO Roles (role_name) VALUES 
                ('Администратор'), 
                ('Комендант'), 
                ('Кастелянша'), 
                ('Студент')";

            DbConnectionManager.ExecuteNonQuery(insertRolesSql);
        }

        private static Dictionary<string, int> GetRoleIds()
        {
            var roleIds = new Dictionary<string, int>();
            string query = "SELECT role_id, role_name FROM Roles";
            DataTable rolesTable = DbConnectionManager.ExecuteSelectQuery(query);

            foreach (DataRow row in rolesTable.Rows)
            {
                string roleName = row["role_name"].ToString();
                int roleId = Convert.ToInt32(row["role_id"]);
                roleIds[roleName] = roleId;
            }

            return roleIds;
        }

        private static void CreateTestUsers()
        {
            // Проверяем, есть ли уже пользователи
            string checkUsersSql = "SELECT COUNT(*) FROM Users";
            int usersCount = Convert.ToInt32(DbConnectionManager.ExecuteScalar(checkUsersSql));

            if (usersCount > 0)
            {
                Console.WriteLine($"Пользователи уже существуют ({usersCount} записей).");
                return;
            }

            Console.WriteLine("Добавляем тестовых пользователей...");

            // Получаем ID ролей
            var roleIds = GetRoleIds();

            // Тестовые пользователи
            var testUsers = new[]
            {
                new { Username = "admin", Password = "Admin123!", RoleName = "Администратор" },
                new { Username = "komendant", Password = "Komendant123!", RoleName = "Комендант" },
                new { Username = "kastylyanka", Password = "Kastylyanka123!", RoleName = "Кастелянша" },
                new { Username = "student", Password = "Student123!", RoleName = "Студент" }
            };

            foreach (var user in testUsers)
            {
                if (roleIds.TryGetValue(user.RoleName, out int roleId))
                {
                    int newUserId = userRepository.AddUser(user.Username, user.Password, roleId);
                    Console.WriteLine($"Добавлен пользователь: {user.Username} (ID: {newUserId})");
                }
                else
                {
                    Console.WriteLine($"Ошибка: роль '{user.RoleName}' не найдена.");
                }
            }
        }
    }
}
