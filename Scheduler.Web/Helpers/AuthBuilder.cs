using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Scheduler.Core.Models;
using Microsoft.Extensions.Configuration;

namespace Scheduler.Web
{
    public static class AuthBuilder
    {
        // ========================= Build Claims Principle ==================
        
        // https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-5.0
        // https://andrewlock.net/introduction-to-authentication-with-asp-net-core/
                
        // return claims principal based on authenticated user
        public static ClaimsPrincipal BuildClaimsPrincipal(User user)
        { 
            // define user claims
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())                              
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            // build principal using claims
            return  new ClaimsPrincipal(claims);
        }
        
        // method to sign a users token 
        // public static User SignJwtToken(User user, string secret) {     
        //     // generate Jwt token
        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.ASCII.GetBytes(secret);
        //     var tokenDescriptor = new SecurityTokenDescriptor {
        //         Subject = new ClaimsIdentity(new Claim[]  {
        //             new Claim(ClaimTypes.Name, user.Id.ToString()),
        //             new Claim(ClaimTypes.Email, user.Email),
        //             new Claim(ClaimTypes.Role, user.Role.ToString())
        //         }),
        //         Expires = DateTime.UtcNow.AddDays(7),
        //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
        //                                             SecurityAlgorithms.HmacSha256Signature)
        //     };
        //     var token = tokenHandler.CreateToken(tokenDescriptor);
        //     // return user after adding token and removing password
        //     user.Token = tokenHandler.WriteToken(token);
        //     user.Password = null; // remove the password
        //     return user;
        // }

        // method to Build a JWT Token 
        public static string BuildJwtToken(User user, IConfiguration _configuration)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),                
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:JwtSecret"]));

            var token = new JwtSecurityToken(
                _configuration["JwtConfig:JwtIssuer"],
                _configuration["JwtConfig:JwtAudience"],
                claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtConfig:JwtExpiryInDays"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}