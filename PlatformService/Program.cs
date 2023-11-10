using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

// Add configuration sources
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Production.json", optional: true)
    .AddEnvironmentVariables();

// Configure the database based on the environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
    Console.WriteLine("--> Using Development settings");
}
else if (builder.Environment.IsProduction())
{
    var connectionString = builder.Configuration.GetConnectionString("PlatformDBConnection");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
    Console.WriteLine("--> Using Production settings");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

// Endpoint for the CommandService
Console.WriteLine($"--> CommandService Endpoint: {builder.Configuration["CommandService"]}");
Console.WriteLine($"--> appsettings.Production.json RabbitMQHost value: {builder.Configuration["RabbitMQHost"]}");
Console.WriteLine($"--> appsettings.Production.json ConnectionString value: {builder.Configuration["PlatformDBConnection"]}");

// Generate some data for the in-memory database (only in Development: IsProduction = false)
PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
