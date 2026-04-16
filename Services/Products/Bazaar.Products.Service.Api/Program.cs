using Bazaar.Products.Service.Commands;
using Bazaar.Products.Service.DataAccess;
using Bazaar.Products.Service.Queries;
using Bazaar.Shared.Infrastructure;
using Bazaar.Shared.Infrastructure.Auth;
using Prometheus;
using Prometheus.DotNetRuntime;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
