using Azure.Messaging.ServiceBus.Administration;
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
        
        public StatusService(IConfiguration configuration)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBus");
            _client = new ServiceBusAdministrationClient(serviceBusConnectionString);
        }


        public async Task<Status> GetStatusAsync()
        {
            var queueInfo = await _client.GetQueueRuntimePropertiesAsync("orders");

            return new Status
            {
                MessageCount = queueInfo.Value.TotalMessageCount,
            };
        }
    }
}
