using Azure.Messaging.ServiceBus;
using Bogus;
using Common.Entities;
using Newtonsoft.Json;

namespace Web.Services
{
    public interface IMessageGenerator
    {
        Task QueueOrdersAsync(int requestedAmount);
    }

    public class MessageGenerator : IMessageGenerator
    {
        private readonly ServiceBusClient _client;

        public MessageGenerator(IConfiguration configuration)
        {
            var serviceBusConnectionString = configuration.GetConnectionString("ServiceBus");
            _client = new ServiceBusClient(serviceBusConnectionString);
        }

        public async Task QueueOrdersAsync(int requestedAmount)
        {
            var serviceBusSender = _client.CreateSender("orders");

            for (int currentOrderAmount = 0; currentOrderAmount < requestedAmount; currentOrderAmount++)
            {
                var order = GenerateOrder();
                var rawOrder = JsonConvert.SerializeObject(order);
                var orderMessage = new ServiceBusMessage(rawOrder);

                await serviceBusSender.SendMessageAsync(orderMessage);
            }
        }

        private static Order GenerateOrder()
        {
            var customerGenerator = new Faker<Customer>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName());

            var orderGenerator = new Faker<Order>()
                .RuleFor(u => u.Customer, () => customerGenerator)
                .RuleFor(u => u.Id, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.Amount, f => f.Random.Int())
                .RuleFor(u => u.ArticleNumber, f => f.Commerce.Product());

            return orderGenerator.Generate();
        }
    }
}
