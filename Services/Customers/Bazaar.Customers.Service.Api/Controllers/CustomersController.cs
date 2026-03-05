using Bazaar.Customers.Service.Commands.Handlers.Update;
using Bazaar.Customers.Service.Commands.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Customers.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly UpdateCustomerCommandHandler _updateCustomerHandler;

        public CustomersController(UpdateCustomerCommandHandler updateCustomerHandler)
        {
            _updateCustomerHandler = updateCustomerHandler;
        }

        [HttpPatch("me")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand command)
        {
            var userId = Request.Headers["X-User-Id"].ToString();

            if (string.IsNullOrEmpty(userId))
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
