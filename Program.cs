using EventDriven.EventBus.Abstractions;
using subscriber.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<MessageCreatedEventHandler>();

ConfigurationManager configuration = builder.Configuration;

//builder.Services.AddLogging();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add Dapr Event Bus
builder.Services.AddDaprEventBus(configuration);

// Add Dapr Mongo event cache
builder.Services.AddDaprMongoEventCache(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseCloudEvents();

var messageCreatedEventHandler = app.Services.GetService<MessageCreatedEventHandler>();

app.UseEndpoints(endpoints =>
{
    // Map SubscribeHandler and DapEventBus
    endpoints.MapSubscribeHandler();
    endpoints.MapDaprEventBus(eventBus =>
    {
        // Subscribe with a handler
        eventBus.Subscribe(messageCreatedEventHandler, "test-eventbus");
    });
});

app.Run();

public record MessageCreatedEvent(string message) : IntegrationEvent;
