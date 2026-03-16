using Bazaar.Customers.Service.Commands.Handlers.SignIn;
using Bazaar.Customers.Service.Commands.Handlers.SignUp;
using Bazaar.Customers.Service.Commands.Responses;
using Bazaar.Customers.Service.Domain.ErrorCodes;
using Bazaar.Shared.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Customers.Service.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignUpCommandHandler _signUpCommandHandler;
        private readonly SignInCommandHandler _signInCommandHandler;

        public AuthController(SignUpCommandHandler signUpCommandHandler, SignInCommandHandler signInCommandHandler)
        {
            _signUpCommandHandler = signUpCommandHandler;
            _signInCommandHandler = signInCommandHandler;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<SignUpResult>> SignUp([FromBody] SignUpCommand command)
        {
            var result = await _signUpCommandHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return result.ErrorCode switch
            {
                ErrorCode.EmailAlreadyRegistered => Conflict(result),
                ErrorCode.ValidationFailed => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }

        [HttpPost("signin")]
        public async Task<ActionResult<AuthenticationResult>> SignIn([FromBody] SignInCommand command)
        {
            var result = await _signInCommandHandler.Handle(command);
            if (result.IsSuccess)
                return Ok(result);

            return Unauthorized(result);
        }
    }
}
