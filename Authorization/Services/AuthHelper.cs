using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BackEnd.Authorization.Services
{

    public class AuthHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public AuthHelper(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public string? GetCurrentUserLogin()
        {
            if (_httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("jwtToken", out var token) == true)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                return jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            }
            return null;
        }

        public string? GetCurrentUserRole()
        {
            if (_httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("jwtToken", out var token) == true)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                return jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            }
            return null;
        }
    }

}
