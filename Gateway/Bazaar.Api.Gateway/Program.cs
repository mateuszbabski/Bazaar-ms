using Bazaar.Api.Gateway.Clients;
using Bazaar.Shared.Infrastructure;
using Bazaar.Shared.Infrastructure.Auth;
using Prometheus;
using Prometheus.DotNetRuntime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterSharedServices(builder.Configuration);
builder.Services.RegisterGatewayAuth(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CustomersServiceClient>();
builder.Services.AddScoped<ProductsServiceClient>();
builder.Services.AddScoped<BasketsServiceClient>();
builder.Services.AddScoped<OrdersServiceClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpMetrics();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

DotNetRuntimeStatsBuilder
    .Customize()
    .WithGcStats()
    .WithThreadPoolStats()
    .WithExceptionStats()
    .StartCollecting();

app.MapMetrics();

app.Run();
