using Bazaar.Products.Service.Commands.Handlers.AddProduct;
using Bazaar.Products.Service.Commands.Handlers.UpdateProduct;
using Bazaar.Products.Service.Commands.Responses;
using Bazaar.Products.Service.Domain.ErrorCodes;
using Bazaar.Products.Service.Queries.Handlers.GetAllProducts;
using Bazaar.Products.Service.Queries.Responses;
using Bazaar.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Products.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AddProductCommandHandler _addProductCommandHandler;
        private readonly GetAllProductsQueryHandler _getAllProductsQueryHandler;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;

        public ProductsController(AddProductCommandHandler addProductCommandHandler,
            GetAllProductsQueryHandler getAllProductsQueryHandler,
            UpdateProductCommandHandler updateProductCommandHandler)
        {
            _addProductCommandHandler = addProductCommandHandler;
            _getAllProductsQueryHandler = getAllProductsQueryHandler;
            _updateProductCommandHandler = updateProductCommandHandler;
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
    }
}
