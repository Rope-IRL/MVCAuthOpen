using System.Net;
using System.Security.Claims;

namespace MVCAuth.Services.Jwt;

public class CheckSign(IHttpContextAccessor httpContextAccessor)
{
    public string IsSignedIn()
    {
        string user = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value.ToString();
        return user;
    }
    
    public string GetName()
    {
        string user = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value.ToString();
        return user;
    }
}