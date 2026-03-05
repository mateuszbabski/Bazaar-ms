using Bazaar.Customers.Service.Commands.Handlers.SignIn;
using Bazaar.Customers.Service.Commands.Handlers.SignUp;
using Bazaar.Customers.Service.Commands.Responses;
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

            return Ok(result);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<AuthenticationResult>> SignIn([FromBody] SignInCommand command)
        {
            var result = await _signInCommandHandler.Handle(command);
            return Ok(result);
        }
    }
}
