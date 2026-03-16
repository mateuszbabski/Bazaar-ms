using Bazaar.Api.Gateway.Clients;
using Bazaar.Api.Gateway.Requests.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly CustomersServiceClient _serviceClient;

        public CustomersController(CustomersServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCustomer()
        {
            var response = await _serviceClient.GetCustomer();
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPatch("me")]
        [Authorize]
        public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerRequest updateRequest)
        {
            var response = await _serviceClient.UpdateCustomer(updateRequest);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPatch("me/updatetoshop")]
        [Authorize]
        public async Task<ActionResult> UpdateCustomerToShop([FromBody] UpdateCustomerToShopRequest updateRequest)
        {
            var response = await _serviceClient.UpdateCustomerToShop(updateRequest);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp([FromBody] SignUpCustomerRequest signUpRequest)
        {
            var response = await _serviceClient.SignUp(signUpRequest);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn([FromBody] SignInCustomerRequest signInRequest)
        {
            var response = await _serviceClient.SignIn(signInRequest);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
