using Microsoft.EntityFrameworkCore;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.SyncDataServices.Http;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IKiteschoolDataClient, HttpKiteschoolDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
// builder.Services.AddAuthentication(x =>
//     {
//         x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//     }).AddJwtBearer(x =>
//     {
//         x.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//             ValidAudience = builder.Configuration["JwtSettings:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey
//             (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true
//         };
//     }
// );

// builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService API", Version = "v1" });
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

// Add CORS configuration
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigins",
//         builder =>
//         {
//             builder.WithOrigins("http://client_web.com", "https://client_mobile.com")
//                    .AllowAnyHeader()
//                    .AllowAnyMethod();
//         });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger-user/v1/swagger.json", "UserService API V1");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// app.Map("/api", app =>
// {
//     Apply CORS policy to "/api" path
//     app.UseCors("AllowSpecificOrigins");

//     app.UseAuthorization();

//     app.MapControllers();
// });

// Endpoint for the KiteschoolService
Console.WriteLine($"--> KiteschoolService Endpoint: {builder.Configuration["KiteschoolService"]}");
Console.WriteLine($"--> RabbitMQHost value: {builder.Configuration["RabbitMQHost"]}");

// Generate some data for the in-memory database (only in Development)
// PrepDb.PrepPopulation(app, app.Environment.IsProduction() || app.Environment.IsDevelopment());

app.Run();
