using Dormitory_System.DAL;
using Dormitory_System.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Repositories
{
    public class DepartmentRepository
    {
        public ObservableCollection<Department> GetAllDepartments()
        {
            var departments = new ObservableCollection<Department>();
            string query = "SELECT * FROM Departments";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var department = new Department
                            {
                                DepartmentId = Convert.ToInt32(reader["department_id"]),
                                Name = reader["name"].ToString()
                            };
                            departments.Add(department);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return departments;
        }

        public Department GetDepartmentById(int departmentId)
        {
            string query = "SELECT * FROM Departments WHERE department_id = @DepartmentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", departmentId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Department
                            {
                                DepartmentId = Convert.ToInt32(reader["department_id"]),
                                Name = reader["name"].ToString()
                            };
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return null;
        }

        public int GetDepartmentIdByName(string departmentName)
        {
            string query = "SELECT department_id FROM Departments WHERE name = @DepartmentName";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentName", departmentName);
                    var result = command.ExecuteScalar();
                    DbConnectionManager.CloseConnection(connection);
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}
