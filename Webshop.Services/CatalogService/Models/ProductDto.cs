using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Webshop.Services.CatalogService.Models
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string SKU { get; set; }
		public int Price { get; set; }
		public string Currency { get; set; }
		public string? Description { get; set; }
		public int? AmountInStock { get; set; }
		public int? MinStock { get; set; }
	}
}

