using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Inputs;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/warehouses2")]
    public class Warehouse2Controller : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post(WarehousePostInput input)
        {
            using SqlConnection connection = new SqlConnection(Repository.ConnectionString);
            var command = new SqlCommand("AddProductToWarehouse", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IdProduct", input.IdProduct);
            command.Parameters.AddWithValue("@IdWarehouse", input.IdWarehouse);
            command.Parameters.AddWithValue("@Amount", input.Amount);
            command.Parameters.AddWithValue("@CreatedAt", input.CreatedAt);

            try
            {
                await connection.OpenAsync();
                var result = (decimal)await command.ExecuteScalarAsync();
                return Ok(decimal.ToInt32(result));
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
