using Microsoft.IdentityModel.Tokens;

namespace MVCAuth.ModelViews;

public class AdminModelView : ICanHaveToken
{
    public string? Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; private set; }

    public AdminModelView()
    {
        Role = "Admin";
    }
    
    public string NameForToken()
    {
        return Name.IsNullOrEmpty() ? Login : Name;
    }

    public string RoleForToken()
    {
        return Role;
    }
}