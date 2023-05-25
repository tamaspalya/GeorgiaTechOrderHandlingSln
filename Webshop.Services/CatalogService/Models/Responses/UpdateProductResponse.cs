using System;
namespace Webshop.Services.CatalogService.Models.Responses
{
	public class UpdateProductResponse
	{
        public ProductDto? Result { get; set; }
        public string? ErrorMessage { get; set; }
        public string? TimeGenerated { get; set; }
    }
}

