namespace Webshop.Order.Api.Exceptions
{
    public class ProductNotFoundException: Exception
    {
        public int ProductId { get; }

        public ProductNotFoundException(int productId)
            : base($"Product with id {productId} not found")
        {
            ProductId = productId;
        }
    }
}
