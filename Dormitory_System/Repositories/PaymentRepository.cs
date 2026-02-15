// PaymentRepository.cs
using Dormitory_System.DAL;
using Dormitory_System.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Dormitory_System.Repositories
{
    public class PaymentRepository
    {
        public ObservableCollection<Payment> GetPaymentsByStudentId(int studentId, int page = 1, int pageSize = 10)
        {
            var payments = new ObservableCollection<Payment>();
            int offset = (page - 1) * pageSize;
            string query = @"
                SELECT p.*, st.full_name 
                FROM Payments p 
                JOIN Students st ON p.student_id = st.student_id 
                WHERE p.student_id = @StudentId 
                ORDER BY p.payment_date DESC 
                LIMIT @Offset, @PageSize";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var payment = new Payment
                            {
                                PaymentId = Convert.ToInt32(reader["payment_id"]),
                                StudentId = studentId,
                                Amount = Convert.ToDecimal(reader["amount"]),
                                PaymentDate = Convert.ToDateTime(reader["payment_date"]),
                                Purpose = reader["purpose"].ToString(),
                                Status = reader["status"].ToString(),
                                Student = new Student
                                {
                                    StudentId = studentId,
                                    FullName = reader["full_name"].ToString()
                                }
                            };
                            payments.Add(payment);
                        }
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return payments;
        }

        public void AddPayment(Payment payment)
        {
            string query = @"
                INSERT INTO Payments (student_id, amount, payment_date, purpose, status) 
                VALUES (@StudentId, @Amount, @PaymentDate, @Purpose, @Status)";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", payment.StudentId);
                    command.Parameters.AddWithValue("@Amount", payment.Amount);
                    command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                    command.Parameters.AddWithValue("@Purpose", payment.Purpose);
                    command.Parameters.AddWithValue("@Status", payment.Status);
                    command.ExecuteNonQuery();
                }
                DbConnectionManager.CloseConnection(connection);
            }
        }

        public decimal GetTotalBalanceByStudentId(int studentId)
        {
            decimal balance = 0;
            string query = "SELECT SUM(amount) FROM Payments WHERE student_id = @StudentId";

            using (MySqlConnection connection = DbConnectionManager.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        balance = Convert.ToDecimal(result);
                    }
                }
                DbConnectionManager.CloseConnection(connection);
            }
            return balance;
        }
    }
}