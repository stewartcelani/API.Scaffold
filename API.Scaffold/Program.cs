using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Serilog;
using Serilog.Debugging;
using API.Scaffold.Handlers;
using API.Scaffold.Middleware;
using API.Scaffold.Pipelines;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                      throw new NullReferenceException("ASPNETCORE_ENVIRONMENT is null");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
SelfLog.Enable(Console.Error);
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(Log.Logger);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediatR(options => { options.RegisterServicesFromAssemblyContaining(typeof(Program)); })
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))     
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))   
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "ApiKey";
        options.DefaultChallengeScheme = "ApiKey";
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("Architecture: {Architecture}", IntPtr.Size == 4 ? "x86" : "x64");
Log.Information("Environment: {Environment}", environmentName);

app.Run();

