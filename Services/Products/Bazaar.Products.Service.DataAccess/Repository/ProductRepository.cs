using Bazaar.Products.Service.DataAccess.Context;
using Bazaar.Products.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Bazaar.Products.Service.DataAccess.Repository
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _dbContext;

        public ProductRepository(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> Add(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByShopId(Guid shopId)
        {
            return await _dbContext.Products
                .Where(p => p.CustomerId == shopId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProducts(
            string? name,
            string? category,
            decimal? minPrice,
            decimal? maxPrice)
        {
            var query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Contains(category));
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return await query.ToListAsync();
        }

        public async Task Update(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
