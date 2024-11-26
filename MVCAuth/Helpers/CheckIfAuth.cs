using MVCAuth.Middleware;

namespace MVCAuth.Helpers;

public static class CheckIfAuth
{
    public static IApplicationBuilder UseCheckIfAuth(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AddTokenMiddleware>();
    }
}