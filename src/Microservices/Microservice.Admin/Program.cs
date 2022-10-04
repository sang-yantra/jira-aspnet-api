using Admin;
using Infrastructure.Jira.Supabase;
using Microservice.Admin.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var env = builder.Environment;

var config = builder.Configuration;
var services = builder.Services;


config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
config.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

/// logging

/// services
services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpContextAccessor();
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        var allowedOrigins = config.GetSection("AllowedOrigins").Get<string[]>();
        builder.WithOrigins(allowedOrigins)
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        ;
    });
});

services.AddDbContext<JiraDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("JiraSupabaseDb"));
});

services.AddMemoryCache();


services.AddJiraInfrastructure(config);
services.AddAdminServices();



// configure middlewares
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseCors();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

// running the app
app.Run();
