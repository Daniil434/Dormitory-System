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
    public class BlockRepository
    {
        public ObservableCollection<Block> GetAllBlocks()
        {
            var blocks = new ObservableCollection<Block>();
            string query = "SELECT * FROM Blocks";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var block = new Block
                            {
                                BlockId = Convert.ToInt32(reader["block_id"]),
                                BlockName = reader["block_name"].ToString()
                            };
                            blocks.Add(block);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return blocks;
        }

        public void AddBlock(Block block)
        {
            string query = "INSERT INTO Blocks (block_name) VALUES (@BlockName)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlockName", block.BlockName);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public void DeleteBlock(int blockId)
        {
            string query = "DELETE FROM Blocks WHERE block_id = @BlockId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BlockId", blockId);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }
    }
}
