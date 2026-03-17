using Bazaar.Api.Gateway.Clients;
using Bazaar.Api.Gateway.Requests.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Api.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsServiceClient _serviceClient;

        public ProductsController(ProductsServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddProduct([FromBody] AddProductRequest addProductRequest)
        {
            var response = await _serviceClient.AddProduct(addProductRequest);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
