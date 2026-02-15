using MySql.Data.MySqlClient;
using System;
using System.Configuration;


namespace Dormitory_System.DAL
{
    public class DbConnectionManager
    {
        // Строка подключения (хранится в App.config или Web.config)
        private static readonly string _connectionString;

        static DbConnectionManager()
        {
            // Получаем строку подключения из конфигурационного файла
            _connectionString = ConfigurationManager.ConnectionStrings["DormitoryDB"].ConnectionString;
        }

        // Метод для получения открытого подключения
        public static MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // Метод для закрытия подключения
        public static void CloseConnection(MySqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Метод для выполнения SELECT-запроса и получения DataTable
        public static System.Data.DataTable ExecuteSelectQuery(string query, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = GetConnection())
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                System.Data.DataTable dataTable = new System.Data.DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Метод для выполнения INSERT/UPDATE/DELETE
        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = GetConnection())
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return command.ExecuteNonQuery();
            }
        }

        // Метод для выполнения запроса и получения одного значения
        public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = GetConnection())
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return command.ExecuteScalar();
            }
        }
    }
}
