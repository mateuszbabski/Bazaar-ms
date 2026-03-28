using Bazaar.Baskets.Service.Commands.Dtos;
using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Bazaar.Baskets.Service.Commands.Clients
{
    public class OrderServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceTokenManager _serviceTokenManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderServiceClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IServiceTokenManager serviceTokenManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _serviceTokenManager = serviceTokenManager;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["Services:OrdersService"]!);
        }

        public async Task<OrderPlacementResult> PlaceOrder(Basket basket)
        {
            var orderRequest = new CreateOrderRequest
            {
                CustomerId = basket.CustomerId,
                TotalPrice = basket.TotalPrice,
                Currency = basket.Currency,
                TotalWeightInKg = basket.TotalWeightInKg,
                Items = basket.Items.Select(i => new CreateOrderItemRequest
                {
                    ProductId = i.ProductId,
                    ShopId = i.ShopId,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Currency = i.Currency,
                    WeightInKg = i.WeightInKg
                }).ToList()
            };

            try
            {
                var request = CreateRequest(HttpMethod.Post, "api/orders", orderRequest);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return OrderPlacementResult.Error($"Order service returned {(int)response.StatusCode}.");

                return OrderPlacementResult.Success();
            }
            catch (HttpRequestException)
            {
                return OrderPlacementResult.ServiceUnavailable();
            }
            catch (TaskCanceledException)
            {
                return OrderPlacementResult.ServiceUnavailable();
            }
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url, object? content = null)
        {
            var request = new HttpRequestMessage(method, url);

            var serviceToken = _serviceTokenManager.GenerateServiceToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", serviceToken);

            var claims = _httpContextAccessor.HttpContext?.User;
            var userId = claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = claims?.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(userId))
                request.Headers.Add("X-User-Id", userId);
            if (!string.IsNullOrEmpty(userRole))
                request.Headers.Add("X-User-Role", userRole);

            if (content != null)
                request.Content = JsonContent.Create(content);

            return request;
        }
    }

    public class OrderPlacementResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public static OrderPlacementResult Success()
            => new() { IsSuccess = true };

        public static OrderPlacementResult ServiceUnavailable()
            => new() { IsSuccess = false, ErrorMessage = "Order service is unavailable." };

        public static OrderPlacementResult Error(string message)
            => new() { IsSuccess = false, ErrorMessage = message };
    }
}
