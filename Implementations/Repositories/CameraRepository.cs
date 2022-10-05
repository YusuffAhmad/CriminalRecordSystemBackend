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
    public class CameraRepository : BaseRepository<Camera>, ICameraRepository
    {
        public CameraRepository(ApplicationContext Context)
        {
            _Context = Context;
        }
    }
}
