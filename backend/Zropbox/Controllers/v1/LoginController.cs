using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zropbox.Exceptions;
using Zropbox.Models;
using Zropbox.Repositories;

namespace Zropbox.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : SimpleController<LoginController>
    {
        public LoginController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            await ValidateLogin();
            return Ok(new UserView(await GetCurrentUser()));
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserRegister dto)
        {
            await ValidateLogin();
            User currentUser = await GetCurrentUser();

            if (currentUser.Name != dto.Username)
            {
                throw new UnauthorizedException();
            }

            UserRepository repo = UserRepository.CreateDefault(ServiceProvider);

            User user = await repo.GetUser(dto.Username);

            return Ok(new UserView(await repo.UpdatePassword(dto.Username, dto.Password)));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            IActionResult response = Unauthorized();

            UserRepository repo = UserRepository.CreateDefault(ServiceProvider);

            if (await repo.CheckLogin(login.Username, login.Password))
            {
                User user = await repo.GetUser(login.Username);
                var tokenString = GenerateJSONWebToken(user.Name);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.GetJwtKey()));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken("cdn",
              "cdn",
              claims,
              expires: DateTime.Now.AddHours(Config.GetLoginDurationHours()),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

