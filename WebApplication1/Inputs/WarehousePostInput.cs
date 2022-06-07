using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Inputs
{
    public class WarehousePostInput
    {
            [Required]
            [Range(0, int.MaxValue)]
            public int IdProduct { get; set; }

            [Required]
            [Range(0, int.MaxValue)]
            public int IdWarehouse { get; set; }

            [Required]
            [Range(1, int.MaxValue)]
            public int Amount { get; set; }

            [Required]
            public DateTime CreatedAt { get; set; }
    }
}
