using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WebApplication1.Repositories
{
    public class ProductRepository : Repository
    {
        public async Task<bool> Exists(int id)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand("SELECT 1 FROM Product WHERE IdProduct = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

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
    }
}
