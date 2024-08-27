using Microsoft.EntityFrameworkCore;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;
using MotorcycleRentalSystem.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Persistence.Repositories.Postgres
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UserEntity> GetUserByUsernameAsync(string username)
            => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}
