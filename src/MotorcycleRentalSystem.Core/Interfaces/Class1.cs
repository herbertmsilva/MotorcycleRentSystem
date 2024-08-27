using MotorcycleRentalSystem.Core.Entities.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Core.Interfaces
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        Task<UserEntity> GetUserByUsernameAsync(string username);
    }
}
