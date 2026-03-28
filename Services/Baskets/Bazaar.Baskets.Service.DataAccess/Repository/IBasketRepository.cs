using Bazaar.Baskets.Service.Domain.Models;

namespace Bazaar.Baskets.Service.DataAccess.Repository
{
    public interface IBasketRepository
    {
        Task<Basket> Add(Basket basket);
        Task<Basket> GetById(Guid id);
        Task<Basket> GetByCustomerId(Guid customerId);
        Task Delete(Basket basket);
        Task Update(Basket basket);
    }
}
