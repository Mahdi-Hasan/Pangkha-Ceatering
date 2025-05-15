using MealManagement.Application.Common.Interfaces;
using MealManagement.Application.Services;
using MealManagement.Domain.Repositories;
using MealManagement.Infrastructure.Authentication;
using MealManagement.Infrastructure.Firebase;
using MealManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MealManagement.Infrastructure.Persistence.Repositories;
using MealManagement.Infrastructure.Repositories;

namespace MealManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOracle(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMealRepository, MealRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            // Add JWT authentication
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

         
            // Add Firebase services
            services.AddSingleton<Firebase.IFirebaseOtpService, FirebaseOtpService>();
            services.AddSingleton<Firebase.IFirebaseNotificationService, FirebaseNotificationService>();
         

            return services;
        }
    }
}