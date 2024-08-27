using MongoDB.Driver;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;
using MotorcycleRentalSystem.Core.Entities.Mongo;
using MotorcycleRentalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Persistence.Repositories.Mongo
{
    public class MongoLogRepository : ILogRepository
    {
        private readonly IMongoCollection<LogEntry> _logCollection;

        public MongoLogRepository(IMongoDbService mongoDbService) => _logCollection = mongoDbService.GetCollection<LogEntry>("SystemLogs");

        public async Task AddLogAsync(LogEntry logEntry) => await _logCollection.InsertOneAsync(logEntry);

        public async Task<IEnumerable<LogEntry>> GetAllLogsAsync() => await _logCollection.Find(Builders<LogEntry>.Filter.Empty).ToListAsync();
    }
}
