using Admin;
using FluentValidation.AspNetCore;
using Infrastructure.Jira.Supabase;
using Microservice.Admin.Persistence;
using Microservices.TasksManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasks;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;

var config = builder.Configuration;
var services = builder.Services;
// Add services to the container.

config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
config.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});
services.AddControllers(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>());

services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    ;

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
services.AddTasksServices();

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
