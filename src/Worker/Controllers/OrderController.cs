using Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace worker.Controllers
{
    [Route("service-bus-queue")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] Order order)
        {
            _logger.LogInformation("Processing order {OrderId} for {OrderAmount} units of {OrderArticle} bought by {CustomerFirstName} {CustomerLastName}", order.Id, order.Amount, order.ArticleNumber, order.Customer?.FirstName, order.Customer?.LastName);
            await Task.Delay(TimeSpan.FromSeconds(2));
            _logger.LogInformation("Order {OrderId} processed", order.Id);
            
            return Ok();
        }
    }
}
