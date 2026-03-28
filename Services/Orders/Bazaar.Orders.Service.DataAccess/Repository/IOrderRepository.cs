using Bazaar.Orders.Service.Domain.Models;

namespace Bazaar.Orders.Service.DataAccess.Repository
{
    public interface IOrderRepository
    {
        Task<Order> Add(Order order);
        Task<Order> GetById(Guid id);
        Task<IEnumerable<Order>> GetByCustomerId(Guid customerId, OrderStatus? status);
        Task<IEnumerable<Order>> GetByShopId(Guid shopId, OrderStatus? status);
        Task Update(Order order);
    }
}
