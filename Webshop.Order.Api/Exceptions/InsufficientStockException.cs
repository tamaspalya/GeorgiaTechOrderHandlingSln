namespace Webshop.Order.Api.Exceptions
{
    public class InsufficientStockException: Exception
    {
        public int ProductId { get; }
        public int RequestedQuantity { get; }
        public int AmountInStock { get; }

        public InsufficientStockException(int productId, int requestedQuantity, int amountInStock)
            : base($"There are not enough products in stock for product with id: {productId}. Requested quantity: {requestedQuantity}. Amount in stock: {amountInStock}")
        {
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
            AmountInStock = amountInStock;
        }
    }
}
