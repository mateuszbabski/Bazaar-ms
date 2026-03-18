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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllProducts()
        {
            var response = await _serviceClient.GetAllProducts();
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetProductById(Guid id)
        {
            var response = await _serviceClient.GetProductById(id);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpGet("shop/{shopId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetProductsByShopId(Guid shopId)
        {
            var response = await _serviceClient.GetProductsByShopId(shopId);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchProducts(
            [FromQuery] string? name,
            [FromQuery] string? category,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var query = new SearchProductsRequest
            {
                Name = name,
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            var response = await _serviceClient.SearchProducts(query);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductRequest updateProductRequest)
        {
            var response = await _serviceClient.UpdateProduct(updateProductRequest);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var response = await _serviceClient.DeleteProduct(id);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
