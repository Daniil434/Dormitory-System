using Dormitory_System.DAL;
using Dormitory_System.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dormitory_System.Repositories
{
    public class StudentRepository
    {
        public ObservableCollection<Student> GetAllStudents()
        {
            var students = new ObservableCollection<Student>();
            string query = "SELECT * FROM Students";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                FullName = reader["full_name"].ToString(),
                                PassportData = reader["passport_data"].ToString(),
                                DepartmentName = reader["department_name"].ToString(),
                                Course = Convert.ToInt32(reader["course"]),
                                ContactPhone = reader["contact_phone"].ToString()
                            };
                            students.Add(student);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return students;
        }

        public Student GetStudentById(int studentId)
        {
            string query = "SELECT * FROM Students WHERE student_id = @StudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student
                            {
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                FullName = reader["full_name"].ToString(),
                                PassportData = reader["passport_data"].ToString(),
                                DepartmentName = reader["department_name"].ToString(),
                                Course = Convert.ToInt32(reader["course"]),
                                ContactPhone = reader["contact_phone"].ToString()
                            };
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return null;
        }

        public Student GetStudentByUserId(int userId)
        {
            string query = "SELECT s.* FROM Students s JOIN Users u ON s.user_id = u.user_id WHERE u.user_id = @UserId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student
                            {
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                FullName = reader["full_name"].ToString(),
                                PassportData = reader["passport_data"].ToString(),
                                DepartmentName = reader["department_name"].ToString(),
                                Course = Convert.ToInt32(reader["course"]),
                                ContactPhone = reader["contact_phone"].ToString()
                            };
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return null;
        }

        public void AddStudent(Student student, int userId)
        {
            string query = "INSERT INTO Students (full_name, passport_data, department_name, course, contact_phone, user_id) VALUES (@FullName, @PassportData, @DepartmentName, @Course, @ContactPhone, @UserId)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", student.FullName);
                    command.Parameters.AddWithValue("@PassportData", student.PassportData);
                    command.Parameters.AddWithValue("@DepartmentName", student.DepartmentName);
                    command.Parameters.AddWithValue("@Course", student.Course);
                    command.Parameters.AddWithValue("@ContactPhone", student.ContactPhone);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void UpdateStudent(Student student)
        {
            string query = "UPDATE Students SET full_name = @FullName, passport_data = @PassportData, department_name = @DepartmentName, course = @Course, contact_phone = @ContactPhone WHERE student_id = @StudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", student.FullName);
                    command.Parameters.AddWithValue("@PassportData", student.PassportData);
                    command.Parameters.AddWithValue("@DepartmentName", student.DepartmentName);
                    command.Parameters.AddWithValue("@Course", student.Course);
                    command.Parameters.AddWithValue("@ContactPhone", student.ContactPhone);
                    command.Parameters.AddWithValue("@StudentId", student.StudentId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void DeleteStudent(int studentId)
        {
            string query = "DELETE FROM Students WHERE student_id = @StudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}
