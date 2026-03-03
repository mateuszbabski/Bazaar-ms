using Bazaar.Customers.Service.Commands.Handlers.SignUp;
using Bazaar.Customers.Service.Commands.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Customers.Service.Api.Controllers
{
    [ApiController]
    [Route("api/customers/auth")]
    public class AuthControllers : ControllerBase
    {
        private readonly SignUpCommandHandler _signUpCommandHandler;

        public AuthControllers(SignUpCommandHandler signUpCommandHandler)
        {
            _signUpCommandHandler = signUpCommandHandler;
        }

        [HttpPost]
        public async Task<ActionResult<SignUpResponse>> SignUp([FromBody] SignUpCommand command)
        {
            var result = await _signUpCommandHandler.Handle(command);

            return Ok(result);
        }
    }
}
