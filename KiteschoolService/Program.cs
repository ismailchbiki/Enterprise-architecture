using KiteschoolService.Data;
using KiteschoolService.AsyncDataServices;
using KiteschoolService.EventProcessing;
using KiteschoolService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IKiteschoolRepo, KiteschoolRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
// builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddScoped<IKiteschoolDataClient, KiteschoolDataClient>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kiteschool Service API", Version = "v1" });
});

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
// PrepDb.PrepPopulation(app);

app.Run();