using Bazaar.Products.Service.Domain.Models;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Tests.Domain
{
    public class ProductTests
    {
        [Fact]
        public void Create_ShouldReturnProduct_WithCorrectProperties()
        {
            var customerId = Guid.NewGuid();

            var product = Product.Create(
                customerId, "Test Product", "Description", "Electronics",
                99.99m, Currency.PLN, "pcs", 1.5m, true);

            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Equal(customerId, product.CustomerId);
            Assert.Equal("Test Product", product.Name);
            Assert.Equal(99.99m, product.Price);
            Assert.Equal(Currency.PLN, product.Currency);
            Assert.True(product.IsAvailable);
        }

        [Fact]
        public void Update_ShouldUpdateOnlyProvidedFields()
        {
            var product = Product.Create(
                Guid.NewGuid(), "Old Name", "Old Desc", "Old Cat",
                50m, Currency.PLN, "pcs", 1m, true);

            product.Update("New Name", null, null, 75m, Currency.EUR, null, null, false);

            Assert.Equal("New Name", product.Name);
            Assert.Equal("Old Desc", product.Description);
            Assert.Equal("Old Cat", product.Category);
            Assert.Equal(75m, product.Price);
            Assert.Equal(Currency.EUR, product.Currency);
            Assert.Equal("pcs", product.Unit);
            Assert.False(product.IsAvailable);
        }

        [Fact]
        public void ChangeProductPrice_WithValidValues_ShouldUpdate()
        {
            var product = Product.Create(
                Guid.NewGuid(), "Product", "Desc", "Cat",
                50m, Currency.PLN, "pcs", 1m);

            product.ChangeProductPrice(100m, Currency.EUR);

            Assert.Equal(100m, product.Price);
            Assert.Equal(Currency.EUR, product.Currency);
        }

        [Fact]
        public void ChangeProductAvailability_ShouldToggle()
        {
            var product = Product.Create(
                Guid.NewGuid(), "Product", "Desc", "Cat",
                50m, Currency.PLN, "pcs", 1m, true);

            product.ChangeProductAvailability(false);

            Assert.False(product.IsAvailable);
        }
    }
}
