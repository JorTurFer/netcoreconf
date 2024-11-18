using Azure.Messaging.ServiceBus.Administration;
using Common.Data;
using Web.Entities;

namespace Web.Services
{

    public interface IStatusService
    {
        Task<Status> GetStatusAsync();
    }

    public class StatusService : IStatusService
    {
        private readonly ServiceBusAdministrationClient _client;
        private KeepAliveDbContext _context { get; }

        public StatusService(IConfiguration configuration, KeepAliveDbContext context)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBus");
            _client = new ServiceBusAdministrationClient(serviceBusConnectionString);
            _context = context;
        }


        public async Task<Status> GetStatusAsync()
        {
            var queueInfo = await _client.GetQueueRuntimePropertiesAsync("orders");
            var from = DateTime.UtcNow.AddSeconds(-20);
            var workers = _context.KeepAlives
                                    .Where(x=>x.LastPing > from)
                                    .AsEnumerable()
                                    .GroupBy(x => x.Name)
                                    .Count();

            return new Status
            {
                MessageCount = queueInfo.Value.TotalMessageCount,
                WorkersCount = workers
            };
        }
    }
}
