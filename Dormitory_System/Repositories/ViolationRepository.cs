// ViolationRepository.cs
using Dormitory_System.DAL;
using Dormitory_System.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Dormitory_System.Repositories
{
    public class ViolationRepository
    {
        public ObservableCollection<Violation> GetAllViolations()
        {
            var violations = new ObservableCollection<Violation>();
            string query = @"
                SELECT v.*, st.full_name, rs.full_name as recorder_name 
                FROM Violations v 
                JOIN Students st ON v.student_id = st.student_id 
                LEFT JOIN Staff rs ON v.recorded_by_staff_id = rs.staff_id";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var violation = new Violation
                            {
                                ViolationId = Convert.ToInt32(reader["violation_id"]),
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                ViolationType = reader["violation_type"].ToString(),
                                ViolationDate = Convert.ToDateTime(reader["violation_date"]),
                                ActionTaken = reader["action_taken"]?.ToString(),
                                RecordedByStaffId = reader["recorded_by_staff_id"] != DBNull.Value ? Convert.ToInt32(reader["recorded_by_staff_id"]) : (int?)null,
                                Student = new Student
                                {
                                    StudentId = Convert.ToInt32(reader["student_id"]),
                                    FullName = reader["full_name"].ToString()
                                },
                                RecordedStaff = reader["recorder_name"] != DBNull.Value ? new Staff
                                {
                                    StaffId = Convert.ToInt32(reader["recorded_by_staff_id"]),
                                    FullName = reader["recorder_name"].ToString()
                                } : null
                            };
                            violations.Add(violation);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return violations;
        }

        public void AddViolation(Violation violation)
        {
            string query = @"
                INSERT INTO Violations (student_id, violation_type, violation_date, action_taken, recorded_by_staff_id) 
                VALUES (@StudentId, @ViolationType, @ViolationDate, @ActionTaken, @RecordedByStaffId)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", violation.StudentId);
                    command.Parameters.AddWithValue("@ViolationType", violation.ViolationType);
                    command.Parameters.AddWithValue("@ViolationDate", violation.ViolationDate);
                    command.Parameters.AddWithValue("@ActionTaken", (object)violation.ActionTaken ?? DBNull.Value);
                    command.Parameters.AddWithValue("@RecordedByStaffId", (object)violation.RecordedByStaffId ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}