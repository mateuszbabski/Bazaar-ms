using Bazaar.Baskets.Service.DataAccess.Context;
using Bazaar.Baskets.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Baskets.Service.DataAccess.Repository
{
    internal sealed class BasketRepository : IBasketRepository
    {
        private readonly BasketsDbContext _dbContext;

        public BasketRepository(BasketsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Basket> Add(Basket basket)
        {
            await _dbContext.Baskets.AddAsync(basket);
            await _dbContext.SaveChangesAsync();

            return basket;
        }

        public void Delete(Basket basket)
        {
            _dbContext.Baskets.Remove(basket);
        }

        public async Task<Basket> GetByCustomerId(Guid customerId)
        {
            return await _dbContext.Baskets.Include(x => x.Items)
                                           .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<Basket> GetById(Guid id)
        {
            return await _dbContext.Baskets.Include(x => x.Items)
                                           .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveItem(Basket basket, Guid basketItemId)
        {
            var product = await _dbContext.BasketItems.FirstOrDefaultAsync(x => x.Id == basketItemId && x.BasketId == basket.Id);

            if (product != null)
            {
                _dbContext.BasketItems.Remove(product);

                if (basket.Items.Count == 0)
                {
                    _dbContext.Baskets.Remove(basket);
                }
            }
        }
    }
}
