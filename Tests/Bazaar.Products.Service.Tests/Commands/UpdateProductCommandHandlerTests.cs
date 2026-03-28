using Bazaar.Products.Service.Commands.Handlers.UpdateProduct;
using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Products.Service.Tests.Commands
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new UpdateProductCommandHandler(_productRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = Product.Create(customerId, "Product", "Desc", "Cat", 50m, Currency.PLN, "pcs", 1m);
            product.Id = productId;

            _productRepository.GetById(productId).Returns(product);

            var command = new UpdateProductCommand
            {
                Id = productId,
                CustomerId = customerId,
                Name = "Updated Product",
                Price = 75m
            };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            Assert.Equal("Updated Product", product.Name);
            await _productRepository.Received(1).Update(product);
        }

        [Fact]
        public async Task Handle_WithNonExistentProduct_ShouldReturnProductNotFound()
        {
            var productId = Guid.NewGuid();
            _productRepository.GetById(productId).Returns((Product)null!);

            var command = new UpdateProductCommand { Id = productId, CustomerId = Guid.NewGuid() };

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

            var command = new UpdateProductCommand { Id = productId, CustomerId = Guid.NewGuid() };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.Forbidden, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithInvalidCurrency_ShouldReturnValidationFailed()
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = Product.Create(customerId, "Product", "Desc", "Cat", 50m, Currency.PLN, "pcs", 1m);
            product.Id = productId;

            _productRepository.GetById(productId).Returns(product);

            var command = new UpdateProductCommand
            {
                Id = productId,
                CustomerId = customerId,
                Currency = "INVALID"
            };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ValidationFailed, result.ErrorCode);
        }
    }
}
