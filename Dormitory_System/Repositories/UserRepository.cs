using Dormitory_System.DAL;
using Dormitory_System.Entities;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Repositories
{
    public class UserRepository
    {
        // Метод для получения пользователя по имени (для аутентификации)
        public User GetUserByUsername(string username)
        {
            string query = @"
                SELECT u.user_id, u.username, u.password_hash, u.is_active, r.role_name
                FROM Users u
                JOIN Roles r ON u.role_id = r.role_id
                WHERE u.username = @username AND u.is_active = TRUE";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", username)
            };

            DataTable dt = DbConnectionManager.ExecuteSelectQuery(query, parameters);

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            return new User
            {
                UserId = Convert.ToInt32(row["user_id"]),
                Username = row["username"].ToString(),
                PasswordHash = row["password_hash"].ToString(),
                IsActive = Convert.ToBoolean(row["is_active"]),
                RoleName = row["role_name"].ToString()
            };
        }

        public ObservableCollection<User> GetAllUsers()
        {
            var users = new ObservableCollection<User>();
            string query = "SELECT u.*, r.role_name FROM Users u JOIN Roles r ON u.role_id = r.role_id";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                UserId = Convert.ToInt32(reader["user_id"]),
                                Username = reader["username"].ToString(),
                                PasswordHash = reader["password_hash"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"]),
                                RoleName = reader["role_name"].ToString(),
                                IsActive = Convert.ToBoolean(reader["is_active"])
                            };
                            users.Add(user);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return users;
        }

        // Метод для добавления нового пользователя (используем BCrypt для хеширования)
        public int AddUser(string username, string plainPassword, int roleId)
        {
            // 1. Генерируем хеш
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            string query = @"
                INSERT INTO Users (username, password_hash, role_id)
                VALUES (@username, @password_hash, @role_id);
                SELECT LAST_INSERT_ID();";

            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", username),
                new MySqlParameter("@password_hash", hashedPassword),
                new MySqlParameter("@role_id", roleId)
            };

            return Convert.ToInt32(DbConnectionManager.ExecuteScalar(query, parameters));
        }

    public void UpdateUser(User user)
        {
            string query = "UPDATE Users SET username = @Username, role_id = @RoleId, is_active = @IsActive WHERE user_id = @UserId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@RoleId", user.RoleId);
                    command.Parameters.AddWithValue("@IsActive", user.IsActive);
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void UpdateUserPassword(int userId, string plainPassword)
        {
            // Генерируем хеш пароля с использованием BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            string query = "UPDATE Users SET password_hash = @PasswordHash WHERE user_id = @UserId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void DeleteUser(int userId)
        {
            string query = "DELETE FROM Users WHERE user_id = @UserId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public bool VerifyPassword(int userId, string plainPassword)
        {
            string query = "SELECT password_hash FROM Users WHERE user_id = @UserId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var hashedPassword = command.ExecuteScalar()?.ToString();
                    DbConnectionManager.CloseConnection(connection);
                    if (hashedPassword != null)
                    {
                        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
                    }
                }
            }
            return false;
        }
    }
}
