using Bazaar.Shared.Models.Models;

namespace Bazaar.Customers.Service.Domain.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set;  }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string? ShopName { get; set; } = "";
        public Role Role { get; set; } = Role.Customer;

        public static Customer Create(
            string name, 
            string lastName, 
            string email, 
            string passwordHash, 
            string phone, 
            string address, 
            string city, 
            string postalCode, 
            string country,
            Role role,
            string? shopName)
        {
            return new Customer
            {
                Id = Guid.NewGuid(),
                Name = name,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash,
                Phone = phone,
                Address = address,
                City = city,
                PostalCode = postalCode,
                Country = country,
                Role = role,
                ShopName = shopName,
            };
        }

        public void Update(
            string? name,
            string? lastName,
            string? phone,
            string? address,
            string? city,
            string? postalCode, 
            string? country)
        {
            if (!string.IsNullOrEmpty(name))
                Name = name;
            if (!string.IsNullOrEmpty(lastName))
                LastName = lastName;
            if (!string.IsNullOrEmpty(phone))
                Phone = phone;
            if (!string.IsNullOrEmpty(address))
                Address = address;
            if (!string.IsNullOrEmpty(city))
                City = city;
            if (!string.IsNullOrEmpty(postalCode))
                PostalCode = postalCode;
            if (!string.IsNullOrEmpty(country))
                Country = country;
        }

        public void UpdateToShop(string shopName)
        {
            ShopName = shopName;
            Role = Role.Shop;
        }
    }
}
