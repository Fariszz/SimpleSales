using Backend.Core;
using Backend.Data;
using Microsoft.Extensions.Options;

namespace Backend.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApplicationSettings _appSettings;
    
    public JwtMiddleware(RequestDelegate next, IOptions<ApplicationSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }
    
    public async Task Invoke(HttpContext context, ApplicationDbContext dataContext, IJwtRepository jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var accountId = jwtUtils.ValidateToken(token);
        if (accountId != null)
        {
            // attach account to context on successful jwt validation
            context.Items["Account"] = await dataContext.Users.FindAsync(accountId.Value);
        }

        await _next(context);
    }
}