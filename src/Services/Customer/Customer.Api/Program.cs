using Customer.Application.Commands.CreateCustomer;
using Customer.Infrastructure;
using Microsoft.OpenApi.Models;
using Telecube.BuildingBlocks.Messaging;
using Telecube.BuildingBlocks.Web;
using Telecube.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntraAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddDefaultProblemDetails();
builder.Services.AddMessaging(builder.Configuration, cfg =>
{
    cfg.Message<CustomerUpdated>(x => x.SetEntityName("customer-updated"));
});
builder.Services.AddCustomerInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Entra External ID endpoints for this tenant, and the scope our API requires.
var authority = $"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0";
var apiScope = $"api://{builder.Configuration["AzureAd:ClientId"]}/Customer.ReadWrite";

builder.Services.AddSwaggerGen(options =>
{
    // Describes the login method: authorization code flow against our tenant, asking for our API's scope.
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{authority}/authorize"),
                TokenUrl = new Uri($"{authority}/token"),
                Scopes = new Dictionary<string, string> { [apiScope] = "Read and write the signed-in customer's data" }
            }
        }
    });

    // Tells Swagger to attach the token (as a Bearer header) to every request.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } }] = [apiScope]
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Swagger acts as the client app registration; PKCE is required for SPA-type registrations.
        options.OAuthClientId(builder.Configuration["Swagger:ClientId"]);
        options.OAuthUsePkce();
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
