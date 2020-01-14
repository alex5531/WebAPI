using System.Security.Claims;

namespace WebAPI.BLL.Contracts
{
    public interface IJwtTokenService
    {
        string GenerateToken(int Id, int expireMinutes);
        bool ValidateToken(string token);

        public ClaimsPrincipal GetPrincipal(string token);
    }
}
