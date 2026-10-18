using MassTransit;
using Search.Api.Consumers;
using Search.Application.Commands.IndexProduct;
using Search.Infrastructure;
using Telecube.BuildingBlocks.Messaging;
using Telecube.BuildingBlocks.Web;
using Telecube.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultProblemDetails();
builder.Services.AddMessaging(
    builder.Configuration,
    consumers =>
    {
        consumers.AddConsumer<ProductCreatedConsumer>();
        consumers.AddConsumer<ProductUpdatedConsumer>();
        consumers.AddConsumer<ProductDeletedConsumer>();
    },
    (context, cfg) =>
    {
        // Same topic names Catalog publishes to.
        cfg.Message<ProductCreated>(x => x.SetEntityName("product-created"));
        cfg.Message<ProductUpdated>(x => x.SetEntityName("product-updated"));
        cfg.Message<ProductDeleted>(x => x.SetEntityName("product-deleted"));

        // Search reads each topic through its own "search" subscription, so other services get their own copy.
        cfg.SubscriptionEndpoint<ProductCreated>("search", e => e.ConfigureConsumer<ProductCreatedConsumer>(context));
        cfg.SubscriptionEndpoint<ProductUpdated>("search", e => e.ConfigureConsumer<ProductUpdatedConsumer>(context));
        cfg.SubscriptionEndpoint<ProductDeleted>("search", e => e.ConfigureConsumer<ProductDeletedConsumer>(context));
    });
builder.Services.AddSearchInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IndexProductCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
