using Microsoft.EntityFrameworkCore;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.SyncDataServices.Grpc;
using UserService.SyncDataServices.Http;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IKiteschoolRepo, KiteschoolRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IKiteschoolDataClient, HttpKiteschoolDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service API", Version = "v1" });
});

// Configure the database based on the environment
if (builder.Environment.IsDevelopment())
{
    var connectionString = builder.Configuration.GetConnectionString("DevelopmentDB");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
    Console.WriteLine("--> Using Development settings");
}
else if (builder.Environment.IsProduction())
{
    var connectionString = builder.Configuration.GetConnectionString("ProductionDB");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
    Console.WriteLine("--> Using Production settings");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API V1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<GrpcUserService>();
app.MapGet("/protos/kiteschools.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/kiteschools.proto"));
});

// Endpoint for the KiteschoolService
Console.WriteLine($"--> KiteschoolService Endpoint: {builder.Configuration["KiteschoolService"]}");
Console.WriteLine($"--> RabbitMQHost value: {builder.Configuration["RabbitMQHost"]}");

// Generate some data for the in-memory database (only in Development)
PrepDb.PrepPopulation(app, app.Environment.IsProduction() || app.Environment.IsDevelopment());

app.Run();
