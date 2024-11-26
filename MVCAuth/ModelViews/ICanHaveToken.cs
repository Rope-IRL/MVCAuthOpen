namespace MVCAuth.ModelViews;

public interface ICanHaveToken
{
   public string NameForToken();
   public string RoleForToken();
}