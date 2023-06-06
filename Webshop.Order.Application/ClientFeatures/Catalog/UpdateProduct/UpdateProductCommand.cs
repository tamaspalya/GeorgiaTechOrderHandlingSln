using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.ClientFeatures.Catalog.UpdateProduct
{
    public class UpdateProductCommand: ICommand
    {
        public UpdateProductCommand(Service.CatalogClient.Models.ProductDto product)
        {
            Ensure.That(product, nameof(product)).IsNotNull();
            Ensure.That(product.Id, nameof(product.Id)).IsNotDefault();
            Ensure.That(product.Id, nameof(product.Id)).IsGt<int>(0);

            Ensure.That(product.AmountInStock, nameof(product.AmountInStock)).IsGt<int>(0);

            Product = product;
        }

        public Service.CatalogClient.Models.ProductDto Product { get; set; }
    }
}
