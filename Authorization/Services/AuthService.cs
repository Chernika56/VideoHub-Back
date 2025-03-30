using BackEnd.Authorization.DTO;
using BackEnd.DB.Context;
using BackEnd.DB.Entities;
using BackEnd.Users.DTO.ResponseDTO;
using BackEnd.Users.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BackEnd.Authorization.Services
{
    public class AuthService(ILogger<UserService> logger, MyDbContext db, IConfiguration config, AuthHelper authHelper)
    {
        public async Task<string?> AuthenticationUser(AuthenticationDTO dto)
        {
            try
            {
                var passwordHash = CreateHashCode(dto.Password!);
                var user = await db.Users.FirstOrDefaultAsync(u => u.Password == passwordHash && u.Login == dto.Login);
                if (user is null) return null;
                logger.LogInformation("User {user} was authenticated", user.Name);
                user.IsLoggedIn = true;
                await db.SaveChangesAsync();
                return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
            }
            catch (Exception ex)
            {
                logger.LogError("Error has occured in user Authentication: {exception}", ex);
                return null;
            }
        }

        public async Task<WhoAmIDTO?> WhoAmI()
        {
            try
            {
                var user = await db.Users
                    .Include(u => u.M2mUsersOrganizations)
                    .Include(u => u.M2mUsersFolders)
                    .Where(u => u.Login == authHelper.GetCurrentUserLogin())
                    .Select(u => new WhoAmIDTO
                    {
                        Id = u.Id,
                        Login = u.Login,
                        Name = u.Name,
                        Email = u.Email,
                        Phone = u.Phone,
                        Note = u.Note,
                        MaxSessions = u.MaxSessions,
                        Disabled = u.Disabled,
                        AccessLevel = u.AccessLevel,
                        IsLoggedIn = u.IsLoggedIn,
                        Organizations = u.M2mUsersOrganizations != null ? u.M2mUsersOrganizations.Select(uo => new Users.DTO.ResponseDTO.Organizations
                        {
                            Id = uo.OrganizationId,
                            // Title = db.Organizations.Find(uo.OrganizationId).Title,
                            IsMember = uo.IsMember,
                            IsAdmin = uo.IsAdmin,
                        }).ToList() : null,
                        Folders = u.M2mUsersFolders != null ? u.M2mUsersFolders.Select(uf => new Users.DTO.ResponseDTO.Folders
                        {
                            Id = uf.FolderId,
                            CanView = uf.CanView,
                        }).ToList() : null,
                    }).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                logger.LogError("Exceptions occured during organization finding {exception}", ex);
                return null;
            }
        }

        private JwtSecurityToken CreateToken(UsersEntity user)
        {
            IEnumerable<Claim> claims =
                [
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.AccessLevel),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                ];
            return new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                issuer: config["JwtParameters:Issuer"],
                audience: config["JwtParameters:Audience"],
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtParameters:Key"]!)),
                    SecurityAlgorithms.HmacSha256)
            );
        }

        public string CreateHashCode(string input)
        {
            string hash = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                hash = builder.ToString();
            }
            return hash;
        }
    }
}
