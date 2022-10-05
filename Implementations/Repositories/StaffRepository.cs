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
    public class StaffRepository : BaseRepository<Staff>, IStaffRepository
    {
        public StaffRepository(ApplicationContext Context)
        {
            _Context = Context;
        }

        public async Task<ICollection<Staff>> GetAllStaffsAsync()
        {
            var staffs = await _Context.Staffs.Where(adm => adm.IsDeleted == false).Include(admin => admin.User).ToListAsync();
            return staffs;
        }
        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            var staff = await _Context.Staffs.Where(adm => adm.Id == id && adm.IsDeleted == false).Include(admin => admin.User).FirstOrDefaultAsync();
            return staff;
        }
    }
}
