using Bazaar.Shared.Infrastructure;
using Bazaar.Shared.Infrastructure.Auth;
using Bazaar.Customers.Service.Commands;
using Bazaar.Customers.Service.Queries;
using Bazaar.Customers.Service.DataAccess;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.RegisterSharedServices(builder.Configuration);
builder.Services.RegisterAuthServices(builder.Configuration);
builder.Services.RegisterDataAccessServices(builder.Configuration);
builder.Services.RegisterCommandServices();
builder.Services.RegisterQueryServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
