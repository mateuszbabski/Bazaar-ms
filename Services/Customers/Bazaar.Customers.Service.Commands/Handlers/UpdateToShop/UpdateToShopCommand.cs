using System.ComponentModel.DataAnnotations;

namespace Bazaar.Customers.Service.Commands.Handlers.UpdateToShop
{
    public class UpdateToShopCommand
    {
        [Required]
        public Guid Id { get; set; }
        public string? ShopName { get; set; }
    }
}
