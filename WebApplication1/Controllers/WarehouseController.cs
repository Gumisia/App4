using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Inputs;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/warehouses")]
    public class WarehouseController : Controller
    {
        private readonly WarehouseRepository _warehouseRepository;
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ProductWarehouseRepository _productWarehouseRepository;

        public WarehouseController(WarehouseRepository warehouseRepository, ProductRepository productRepository, OrderRepository orderRepository, ProductWarehouseRepository productWarehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _productWarehouseRepository = productWarehouseRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(WarehousePostInput input)
        {
            if (!await _warehouseRepository.Exists(input.IdWarehouse))
            {
                return NotFound($"Hurtownia o id {input.IdWarehouse} nie istnieje.");
            }

            if (!await _productRepository.Exists(input.IdProduct))
            {
                return NotFound($"Produkt o id {input.IdProduct} nie istnieje");
            }

            var orderId = await _orderRepository.GetOrderIdForPlacingProduct(input.IdProduct, input.Amount, input.CreatedAt);
            if (!orderId.HasValue)
            {
                return NotFound("Nie znaleziono zamówienia pasującego do żądania");
            }

            if (await _productWarehouseRepository.ExistsByIdOrder(orderId.Value))
            {
                return BadRequest("Zlecenie dla tego żądania zostało już zrealizowane.");
            }

            await _orderRepository.FulfillOrderDate(orderId.Value, input.CreatedAt);

            return Ok(await _productWarehouseRepository.Create(input.IdWarehouse, input.IdProduct, orderId.Value, input.Amount, input.CreatedAt));
        }
    }
}
