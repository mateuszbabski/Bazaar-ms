using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazaar.Customers.Service.Api.Controllers
{    
    [ApiController]
    [Route("api/customers")]
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Health() => Ok();
    }
}
