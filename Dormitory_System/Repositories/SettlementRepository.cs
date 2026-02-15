// SettlementRepository.cs
using Dormitory_System.DAL;
using Dormitory_System.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Dormitory_System.Repositories
{
    public class SettlementRepository
    {
        public ObservableCollection<Settlement> GetAllSettlements()
        {
            var settlements = new ObservableCollection<Settlement>();
            string query = @"
                SELECT s.*, st.full_name, r.room_number, b.block_name 
                FROM Settlements s 
                JOIN Students st ON s.student_id = st.student_id 
                JOIN Rooms r ON s.room_id = r.room_id 
                JOIN Blocks b ON r.block_id = b.block_id";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var settlement = new Settlement
                            {
                                SettlementId = Convert.ToInt32(reader["settlement_id"]),
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                RoomId = Convert.ToInt32(reader["room_id"]),
                                DateIn = Convert.ToDateTime(reader["date_in"]),
                                DateOut = reader["date_out"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["date_out"]),
                                Student = new Student
                                {
                                    StudentId = Convert.ToInt32(reader["student_id"]),
                                    FullName = reader["full_name"].ToString()
                                },
                                Room = new Room
                                {
                                    RoomId = Convert.ToInt32(reader["room_id"]),
                                    RoomNumber = reader["room_number"].ToString(),
                                    Block = new Block
                                    {
                                        BlockId = Convert.ToInt32(reader["block_id"]),
                                        BlockName = reader["block_name"].ToString()
                                    }
                                }
                            };
                            settlements.Add(settlement);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return settlements;
        }

        public ObservableCollection<Settlement> GetSettlementsByStudentId(int studentId)
        {
            var settlements = new ObservableCollection<Settlement>();
            string query = @"
                SELECT s.*, r.room_number, b.block_name 
                FROM Settlements s 
                JOIN Rooms r ON s.room_id = r.room_id 
                JOIN Blocks b ON r.block_id = b.block_id 
                WHERE s.student_id = @StudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var settlement = new Settlement
                            {
                                SettlementId = Convert.ToInt32(reader["settlement_id"]),
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                RoomId = Convert.ToInt32(reader["room_id"]),
                                DateIn = Convert.ToDateTime(reader["date_in"]),
                                DateOut = reader["date_out"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["date_out"]),
                                Room = new Room
                                {
                                    RoomId = Convert.ToInt32(reader["room_id"]),
                                    RoomNumber = reader["room_number"].ToString(),
                                    Block = new Block
                                    {
                                        BlockId = Convert.ToInt32(reader["block_id"]),
                                        BlockName = reader["block_name"].ToString()
                                    }
                                }
                            };
                            settlements.Add(settlement);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return settlements;
        }

        public void CheckOutStudent(int settlementId)
        {
            string query = "UPDATE Settlements SET date_out = @DateOut WHERE settlement_id = @SettlementId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DateOut", DateTime.Now);
                    command.Parameters.AddWithValue("@SettlementId", settlementId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void SettleStudent(int studentId, int roomId)
        {
            string query = "INSERT INTO Settlements (student_id, room_id, date_in) VALUES (@StudentId, @RoomId, @DateIn)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.Parameters.AddWithValue("@DateIn", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}