using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
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

// Endpoint for the CommandService
Console.WriteLine($"--> CommandService Endpoint: {builder.Configuration["CommandService"]}");

// Generate some data for the in-memory database
PrepDb.PrepPopulation(app);

app.Run();
