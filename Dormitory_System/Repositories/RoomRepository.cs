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
    public class RoomRepository
    {
        public ObservableCollection<Room> GetAllRooms()
        {
            var rooms = new ObservableCollection<Room>();
            string query = "SELECT r.*, b.block_name FROM Rooms r JOIN Blocks b ON r.block_id = b.block_id";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var room = new Room
                            {
                                RoomId = Convert.ToInt32(reader["room_id"]),
                                RoomNumber = reader["room_number"].ToString(),
                                BlockId = Convert.ToInt32(reader["block_id"]),
                                RoomType = reader["room_type"].ToString(),
                                Capacity = Convert.ToInt32(reader["capacity"]),
                                Block = new Block
                                {
                                    BlockId = Convert.ToInt32(reader["block_id"]),
                                    BlockName = reader["block_name"].ToString()
                                }
                            };
                            rooms.Add(room);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return rooms;
        }

        public void AddRoom(Room room)
        {
            string query = "INSERT INTO Rooms (room_number, block_id, room_type, capacity) VALUES (@RoomNumber, @BlockId, @RoomType, @Capacity)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    command.Parameters.AddWithValue("@BlockId", room.BlockId);
                    command.Parameters.AddWithValue("@RoomType", room.RoomType);
                    command.Parameters.AddWithValue("@Capacity", room.Capacity);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void DeleteRoom(int roomId)
        {
            string query = "DELETE FROM Rooms WHERE room_id = @RoomId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}
