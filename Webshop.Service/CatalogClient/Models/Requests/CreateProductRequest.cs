namespace Webshop.Service.CatalogClient.Models.Requests
{
    public class CreateProductRequest
    {
        public string? Name { get; set; }
        public string? Sku { get; set; }
        public int Price { get; set; }
        public string? Currency { get; set; }
    }
}
