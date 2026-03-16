namespace Bazaar.Shops.Service.Domain.Models
{
    public class Shop
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string OwnerName { get; private set; }
        public string OwnerLastName { get; private set; }
        public string ShopName { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string Role { get; private set; } = "Shop";

        public static Shop Create(string email,
                                  string passwordHash,
                                  string ownerName,
                                  string ownerLastName,
                                  string shopName,
                                  string address,
                                  string city,
                                  string postalCode,
                                  string country,
                                  string phone)
        {
            return new Shop
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                OwnerName = ownerName,
                OwnerLastName = ownerLastName,
                ShopName = shopName,
                Phone = phone,
                Address = address,
                City = city,
                PostalCode = postalCode,
                Country = country,
                Role = "Shop"
            };
        }
    }
}
