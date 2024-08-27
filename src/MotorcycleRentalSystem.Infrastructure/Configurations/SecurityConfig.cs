using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Settings;
using MotorcycleRentalSystem.Infrastructure.Settings;
using System.Security.Claims;
using System.Text;

namespace MotorcycleRentalSystem.Infrastructure.Configurations
{
    public static class SecurityConfig
    {
        public static IServiceCollection AddSecurityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            var jwt = configuration.GetSection("jwt").Get<JwtSettings>() ?? throw new ArgumentNullException(nameof(configuration));

            services.AddScoped<IJwtSettings>(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
                    }; options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine($"Token validated for {context.Principal.Identity.Name}");
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            Console.WriteLine($"Token received: {context.Token}");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("DeliveryOnly", policy => policy.RequireClaim(ClaimTypes.Role, "DeliveryDriver"));
            });

            return services;
        }
    }
}
