using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPI.BLL.Contracts;
using WebAPI.BLL.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace WebAPI.BLL
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptionsMonitor<UsersBLLOptions> _options;

        public JwtTokenService(IOptionsMonitor<UsersBLLOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string GenerateToken(int id, int expireMinutes = 20)
        {
            var nowDT = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.CurrentValue.JwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = nowDT,
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.SerialNumber, id.ToString())
                        }),

                Expires = nowDT.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var finalToken = tokenHandler.WriteToken(token);

            return finalToken;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.CurrentValue.JwtSecretKey))
            };

            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Encoding.UTF8.GetBytes(_options.CurrentValue.JwtSecretKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}
