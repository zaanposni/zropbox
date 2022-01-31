using CDN.Exceptions;
using CDN.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CDN.Repositories
{
    public class UserRepository : BaseRepository<UserRepository>
    {
        private UserRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public static UserRepository CreateDefault(IServiceProvider serviceProvider)
        {
            return new UserRepository(serviceProvider);
        }

        /// <summary>
        /// Gets the defined user. Checked in lower case.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">If the username is not token. Checked in lower case.</exception>
        public async Task<User> GetUser(string username)
        {
            User user = await Context.Users.Where(x => x.Name.ToLower() == username.ToLower()).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ResourceNotFoundException();
            }
            return user;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="isAdmin"></param>
        /// <returns>The created user.</returns>
        /// <exception cref="ResourceAlreadyExistException">If the username is already taken.</exception>
        public async Task<User> RegisterUser(string username, string password, bool isAdmin)
        {
            try
            {
                User alreadyExists = await GetUser(username);
                if (alreadyExists != null)
                {
                    throw new ResourceAlreadyExistException();
                }
            } catch (ResourceNotFoundException) { }

            using var hmac = new HMACSHA512();

            User createUser = new()
            {
                Name = username,
                TokenSalt = hmac.Key,
                TokenHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                IsAdmin = isAdmin
            };

            Context.Users.Add(createUser);
            await Context.SaveChangesAsync();

            return createUser;
        }

        public async Task<bool> CheckLogin(string username, string password)
        {
            User user = await GetUser(username);

            using var hmac = new HMACSHA512(user.TokenSalt);
            byte[] checkPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            bool isValid = true;

            if (checkPassword.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < user.TokenHash.Length; i++)
            {
                byte checkCharacter = 0;
                byte correctCharacter = 1;
                if (checkPassword.Length >= i)
                {
                    checkCharacter = checkPassword[i];
                    correctCharacter = user.TokenHash[i];
                }
                if (correctCharacter != checkCharacter) isValid = false;
            }

            return isValid;
        }

        public async Task DeleteUser(string username)
        {
            User user = await GetUser(username);

            Context.Users.Remove(user);
            await Context.SaveChangesAsync();
        }

        public async Task<User> UpdatePassword(string username, string newPassword)
        {
            User user = await GetUser(username);

            using var hmac = new HMACSHA512();

            user.TokenSalt = hmac.Key;
            user.TokenHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateAdminStatus(string username, bool isAdmin)
        {
            User user = await GetUser(username);

            user.IsAdmin = isAdmin;

            Context.Users.Update(user);
            await Context.SaveChangesAsync();

            return user;
        }
    }
}
