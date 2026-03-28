using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Domain.Models;
using Bazaar.Products.Service.Queries.Handlers.GetProductById;
using Bazaar.Products.Service.Queries.Handlers.GetAllProducts;
using Bazaar.Products.Service.Queries.Handlers.GetProductsByShopId;
using Bazaar.Products.Service.Queries.Handlers.SearchProducts;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Products.Service.Tests.Queries
{
    public class ProductQueryHandlerTests
    {
        private readonly IProductRepository _productRepository;

        public ProductQueryHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
        }

        [Fact]
        public async Task GetProductById_WithExistingProduct_ShouldReturnSuccess()
        {
            var handler = new GetProductByIdQueryHandler(_productRepository);
            var productId = Guid.NewGuid();
            var product = Product.Create(Guid.NewGuid(), "Product", "Desc", "Cat", 50m, Currency.PLN, "pcs", 1m);
            product.Id = productId;

            _productRepository.GetById(productId).Returns(product);

            var result = await handler.Handle(new GetProductByIdQuery { Id = productId });

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Product);
            Assert.Equal("Product", result.Product.Name);
        }

        [Fact]
        public async Task GetProductById_WithNonExistentProduct_ShouldReturnNotFound()
        {
            var handler = new GetProductByIdQueryHandler(_productRepository);
            var productId = Guid.NewGuid();
            _productRepository.GetById(productId).Returns((Product)null!);

            var result = await handler.Handle(new GetProductByIdQuery { Id = productId });

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ProductNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetAllProducts_WithExistingProducts_ShouldReturnSuccess()
        {
            var handler = new GetAllProductsQueryHandler(_productRepository);
            var products = new List<Product>
            {
                Product.Create(Guid.NewGuid(), "Product 1", "Desc", "Cat", 50m, Currency.PLN, "pcs", 1m),
                Product.Create(Guid.NewGuid(), "Product 2", "Desc", "Cat", 75m, Currency.EUR, "pcs", 2m)
            };

            _productRepository.GetAllProducts().Returns(products);

            var result = await handler.Handle();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Products!.Count);
        }

        [Fact]
        public async Task GetAllProducts_WithNoProducts_ShouldReturnNotFound()
        {
            var handler = new GetAllProductsQueryHandler(_productRepository);
            _productRepository.GetAllProducts().Returns(Enumerable.Empty<Product>());

            var result = await handler.Handle();

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ProductsNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task GetProductsByShopId_WithExistingProducts_ShouldReturnSuccess()
        {
            var handler = new GetProductsByShopIdQueryHandler(_productRepository);
            var shopId = Guid.NewGuid();
            var products = new List<Product>
            {
                Product.Create(shopId, "Product 1", "Desc", "Cat", 50m, Currency.PLN, "pcs", 1m)
            };

            _productRepository.GetProductsByShopId(shopId).Returns(products);

            var result = await handler.Handle(new GetProductsByShopIdQuery { ShopId = shopId });

            Assert.True(result.IsSuccess);
            Assert.Single(result.Products!);
        }

        [Fact]
        public async Task SearchProducts_WithResults_ShouldReturnSuccess()
        {
            var handler = new SearchProductsQueryHandler(_productRepository);
            var products = new List<Product>
            {
                Product.Create(Guid.NewGuid(), "Laptop", "Gaming", "Electronics", 3000m, Currency.PLN, "pcs", 2.5m)
            };

            _productRepository.SearchProducts("Laptop", null, null, null).Returns(products);

            var query = new SearchProductsQuery { Name = "Laptop" };

            var result = await handler.Handle(query);

            Assert.True(result.IsSuccess);
            Assert.Single(result.Products!);
        }

        [Fact]
        public async Task SearchProducts_WithNoResults_ShouldReturnNotFound()
        {
            var handler = new SearchProductsQueryHandler(_productRepository);
            _productRepository.SearchProducts("NonExistent", null, null, null).Returns(Enumerable.Empty<Product>());

            var query = new SearchProductsQuery { Name = "NonExistent" };

            var result = await handler.Handle(query);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ProductsNotFound, result.ErrorCode);
        }
    }
}
