using System;
using Webshop.Services.CustomerService.Models;

namespace Webshop.Services.CatalogService.Models.Responses
{
	public class GetProductResponse
	{
        public ProductDto? Result { get; set; }
        public string? ErrorMessage { get; set; }
        public string? TimeGenerated { get; set; }
    }
}

