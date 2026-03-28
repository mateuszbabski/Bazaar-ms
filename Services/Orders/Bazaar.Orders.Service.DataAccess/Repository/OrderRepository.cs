using Bazaar.Orders.Service.DataAccess.Context;
using Bazaar.Orders.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Orders.Service.DataAccess.Repository
{
    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _dbContext;

        public OrderRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> Add(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetById(Guid id)
        {
            return await _dbContext.Orders.Include(x => x.Items)
                                          .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByCustomerId(Guid customerId, OrderStatus? status)
        {
            var query = _dbContext.Orders.Include(x => x.Items)
                                         .Where(x => x.CustomerId == customerId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByShopId(Guid shopId, OrderStatus? status)
        {
            var query = _dbContext.Orders.Include(x => x.Items)
                                         .Where(x => x.Items.Any(i => i.ShopId == shopId));

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task Update(Order order)
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
