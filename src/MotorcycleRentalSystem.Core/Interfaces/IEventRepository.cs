using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Core.Interfaces
{
    public interface IEventRepository<T>
    {
        Task AddAsync(T eventItem);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
