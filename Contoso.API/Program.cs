using Contoso.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration);

builder
    .Build()
    .ConfigureApp()
    .Run();
