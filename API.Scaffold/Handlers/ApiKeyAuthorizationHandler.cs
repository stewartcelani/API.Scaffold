using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API.Scaffold.Handlers;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    private readonly ILogger<ApiKeyAuthenticationHandler> _logger = logger.CreateLogger<ApiKeyAuthenticationHandler>();

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-API-Key", out var apiKey))
        {
            _logger.LogWarning("Authentication failed - Missing API Key. IP: {IpAddress}, Path: {Path}", 
                Request.HttpContext.Connection.RemoteIpAddress,
                Request.Path);
                
            return Task.FromResult(AuthenticateResult.Fail("API Key is missing"));
        }

        var validApiKeys = configuration.GetSection("ApiKeys").Get<string[]>();
        
        if (validApiKeys is null || validApiKeys.Length == 0)
        {
            _logger.LogError("Authentication failed - No API Keys configured in settings. IP: {IpAddress}, Path: {Path}",
                Request.HttpContext.Connection.RemoteIpAddress,
                Request.Path);
                
            return Task.FromResult(AuthenticateResult.Fail("No API Keys configured"));
        }
        
        if (!validApiKeys.Contains(apiKey.ToString()))
        {
            _logger.LogWarning("Authentication failed - Invalid API Key: {ApiKey}. IP: {IpAddress}, Path: {Path}",
                apiKey.ToString()[..6] + "...", // Only log first 6 chars of key for security
                Request.HttpContext.Connection.RemoteIpAddress,
                Request.Path);
                
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, "API User") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 401;
        Response.Headers.Append("WWW-Authenticate", "ApiKey");
        
        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Title = "Unauthorized",
            Status = 401,
            Detail = "API key is missing or invalid.",
            Instance = Request.Path
        };

        await Response.WriteAsJsonAsync(problem);
    }
}