using PrivateEye.Entites;
using PrivateEye.Interface.IRespositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateEye.InterFaces.IRepositories
{
    public interface IStaffRepository : IBaseRepository<Staff>
    {
        Task<ICollection<Staff>> GetAllStaffsAsync();
        Task<Staff> GetStaffByIdAsync(int id);
    }
}
