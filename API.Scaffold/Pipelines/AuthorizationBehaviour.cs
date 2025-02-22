using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace API.Scaffold.Pipelines;

public class AuthorizationBehaviour<TRequest, TResponse>(
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthorizationBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes(typeof(AuthorizeAttribute), true);
        if (authorizeAttributes.Length == 0) return await next();

        // Check if the user is authenticated
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            logger.LogWarning("Unauthorized access attempt for request {RequestType}", typeof(TRequest).Name);
            throw new UnauthorizedAccessException("Authentication required");
        }

        return await next();
    }
}