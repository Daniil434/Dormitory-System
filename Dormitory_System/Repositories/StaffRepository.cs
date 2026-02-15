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
    public class StaffRepository
    {
        public ObservableCollection<Staff> GetAllStaff()
        {
            var staff = new ObservableCollection<Staff>();
            string query = "SELECT * FROM Staff";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var staffMember = new Staff
                            {
                                StaffId = Convert.ToInt32(reader["staff_id"]),
                                FullName = reader["full_name"].ToString(),
                                Position = reader["position"].ToString(),
                                ContactPhone = reader["contact_phone"].ToString()
                            };
                            staff.Add(staffMember);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return staff;
        }

        public Staff GetStaffById(int staffId)
        {
            string query = "SELECT * FROM Staff WHERE staff_id = @StaffId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Staff
                            {
                                StaffId = Convert.ToInt32(reader["staff_id"]),
                                FullName = reader["full_name"].ToString(),
                                Position = reader["position"].ToString(),
                                ContactPhone = reader["contact_phone"].ToString()
                            };
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return null;
        }

        public void AddStaff(Staff staff)
        {
            string query = "INSERT INTO Staff (full_name, position, contact_phone) VALUES (@FullName, @Position, @ContactPhone)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", staff.FullName);
                    command.Parameters.AddWithValue("@Position", staff.Position);
                    command.Parameters.AddWithValue("@ContactPhone", staff.ContactPhone);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void UpdateStaff(Staff staff)
        {
            string query = "UPDATE Staff SET full_name = @FullName, position = @Position, contact_phone = @ContactPhone WHERE staff_id = @StaffId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", staff.FullName);
                    command.Parameters.AddWithValue("@Position", staff.Position);
                    command.Parameters.AddWithValue("@ContactPhone", staff.ContactPhone);
                    command.Parameters.AddWithValue("@StaffId", staff.StaffId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void DeleteStaff(int staffId)
        {
            string query = "DELETE FROM Staff WHERE staff_id = @StaffId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}
