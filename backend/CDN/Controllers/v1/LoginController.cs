using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CDN.Models;

namespace CDN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var currentUser = HttpContext.User;
            return new string[] { "value1", "value2", "value3", "value4", "value5",
                currentUser.Identity.Name,
            };
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin login)
        {
            IActionResult response = Unauthorized();

            if (AuthenticateUser(login))
            {
                var tokenString = GenerateJSONWebToken();
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes());
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Name, "zaanposni"),
                new Claim(JwtRegisteredClaimNames.UniqueName, "zaanposni"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken("cdn",
              "cdn",
              claims,
              expires: DateTime.Now.AddHours(6),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool AuthenticateUser(UserLogin login)
        {
            //Validate the User Credentials
            //Demo Purpose, I have Passed HardCoded User Information
            return login.Username == "Jignesh";
        }
    }
}

