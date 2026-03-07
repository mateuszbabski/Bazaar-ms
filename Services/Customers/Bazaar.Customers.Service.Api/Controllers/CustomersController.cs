using Bazaar.Customers.Service.Commands.Handlers.Update;
using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Customers.Service.Queries.Handlers;
using Bazaar.Customers.Service.Queries.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Customers.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly UpdateCustomerCommandHandler _updateCustomerHandler;
        private readonly GetCustomerQueryHandler _getCustomerHandler;

        public CustomersController(
            UpdateCustomerCommandHandler updateCustomerHandler,
            GetCustomerQueryHandler getCustomerHandler)
        {
            _updateCustomerHandler = updateCustomerHandler;
            _getCustomerHandler = getCustomerHandler;
        }

        [HttpGet]
        public async Task<ActionResult<GetCustomerResult>> Get()
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || userRole != "Customer")
                return Unauthorized("Unauthorized");

            var result = await _getCustomerHandler.Handle(userId);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            { 
                ErrorCode.CustomerNotFound => NotFound(result.ErrorMessage),
                _ => StatusCode(500, result)
            };
        }

        [HttpPatch("me")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();
            var userRole = Request.Headers["X-User-Role"].ToString();
            if (string.IsNullOrEmpty(userId) || userRole != "Customer")
                return Unauthorized("Unauthorized");

            command.Id = Guid.Parse(userId);

            var result = await _updateCustomerHandler.Handle(command);
            if(result.IsSuccess)
                return Ok(result);
            
            return result.ErrorCode switch
            {
                ErrorCode.CustomerNotFound => NotFound(result.ErrorMessage),
                ErrorCode.ValidationFailed => BadRequest(result.ErrorMessage),
                _ => StatusCode(500, result)
            };
        }
    }
}
