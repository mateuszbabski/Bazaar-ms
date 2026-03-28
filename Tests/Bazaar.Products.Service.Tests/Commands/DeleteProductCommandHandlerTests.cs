using Bazaar.Products.Service.Commands.Handlers.DeleteProduct;
using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Products.Service.Tests.Commands
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_productRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = Product.Create(customerId, "Product", "Desc", "Cat", 50m, Currency.PLN, "pcs", 1m);
            product.Id = productId;

            _productRepository.GetById(productId).Returns(product);

            var command = new DeleteProductCommand { Id = productId, CustomerId = customerId };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _productRepository.Received(1).Delete(product);
        }

        [Fact]
        public async Task Handle_WithNonExistentProduct_ShouldReturnProductNotFound()
        {
            var productId = Guid.NewGuid();
            _productRepository.GetById(productId).Returns((Product)null!);

            var command = new DeleteProductCommand { Id = productId, CustomerId = Guid.NewGuid() };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ProductNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithDifferentCustomer_ShouldReturnForbidden()
        {
            var productId = Guid.NewGuid();
            var product = Product.Create(Guid.NewGuid(), "Product", "Desc", "Cat", 50m, Currency.PLN, "pcs", 1m);
            product.Id = productId;

            _productRepository.GetById(productId).Returns(product);

            var command = new DeleteProductCommand { Id = productId, CustomerId = Guid.NewGuid() };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.Forbidden, result.ErrorCode);
        }
    }
}
