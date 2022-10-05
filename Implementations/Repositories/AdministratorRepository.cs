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
    public class AdministratorRepository : BaseRepository<Administrator>, IAdministartorRepository
    {
        public AdministratorRepository(ApplicationContext Context)
        {
            _Context = Context;
        }
        public async Task<List<Administrator>> GetAdministratorsAsync()
        {
           var admins = await _Context.Administrators.Where(adm => adm.IsDeleted == false).Include(admin => admin.User).ToListAsync();
            return admins;
        }
        
        public async Task<Administrator> GetAdminByIdAsync(int id)
        {
           var admins = await _Context.Administrators.Where(adm => adm.Id == id && adm.IsDeleted == false).Include(admin => admin.User).FirstOrDefaultAsync();
            return admins;
        }
    }
}
