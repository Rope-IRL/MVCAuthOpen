using Microsoft.AspNetCore.Identity;

namespace MVCAuth.ModelViews
{
    public class UserModelView : ICanHaveToken
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public string? Role { get; private set; }

        public UserModelView()
        {
            Role = "User";
        }
        public override string ToString()
        {
            return $"Login is {Login} pass is {Password}";
        }

        public string NameForToken()
        {
            return Login;
        }

        public string RoleForToken()
        {
            return Role;
        }
    }
}
