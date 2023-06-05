using EnsureThat;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.ClientFeatures.Catalog.GetProduct
{
    public class GetProductQuery: IQuery<ProductDto>
    {
        public GetProductQuery(int productId)
        {
            Ensure.That(productId, nameof(productId)).IsNotDefault<int>();
            Ensure.That(productId, nameof(productId)).IsGt<int>(0);
            ProductId = productId;
        }

        public int ProductId { get; private set; }
    }
}
