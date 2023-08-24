using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ServerSentEventsExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SSEController : ControllerBase
    {
        [HttpGet("events")]
        public async Task GetEvents(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            for (var i = 0; i < 10; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var data = Encoding.UTF8.GetBytes($"data: Event {i}\n\n");
                await Response.Body.WriteAsync(data, 0, data.Length);
                await Response.Body.FlushAsync();

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
