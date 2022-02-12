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

            return Ok(new UserView(await UserRepository.CreateDefault(ServiceProvider).UpdatePassword(dto.Username, dto.Password)));
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
                response = Ok(new { token = GenerateJSONWebToken(user.Name) });
            }

            return response;
        }

        [Authorize]
        [HttpPost("impersonate/{targetUsername}")]
        public async Task<IActionResult> Impersonate([FromBody] UserLogin login, [FromRoute] string targetUsername)
        {
            await ValidateLogin(true);

            UserRepository repo = UserRepository.CreateDefault(ServiceProvider);

            if (await repo.CheckLogin(login.Username, login.Password))
            {
                User loggedInUser = await repo.GetUser(login.Username);
                User targetUser = await repo.GetUser(targetUsername);
                if (loggedInUser == null || !loggedInUser.IsAdmin)
                {
                    throw new UnauthorizedException();
                }
                return Ok(new { token = GenerateJSONWebToken(targetUser.Name) });
            }
            return Unauthorized();
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

            var token = new JwtSecurityToken("zropbox",
              "zropbox",
              claims,
              expires: DateTime.Now.AddHours(Config.GetLoginDurationHours()),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

