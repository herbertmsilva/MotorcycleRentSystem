using MotorcycleRentalSystem.Core.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Core.Interfaces
{
    public interface ILogRepository
    {
        Task AddLogAsync(LogEntry logEntry);
        Task<IEnumerable<LogEntry>> GetAllLogsAsync();
    }
}
