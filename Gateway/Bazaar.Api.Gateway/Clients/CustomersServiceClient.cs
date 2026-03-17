using Bazaar.Api.Gateway.Requests.Customers;
using Bazaar.Shared.Infrastructure.Auth;

namespace Bazaar.Api.Gateway.Clients
{
    public class CustomersServiceClient : BaseServiceClient
    {
        public CustomersServiceClient(
            IHttpClientFactory httpClientFactory,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(httpClientFactory, serviceTokenManager, httpContextAccessor,
                   configuration["Services:CustomersService"])
        {
        }

        public async Task<HttpResponseMessage> SignUp(SignUpCustomerRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Post, "api/auth/signup", requestObject);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SignIn(SignInCustomerRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Post, "api/auth/signin", requestObject);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetCustomer()
        {
            var request = CreateRequest(HttpMethod.Get, "api/customers", withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UpdateCustomer(UpdateCustomerRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Patch, "api/customers/me", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UpdateCustomerToShop(UpdateCustomerToShopRequest requestObject)
        {
            var request = CreateRequest(HttpMethod.Patch, "api/customers/me/updatetoshop", requestObject, withUserContext: true);
            return await _httpClient.SendAsync(request);
        }
    }
}
