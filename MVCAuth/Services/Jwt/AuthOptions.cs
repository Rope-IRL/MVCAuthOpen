using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace MVCAuth.Services.Jwt
{
    public class AuthOptions
    {
        public const string ISSUER = "Publisher";
        public const string AUDIENCE = "Org";
        private const string key = "somesecretkeyveryverysecretasdfasdsdf";
        public const int LIFETIME = 300;

        public static SymmetricSecurityKey GetSymmetricKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}
