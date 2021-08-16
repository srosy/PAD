using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PAD.Data
{
    public class JWT_Helper
    {
        private readonly string _tokenIssuer;
        private readonly string _secret;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _user;

        public JWT_Helper(IConfiguration configuration, UserManager<IdentityUser> user)
        {
            _config = configuration;
            _tokenIssuer = _config.GetValue<string>("JWT_ISSUER");
            _secret = _config.GetValue<string>("JWT_SECRET");
            _user = user;
        }

        /// <summary>
        /// Validates the token was issued by PAD, was signed with the correct key and is not expired 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>bool</returns>
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validParams = new TokenValidationParameters();

            validParams.ValidateIssuerSigningKey = true; //token must be signed with api key
            validParams.ValidateLifetime = true; //token should NOT be expired
            validParams.ValidateIssuer = true; //token must be issued by the Client api
            validParams.ValidateAudience = false;//token audience should NOT be checked
            validParams.ValidIssuer = _tokenIssuer;
            validParams.IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)); ;
            validParams.ClockSkew = TimeSpan.Zero; //set clock skew to accurately validate token expiration dates

            try
            {
                tokenHandler.ValidateToken(token, validParams, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Gets the JWT Authentication object for a user. Used with Client and Admin apis.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<object> Authenticate(IdentityUser user)
        {
            var roles = await _user.GetRolesAsync(user);
            var claims = roles != null && roles.Any() ? roles.Select(r => new Claim("role", r)).ToList() : new List<Claim>();

            return new
            {
                username = $"{user.UserName}",
                email = $"{user.Email}",
                jwt = new JwtSecurityTokenHandler().WriteToken(GenerateToken(claims)),
                jwt_expire = $"{DateTime.Now.AddMinutes(5)}"
            };
        }

        /// <summary>
        /// Generates a JWT token.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private JwtSecurityToken GenerateToken(List<Claim> claims)
        {
            var token = new JwtSecurityToken(
                _tokenIssuer,
                _tokenIssuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                    SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}
