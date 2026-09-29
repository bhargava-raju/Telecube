using Customer.Infrastructure;
using Telecube.BuildingBlocks.Messaging;
using Telecube.BuildingBlocks.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddDefaultProblemDetails();
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddCustomerInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
