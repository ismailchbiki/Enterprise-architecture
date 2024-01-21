using KiteschoolService.Data;
using KiteschoolService.AsyncDataServices;
using KiteschoolService.EventProcessing;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Program
{
    private static void Main(string[] args)
    {
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
        builder.Services.AddSingleton<IMessageBusSubscriber, MessageBusSubscriber>();
        builder.Services.AddHostedService<MessageBusSubscriber>();
        builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            }
        );

        builder.Services.AddAuthorization();

        // builder.Services.AddCors(options =>
        // {
        //     options.AddPolicy("AllowSwagger",
        //         builder => builder.WithOrigins("https://allowed-origin.com")
        //                         .AllowAnyHeader()
        //                         .AllowAnyMethod());
        // });

        // Build the app
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kiteschool Service API V1");
        });

        // app.UseCors("AllowSwagger");

        app.UseHttpsRedirection();
        app.UseAuthentication();
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

            services.AddSingleton(provider =>
            {
                var mongoClient = new MongoClient(connectionString);
                var databaseName = "KiteschoolDB";
                return mongoClient.GetDatabase(databaseName);
            });

            Console.WriteLine($"--> Using {environment.EnvironmentName} settings");
        }
    }
}