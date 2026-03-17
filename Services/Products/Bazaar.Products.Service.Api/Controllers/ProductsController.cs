using Bazaar.Products.Service.Commands.Handlers.AddProduct;
using Bazaar.Products.Service.Commands.Responses;
using Bazaar.Shared.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Products.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AddProductCommandHandler _addProductCommandHandler;

        public ProductsController(AddProductCommandHandler addProductCommandHandler)
        {
            _addProductCommandHandler = addProductCommandHandler;
        }

        [HttpPost]
        public async Task<ActionResult<AddProductCommandResult>> AddProduct([FromBody] AddProductCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"];
            if (string.IsNullOrEmpty(userId) || userRole != Role.Shop)
                return Unauthorized("Unauthorized");

            command.CustomerId = Guid.Parse(userId);
            var result = await _addProductCommandHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                Domain.ErrorCodes.ErrorCode.ValidationFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }
    }
}
