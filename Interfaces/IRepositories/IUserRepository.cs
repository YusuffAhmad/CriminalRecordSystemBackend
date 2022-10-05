using PrivateEye.DTOs;
using PrivateEye.Identity;
using PrivateEye.Implementation.Repositries;
using PrivateEye.Interface.IRespositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
       Task<User> ExistsByEmailAsync(string Email, string password);
       Task<List<string>> GetAllUserEmailAsync();
    }
}
