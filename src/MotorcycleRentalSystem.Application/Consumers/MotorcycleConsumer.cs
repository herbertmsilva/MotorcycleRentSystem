using MotorcycleRentalSystem.Core.Interfaces;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using MotorcycleRentalSystem.Application.Events;

namespace MotorcycleRentalSystem.Application.Consumers
{
    public class MotorcycleConsumer : IMessageConsumer<MotorcycleRegisteredEvent>
    {
        private readonly IEventRepository<MotorcycleRegisteredEvent> _mongoEventRepository;

        public MotorcycleConsumer(IEventRepository<MotorcycleRegisteredEvent> mongoEventRepository)
        {
            _mongoEventRepository = mongoEventRepository;
        }

        public async Task HandleAsync(MotorcycleRegisteredEvent motorcycleEvent)
        {
            if (motorcycleEvent.Year == 2024)
            {
                await _mongoEventRepository.AddAsync(motorcycleEvent);
            }
        }
    }
}
