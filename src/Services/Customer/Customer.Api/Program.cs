var builder = WebApplication.CreateBuilder(args);

// TODO (Step 6): AddEntraAuthentication, AddDefaultProblemDetails, AddMessaging, health checks,
// repository/MediatR registration, controllers.

var app = builder.Build();

app.Run();
