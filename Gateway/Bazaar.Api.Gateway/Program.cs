using Bazaar.Api.Gateway.Clients;
using Bazaar.Shared.Infrastructure;
using Bazaar.Shared.Infrastructure.Auth;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
