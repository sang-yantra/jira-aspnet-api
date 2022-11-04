using Admin;
using Authentication;
using Chats;
using FluentValidation.AspNetCore;
using Infrastructure.Jira.Supabase;
using Microservice.Admin.Persistence;
using Microservices.TasksManagement.Filters;
using Microservices.TasksManagement.Middlewares;
using Microservices.TasksManagement.Sockets;
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

/// <summary>
/// Api versioning services
/// </summary>
services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

/// <summary>
/// controllers services
/// </summary>
services.AddControllers(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>());


/// <summary>
/// fluent validation services
/// </summary>
services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

services.AddSwaggerGen();
services.AddHttpContextAccessor();

/// <summary>
/// CORS services
/// </summary>
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




/// <summary>
/// setting dbcontext
/// </summary>
services.AddDbContext<JiraDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("JiraSupabaseDb"));
});
services.AddMemoryCache();
services.AddJiraInfrastructure(config);
services.AddAdminServices();
services.AddTasksServices();
services.AddChatsServices(config);
services.AddAuthenticationServices(config);
services.AddTransient<ConnectionManager>();
services.AddSingleton<ChatHandler>();



// configure middlewares
var app = builder.Build();

//var webSocketOptions = new WebSocketOptions()
//{
//    KeepAliveInterval = TimeSpan.FromSeconds(120),
//    ReceiveBufferSize = 4 * 1024
//};

//app.UseWebSockets(webSocketOptions);
//var chatHandler = app.Services.GetService<ChatHandler>();
//app.UseMiddleware<WebSocketMiddleware>(chatHandler);

app.UseCors();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

// running the app
app.Run();
