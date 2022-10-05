using Microsoft.EntityFrameworkCore;
using PrivateEye.Context;
using PrivateEye.Entites;
using PrivateEye.Implementation.Repositries;
using PrivateEye.InterFaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.Implementations.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(ApplicationContext Context)
        {
            _Context = Context;
        }

        public async Task<ICollection<Security>> GetAllSecuritiesAsync()
        {
            var security = await _Context.Securities.Where(adm => adm.IsDeleted == false).Include(admin => admin.User).ToListAsync();
            return security;
        }

        public async Task<Security> GetSecurityByIdAsync(int id)
        {
            var security = await _Context.Securities.Where(adm => adm.Id == id && adm.IsDeleted == false).Include(admin => admin.User).FirstOrDefaultAsync();
            return security;
        }
    }
}
