using Bazaar.Products.Service.Commands.Handlers.AddProduct;
using Bazaar.Products.Service.Commands.Handlers.DeleteProduct;
using Bazaar.Products.Service.Commands.Handlers.UpdateProduct;
using Bazaar.Products.Service.Commands.Responses;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Queries.Handlers.GetAllProducts;
using Bazaar.Products.Service.Queries.Handlers.GetProductById;
using Bazaar.Products.Service.Queries.Handlers.GetProductsByShopId;
using Bazaar.Products.Service.Queries.Handlers.SearchProducts;
using Bazaar.Products.Service.Queries.Responses;
using Bazaar.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Bazaar.Products.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AddProductCommandHandler _addProductCommandHandler;
        private readonly GetAllProductsQueryHandler _getAllProductsQueryHandler;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;
        private readonly GetProductByIdQueryHandler _getProductByIdQueryHandler;
        private readonly GetProductsByShopIdQueryHandler _getProductsByShopIdQueryHandler;
        private readonly SearchProductsQueryHandler _searchProductsQueryHandler;
        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;

        public ProductsController(
            AddProductCommandHandler addProductCommandHandler,
            GetAllProductsQueryHandler getAllProductsQueryHandler,
            UpdateProductCommandHandler updateProductCommandHandler,
            GetProductByIdQueryHandler getProductByIdQueryHandler,
            GetProductsByShopIdQueryHandler getProductsByShopIdQueryHandler,
            SearchProductsQueryHandler searchProductsQueryHandler,
            DeleteProductCommandHandler deleteProductCommandHandler)
        {
            _addProductCommandHandler = addProductCommandHandler;
            _getAllProductsQueryHandler = getAllProductsQueryHandler;
            _updateProductCommandHandler = updateProductCommandHandler;
            _getProductByIdQueryHandler = getProductByIdQueryHandler;
            _getProductsByShopIdQueryHandler = getProductsByShopIdQueryHandler;
            _searchProductsQueryHandler = searchProductsQueryHandler;
            _deleteProductCommandHandler = deleteProductCommandHandler;
        }

        [HttpPost]
        public async Task<ActionResult<AddProductResult>> AddProduct([FromBody] AddProductCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Shop), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            command.CustomerId = Guid.Parse(userId);
            var result = await _addProductCommandHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.ValidationFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpGet]
        public async Task<ActionResult<GetAllProductsResult>> GetAllProducts()
        {
            var result = await _getAllProductsQueryHandler.Handle();
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.ProductsNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductByIdResult>> GetProductById(Guid id)
        {
            var result = await _getProductByIdQueryHandler.Handle(new GetProductByIdQuery { Id = id });
            if (result.IsSuccess)
                return Ok(result);
            return result.ErrorCode switch
            {
                ErrorCode.ProductNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpGet("shop/{shopId}")]
        public async Task<ActionResult<GetProductByIdResult>> GetProductByShopId(Guid shopId)
        {
            var result = await _getProductsByShopIdQueryHandler.Handle(new GetProductsByShopIdQuery { ShopId = shopId });
            if (result.IsSuccess)
                return Ok(result);
            return result.ErrorCode switch
            {
                ErrorCode.ProductsNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchProductsResult>> SearchProducts(
            [FromQuery] string? name,
            [FromQuery] string? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var query = new SearchProductsQuery
            {
                Name = name,
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var result = await _searchProductsQueryHandler.Handle(query);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.ProductsNotFound => NotFound(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPatch]
        public async Task<ActionResult<UpdateProductResult>> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Shop), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            command.CustomerId = Guid.Parse(userId);
            var result = await _updateProductCommandHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);
            return result.ErrorCode switch
            {
                ErrorCode.ProductNotFound => NotFound(result),
                ErrorCode.Forbidden => StatusCode(403, result),
                ErrorCode.ValidationFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteProductResult>> DeleteProduct(Guid id)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || !userRole.Equals(nameof(Role.Shop), StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Unauthorized");

            var result = await _deleteProductCommandHandler.Handle(
                new DeleteProductCommand 
                { 
                    Id = id,
                    CustomerId = Guid.Parse(userId)
                });
            if (result.IsSuccess)
                return Ok(result);
            return result.ErrorCode switch
            {
                ErrorCode.ProductNotFound => NotFound(result),
                ErrorCode.Forbidden => StatusCode(403, result),
                _ => StatusCode(500, result)
            };
        }
    }
}
