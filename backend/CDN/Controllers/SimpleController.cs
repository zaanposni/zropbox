using CDN.Exceptions;
using CDN.Models;
using CDN.Repositories;
using CDN.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CDN.Controllers
{
    public class SimpleController<T> : Controller
    {
        protected ILogger<T> Logger { get; set; }
        protected InternalConfiguration Config { get; set; }
        protected IServiceProvider ServiceProvider { get; set; }

        public SimpleController(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetRequiredService<ILogger<T>>();
            Config = serviceProvider.GetRequiredService<InternalConfiguration>();
            ServiceProvider = serviceProvider;
        }

        public async Task ValidateLogin(bool requireAdmin = false)
        {
            if (HttpContext.User == null || HttpContext.User.Identity == null)
            {
                throw new UnauthorizedException();
            }
            if (string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                throw new UnauthorizedException();
            }
            try
            {
                User user = await UserRepository.CreateDefault(ServiceProvider).GetUser(HttpContext.User.Identity.Name);
                if (requireAdmin && !user.IsAdmin)
                {
                    throw new UnauthorizedException();
                }
            }
            catch (Exception)
            {
                throw new UnauthorizedException();
            }
        }

        public async Task<string> GetCurrentUsername()
        {
            return (await GetCurrentUser()).Name;
        }

        public async Task<User> GetCurrentUser()
        {
            if (HttpContext.User == null || HttpContext.User.Identity == null)
            {
                throw new UnauthorizedException();
            }
            if (string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                throw new UnauthorizedException();
            }
            try
            {
                User user = await UserRepository.CreateDefault(ServiceProvider).GetUser(HttpContext.User.Identity.Name);
                return user;
            }
            catch (Exception)
            {
                throw new UnauthorizedException();
            }

        }
    }
}
