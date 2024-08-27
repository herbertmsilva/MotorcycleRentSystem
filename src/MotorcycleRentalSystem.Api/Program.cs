using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MotorcycleRentalSystem.Api.Configurations;
using MotorcycleRentalSystem.Application.Configuration;
using MotorcycleRentalSystem.Infrastructure.Configurations;
using MotorcycleRentalSystem.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddSwaggerConfig(builder.Configuration);


builder.Services.AddPostgresConfiguration(builder.Configuration);
builder.Services.AddMongoDbConfiguration(builder.Configuration);
builder.Services.AddRabbitMqConfiguration(builder.Configuration);
builder.Services.AddDIConfiguration(builder.Configuration);
builder.Services.AddHelthCheckConfig(builder.Configuration);
builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddSecurityConfiguration(builder.Configuration);
builder.Services.AddFluentValidationConfig();
builder.Services.AddAutomapperConfig();
builder.Services.AddMediatrConfig();

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");


app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseMiddleware<SuccessResponseMiddleware>();
app.UseMiddleware<ErrorResponseMiddleware>();

app.UseRouting();
app.MapControllers();


app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }