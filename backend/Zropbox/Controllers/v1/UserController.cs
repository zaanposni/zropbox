using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zropbox.Models;
using Zropbox.Repositories;

namespace Zropbox.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : SimpleController<UserController>
    {
        public UserController(IServiceProvider serviceProvider) : base(serviceProvider) { }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            await ValidateLogin(true);
            List<User> users = await UserRepository.CreateDefault(ServiceProvider).GetUsers();
            return Ok(users.Select(x => new UserView(x)));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister login)
        {
            await ValidateLogin(true);
            return Ok(new UserView(await UserRepository.CreateDefault(ServiceProvider).RegisterUser(login.Username, login.Password, login.IsAdmin)));
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete([FromRoute] string username)
        {
            await ValidateLogin(true);

            List<User> users = await UserRepository.CreateDefault(ServiceProvider).GetUsers();
            if (users.Count == 1)
            {
                return BadRequest();
            }

            await UserRepository.CreateDefault(ServiceProvider).DeleteUser(username);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Delete([FromBody] UserRegister dto)
        {
            await ValidateLogin(true);

            UserRepository repo = UserRepository.CreateDefault(ServiceProvider);

            await repo.UpdatePassword(dto.Username, dto.Password);
            await repo.UpdateAdminStatus(dto.Username, dto.IsAdmin);

            return Ok(new UserView(await repo.GetUser(dto.Username)));
        }
    }
}
