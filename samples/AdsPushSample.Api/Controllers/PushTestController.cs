using AdsPush;
using AdsPush.Abstraction;
using AdsPushSample.Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AdsPushSample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PushTestController : ControllerBase
{
    private readonly IAdsPushSender _pushSender;

    public PushTestController(
        IAdsPushSenderFactory adsPushSenderFactory)
    {
        this._pushSender = adsPushSenderFactory.GetSender("MyApp");
    }

    [HttpPost("send/basic")]
    public async Task<IActionResult> SendBasicAsync(
        [FromBody] BasicSendRequest request,
        CancellationToken cancellationToken)
    {
        await _pushSender.BasicSendAsync(
            request.Target,
            request.PushToken,
            new()
            {
                Title = AdsPushText.CreateUsingString(request.Title),
                Detail = AdsPushText.CreateUsingString(request.Detail)
            },
            cancellationToken);

        return Ok();
    }
}