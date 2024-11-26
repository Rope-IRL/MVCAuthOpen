using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCAuth.ModelViews;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MVCAuth.Models.Users;

namespace MVCAuth.Services.Jwt
{
    public class GiveToken
    {
        public async Task<string> Token(ICanHaveToken user)
        {
            var identity = GetIdentity(user);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt.ToString();

        }

        private ClaimsIdentity GetIdentity(ICanHaveToken user)
        {
            if (user != null)
            {
                var ci = new ClaimsIdentity();
                ci.AddClaim(new Claim(ClaimTypes.Role, user.RoleForToken()));
                ci.AddClaim(new Claim(ClaimTypes.Name, user.NameForToken()));
                
                return ci;

            }
            return null;

        }
        
      
    }
}
