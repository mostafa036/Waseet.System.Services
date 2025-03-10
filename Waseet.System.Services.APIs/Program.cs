using Microsoft.AspNetCore.Mvc;
using ShopSphere.Services.API.Extensions;
using System.Text.Json.Serialization;
using Waseet.System.Services.APIs.Extensions;
using Waseet.System.Services.APIs.Middlewares;
using Waseet.System.Services.Application.Resolving;
using Waseet.System.Services.Persistence.Errors;

namespace Waseet.System.Services.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureDatabases(builder.Configuration);
            builder.Services.AddApplicationServicse(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddHttpClient();

            // Register AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Register CategoryPictureResolver
            builder.Services.AddTransient<CategoryPictureResolver>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
                    var errorResponse = new ApiValidationErrorResponse { Errors = errors };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            var app = builder.Build();

            await app.ApplyMigrationsAndSeedDataAsync();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseStaticFiles();   

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Ensure this is before UseAuthorization()

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}