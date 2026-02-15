// ApplicationRepository.cs
using Dormitory_System.DAL;
using Dormitory_System.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Dormitory_System.Repositories
{
    public class ApplicationRepository
    {
        public ObservableCollection<Application> GetApplicationsByStudentId(int studentId)
        {
            var applications = new ObservableCollection<Application>();
            string query = @"
                SELECT a.*, r.room_number, s.full_name AS staff_name
                FROM Applications a
                LEFT JOIN Rooms r ON a.room_id = r.room_id
                LEFT JOIN Staff s ON a.assigned_to_staff_id = s.staff_id
                WHERE a.student_id = @StudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var application = new Application
                            {
                                ApplicationId = Convert.ToInt32(reader["application_id"]),
                                RoomId = Convert.ToInt32(reader["room_id"]),
                                Description = reader["description"].ToString(),
                                ReportDate = Convert.ToDateTime(reader["report_date"]),
                                Status = reader["status"].ToString(),
                                AssignedToStaffId = reader["assigned_to_staff_id"] != DBNull.Value ? Convert.ToInt32(reader["assigned_to_staff_id"]) : (int?)null,
                                Room = new Room
                                {
                                    RoomNumber = reader["room_number"].ToString()
                                },
                                AssignedStaff = reader["staff_name"] != DBNull.Value ? new Staff
                                {
                                    FullName = reader["staff_name"].ToString()
                                } : null
                            };
                            applications.Add(application);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return applications;
        }


        public void AddApplication(Application application, int studentId)
        {
            string query = "INSERT INTO Applications (room_id, description, report_date, status, student_id) VALUES (@RoomId, @Description, @ReportDate, @Status, @StudentId)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", application.RoomId);
                    command.Parameters.AddWithValue("@Description", application.Description);
                    command.Parameters.AddWithValue("@ReportDate", application.ReportDate);
                    command.Parameters.AddWithValue("@Status", application.Status);
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}