﻿

namespace Waseet.System.Services.Application.Dtos
{
    public class UpdateProductDTO
    {
        public int id { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public int category { get; set; }
    }
}
