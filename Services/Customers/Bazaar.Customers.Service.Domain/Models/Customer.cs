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

        public static Customer Create(
            string name, 
            string lastName, 
            string email, 
            string passwordHash, 
            string phone, 
            string address, 
            string city, 
            string postalCode, 
            string country)
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
                Country = country
            };
        }
    }
}
