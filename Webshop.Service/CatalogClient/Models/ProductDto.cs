namespace Webshop.Service.CatalogClient.Models
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as ProductDto;

            return Id == other.Id
                && Name == other.Name
                && SKU == other.SKU
                && Price == other.Price
                && Currency == other.Currency
                && Description == other.Description
                && AmountInStock == other.AmountInStock
                && MinStock == other.MinStock;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + (Name?.GetHashCode() ?? 0);
                hash = hash * 23 + (SKU?.GetHashCode() ?? 0);
                hash = hash * 23 + Price.GetHashCode();
                hash = hash * 23 + (Currency?.GetHashCode() ?? 0);
                hash = hash * 23 + (Description?.GetHashCode() ?? 0);
                hash = hash * 23 + (AmountInStock?.GetHashCode() ?? 0);
                hash = hash * 23 + (MinStock?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}
