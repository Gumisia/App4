using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication1.Repositories
{
    public class ProductWarehouseRepository : Repository
    {
        public async Task<bool> ExistsByIdOrder(int orderId)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand("SELECT 1 FROM Product_Warehouse WHERE IdOrder = @Id", connection);
            command.Parameters.AddWithValue("@Id", orderId);

            try
            {
                await connection.OpenAsync();
                var result = (int?)await command.ExecuteScalarAsync();
                return result.HasValue;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<int> Create(int idWarehouse, int idProduct, int idOrder, int amount, DateTime createdAt)
        {
            const string query = @"
INSERT INTO Product_Warehouse (IdWarehouse, IdOrder, IdProduct, Amount, Price, CreatedAt)
VALUES (@IdWarehouse,@IdOrder,@IdProduct,@Amount,(SELECT Price * @Amount FROM Product WHERE IdProduct = 1),@CreatedAt);

SELECT @@IDENTITY;
";

            using SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
            command.Parameters.AddWithValue("@IdOrder", idProduct);
            command.Parameters.AddWithValue("@IdProduct", idOrder);
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@CreatedAt", createdAt);

            try
            {
                await connection.OpenAsync();
                var result = (decimal)await command.ExecuteScalarAsync();
                return decimal.ToInt32(result);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
