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
    public class VisitRepository
    {
        public ObservableCollection<Visit> GetVisitsByHostStudentId(int hostStudentId)
        {
            var visits = new ObservableCollection<Visit>();
            string query = "SELECT * FROM Visits WHERE host_student_id = @HostStudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@HostStudentId", hostStudentId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var visit = new Visit
                            {
                                VisitId = Convert.ToInt32(reader["visit_id"]),
                                GuestName = reader["guest_name"].ToString(),
                                HostStudentId = Convert.ToInt32(reader["host_student_id"]),
                                TimeIn = Convert.ToDateTime(reader["time_in"]),
                                TimeOut = reader["time_out"] != DBNull.Value ? Convert.ToDateTime(reader["time_out"]) : (DateTime?)null
                            };
                            visits.Add(visit);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return visits;
        }

        public void AddVisit(Visit visit)
        {
            string query = "INSERT INTO Visits (guest_name, host_student_id, time_in) VALUES (@GuestName, @HostStudentId, @TimeIn)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestName", visit.GuestName);
                    command.Parameters.AddWithValue("@HostStudentId", visit.HostStudentId);
                    command.Parameters.AddWithValue("@TimeIn", visit.TimeIn);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void UpdateVisit(Visit visit)
        {
            string query = "UPDATE Visits SET guest_name = @GuestName, host_student_id = @HostStudentId, time_in = @TimeIn, time_out = @TimeOut WHERE visit_id = @VisitId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestName", visit.GuestName);
                    command.Parameters.AddWithValue("@HostStudentId", visit.HostStudentId);
                    command.Parameters.AddWithValue("@TimeIn", visit.TimeIn);
                    command.Parameters.AddWithValue("@TimeOut", visit.TimeOut ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@VisitId", visit.VisitId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void DeleteVisit(int visitId)
        {
            string query = "DELETE FROM Visits WHERE visit_id = @VisitId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VisitId", visitId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}
