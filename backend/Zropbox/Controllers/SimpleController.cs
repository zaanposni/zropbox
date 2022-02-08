using Zropbox.Exceptions;
using Zropbox.Models;
using Zropbox.Repositories;
using Zropbox.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Zropbox.Controllers
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

        protected async Task ValidateLogin(bool requireAdmin = false)
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

        protected async Task<string> GetCurrentUsername()
        {
            return (await GetCurrentUser()).Name;
        }

        protected async Task<User> GetCurrentUser()
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
                if (user == null)
                {
                    throw new UnauthorizedException();
                }
                return user;
            }
            catch (Exception)
            {
                throw new UnauthorizedException();
            }
        }
    }
}
