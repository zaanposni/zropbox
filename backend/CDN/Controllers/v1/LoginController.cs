﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CDN.Models;
using CDN.Repositories;

namespace CDN.Controllers
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
            var currentUser = HttpContext.User;
            return Ok(new string[] {
                "value1",
                currentUser.Identity.Name,
                await GetCurrentUsername()
            });
        }

        [HttpGet("admin")]
        [Authorize]
        public async Task<ActionResult> Admin()
        {
            await ValidateLogin(true);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserLogin login)
        {
            return Ok(new UserView(await UserRepository.CreateDefault(ServiceProvider).RegisterUser(login.Username, login.Password, false)));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            IActionResult response = Unauthorized();

            if (await UserRepository.CreateDefault(ServiceProvider).CheckLogin(login.Username, login.Password))
            {
                var tokenString = GenerateJSONWebToken(login.Username);
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
              expires: DateTime.Now.AddHours(6),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

