using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(AppUser user, int expireMinutes = 30);
        bool ValidateToken(string token, out Exception? validateErrorException);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private string issuer = "";
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        public JwtService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _config = config;
            this.issuer = _config.GetValue<string>("JwtSettings:Issuer") ?? "";
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JwtSettings:SignKey") ?? ""));
        }

        public async Task<string> GenerateTokenAsync(AppUser user, int expireMinutes = 30)
        {
            var claims = new List<Claim>();
            // Token 的主體內容
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            // JWT ID
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "The Audience"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));

            // // 加入自訂權限
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            // claims.Add(new Claim("roles", roleName));

            var userClaimsIdentity = new ClaimsIdentity(claims);

            // Create SecurityTokenDescriptor            
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                //Audience = issuer, 
                // 由於你的 API 受眾通常沒有區分特別對象，因此通常不太需要設定，也不太需要驗證
                //NotBefore = DateTime.Now, 預設值就是 DateTime.Now
                //IssuedAt = DateTime.Now, 預設值就是 DateTime.Now
                Subject = userClaimsIdentity,
                Expires = DateTime.Now.AddMinutes(expireMinutes),
                SigningCredentials = creds,
                NotBefore = DateTime.UtcNow,
            };

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(tokenDescriptor);
            var token = handler.WriteToken(securityToken);

            return token;
        }

        public bool ValidateToken(string token, out Exception? validateErrorException)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = this.issuer,
                IssuerSigningKey = _key,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var isAuthor = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                validateErrorException = null;
                return true;
            }
            catch (Exception ex)
            {
                validateErrorException = ex;
                return false;
            }
        }
    }
}