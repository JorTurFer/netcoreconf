using Microsoft.AspNetCore.Mvc;
using Web.Entities;
using Web.Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QueueController : ControllerBase
    {
        protected IStatusService _statusService { get; }
        public IMessageGenerator _messageGenerator { get; }

        public QueueController(IStatusService statusService, IMessageGenerator messageGenerator)
        {
            _statusService = statusService;
            _messageGenerator = messageGenerator;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Status> Get()
        {
            return await _statusService.GetStatusAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessages(int count)
        {
            await _messageGenerator.QueueOrdersAsync(count);
            return Ok();
        }
    }
}