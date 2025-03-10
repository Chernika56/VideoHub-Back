﻿using BackEnd.DataBase.Context;
using BackEnd.DataBase.Entities;
using BackEnd.Users.DTO;
using BackEnd.Users.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackEnd.Users.Services
{
    public class UserService(ILogger<UserService> logger, VideoHubDbContext db, IConfiguration config)
    {
        public async Task<string?> AuthenticationUser(AuthenticationDTO dto)
        {
            try
            {
                var passwordHash = CreateHashCode(dto.Password!);
                var user = await UserStorage.GetUser(dto.Login, passwordHash);
                if (user is null) return null;
                logger.LogInformation("User {user} was authenticated", user.Login);
                return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
            }
            catch (Exception ex)
            {
                logger.LogError("Error has occured in user Authentication: {exception}", ex);
                return null;
            }
        }

        //public async Task<string?> AuthenticationUser(AuthenticationDTO dto)
        //{
        //    try
        //    {
        //        var passwordHash = CreateHashCode(dto.Password!);
        //        var user = await db.Users.FirstOrDefaultAsync(u => u.UsrPassword == passwordHash && u.UsrLogin == dto.Login);
        //        if (user is null) return null;
        //        logger.LogInformation("User {user} was authenticated", user.UsrName);
        //        return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("Error has occured in user Authentication: {exception}", ex);
        //        return null;
        //    }
        //}

        public async Task<string?> RegisterUser(RegistrationDTO dto)
        {
            try
            {
                var passwordHash = CreateHashCode(dto.Password);
                var user = await UserStorage.CreateUser(dto.Login, passwordHash);

                logger.LogInformation("User {user} was registered", user.Login);

                return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
            }
            catch (Exception ex)
            {
                logger.LogError("Error has occured while user registration: {exception}", ex);
                return null;
            }
        }

        //public async Task<string?> RegisterUser(RegistrationDTO dto)
        //{
        //    try
        //    {
        //        var passwordHash = CreateHashCode(dto.Password);
        //        var user = new User { UsrPassword = passwordHash, UsrRole = "User", UsrLogin = dto.Login };
        //        db.Users.Add(user);
        //        int numberOfAdded = await db.SaveChangesAsync();
        //        if (numberOfAdded == 0) return null;
        //        logger.LogInformation("User {user} was registered", user.UsrName);

        //        return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError("Error has occured while user registration: {exception}", ex);
        //        return null;
        //    }
        //}

        private JwtSecurityToken CreateToken(DataBase.Entities.User user)
        {
            IEnumerable<Claim> claims =
                [
                    new Claim(ClaimTypes.Name, user.UsrLogin),
                    new Claim(ClaimTypes.Role, user.UsrRole),
                    new Claim(ClaimTypes.NameIdentifier, user.UsrId.ToString())
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

        private JwtSecurityToken CreateToken(Utils.User user)
        {
            IEnumerable<Claim> claims =
                [
                    new Claim(ClaimTypes.Name, user.Login)
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

        private string CreateHashCode(string input)
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
