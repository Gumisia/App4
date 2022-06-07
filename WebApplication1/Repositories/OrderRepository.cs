using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication1.Repositories
{    public class OrderRepository : Repository
    {
        public async Task<int?> GetOrderIdForPlacingProduct(int productId, int amount, DateTime createdAt)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand("SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt", connection);
            command.Parameters.AddWithValue("@IdProduct", productId);
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@CreatedAt", createdAt);

            try
            {
                await connection.OpenAsync();
                return (int?)await command.ExecuteScalarAsync();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task FulfillOrderDate(int orderId, DateTime createdAt)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand("UPDATE [Order] SET FulfilledAt = @CreatedAt WHERE IdOrder = @Id", connection);
            command.Parameters.AddWithValue("@Id", orderId);
            command.Parameters.AddWithValue("@CreatedAt", createdAt);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
