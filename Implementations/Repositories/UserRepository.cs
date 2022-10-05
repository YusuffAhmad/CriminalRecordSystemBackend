using Microsoft.EntityFrameworkCore;
using PrivateEye.Context;
using PrivateEye.DTOs;
using PrivateEye.Identity;
using PrivateEye.Implementation.Repositries;
using PrivateEye.InterFaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Implementations.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<User> ExistsByEmailAsync(string Email, string password)
        {
            var user = await _Context.Users.Include(user => user.UserRoles).ThenInclude( x => x.Role).FirstOrDefaultAsync(x => x.Email == Email && x.IsDeleted == false);
            return user;
        }

        public async Task<List<string>> GetAllUserEmailAsync()
        {
            var userEmails = await _Context.Users.Select(x => x.Email).ToListAsync();
            return userEmails;
        }
    }
}
