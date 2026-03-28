using Bazaar.Products.Service.Commands.Handlers.AddProduct;
using Bazaar.Products.Service.DataAccess.Repository;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Domain.Models;
using NSubstitute;

namespace Bazaar.Products.Service.Tests.Commands
{
    public class AddProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly AddProductCommandHandler _handler;

        public AddProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new AddProductCommandHandler(_productRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldReturnSuccess()
        {
            var command = new AddProductCommand
            {
                CustomerId = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Description",
                Category = "Electronics",
                Price = 99.99m,
                Currency = "PLN",
                Unit = "pcs",
                WeightPerUnitInKg = 1.5m,
                IsAvailable = true
            };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _productRepository.Received(1).Add(Arg.Any<Product>());
        }

        [Fact]
        public async Task Handle_WithInvalidCurrency_ShouldReturnValidationFailed()
        {
            var command = new AddProductCommand
            {
                CustomerId = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Description",
                Category = "Electronics",
                Price = 99.99m,
                Currency = "INVALID",
                Unit = "pcs",
                WeightPerUnitInKg = 1.5m,
                IsAvailable = true
            };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ValidationFailed, result.ErrorCode);
        }
    }
}
