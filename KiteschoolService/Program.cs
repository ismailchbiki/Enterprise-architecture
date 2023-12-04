using KiteschoolService.Data;
using KiteschoolService.AsyncDataServices;
using KiteschoolService.EventProcessing;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configure the database based on the environment
var environment = builder.Environment;
ConfigureDatabase(builder.Services, environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kiteschool Service API", Version = "v1" });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IKiteschoolRepo, KiteschoolRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kiteschool Service API V1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Generate some data for the in-memory database
PrepDb.PrepPopulation(app);

app.Run();

// Method to configure the database
void ConfigureDatabase(IServiceCollection services, IHostEnvironment environment)
{
    var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

    string connectionStringName = environment.IsDevelopment() ? "DevelopmentDB" : "ProductionDB";
    string connectionString = configuration.GetConnectionString(connectionStringName);

    services.AddSingleton<IMongoDatabase>(provider =>
    {
        var mongoClient = new MongoClient(connectionString);
        var databaseName = "KiteschoolDB";
        return mongoClient.GetDatabase(databaseName);
    });

    Console.WriteLine($"--> Using {environment.EnvironmentName} settings");
}
