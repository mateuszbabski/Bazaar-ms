using Bazaar.Customers.Service.Domain.Models;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Customers.Service.Tests.Domain
{
    public class CustomerTests
    {
        [Fact]
        public void Create_ShouldReturnCustomer_WithCorrectProperties()
        {
            var customer = Customer.Create(
                "John", "Doe", "john@test.com", "hashedpwd",
                "123456789", "123 Street", "Warsaw", "00-001", "Poland",
                Role.Customer, null);

            Assert.NotEqual(Guid.Empty, customer.Id);
            Assert.Equal("John", customer.Name);
            Assert.Equal("Doe", customer.LastName);
            Assert.Equal("john@test.com", customer.Email);
            Assert.Equal(Role.Customer, customer.Role);
        }

        [Fact]
        public void Create_WithShopRole_ShouldSetShopName()
        {
            var customer = Customer.Create(
                "Shop", "Owner", "shop@test.com", "hashedpwd",
                "123456789", "123 Street", "Warsaw", "00-001", "Poland",
                Role.Shop, "My Shop");

            Assert.Equal(Role.Shop, customer.Role);
            Assert.Equal("My Shop", customer.ShopName);
        }

        [Fact]
        public void Update_ShouldOnlyUpdateNonEmptyFields()
        {
            var customer = Customer.Create(
                "John", "Doe", "john@test.com", "hashedpwd",
                "123456789", "123 Street", "Warsaw", "00-001", "Poland",
                Role.Customer, null);

            customer.Update("Jane", null, null, "456 Avenue", null, null, null);

            Assert.Equal("Jane", customer.Name);
            Assert.Equal("Doe", customer.LastName);
            Assert.Equal("123456789", customer.Phone);
            Assert.Equal("456 Avenue", customer.Address);
            Assert.Equal("Warsaw", customer.City);
        }

        [Fact]
        public void UpdateToShop_ShouldChangeRoleAndSetShopName()
        {
            var customer = Customer.Create(
                "John", "Doe", "john@test.com", "hashedpwd",
                "123456789", "123 Street", "Warsaw", "00-001", "Poland",
                Role.Customer, null);

            customer.UpdateToShop("John's Shop");

            Assert.Equal(Role.Shop, customer.Role);
            Assert.Equal("John's Shop", customer.ShopName);
        }
    }
}
