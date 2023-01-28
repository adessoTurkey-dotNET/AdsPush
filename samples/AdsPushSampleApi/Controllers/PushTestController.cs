using AdsPush;
using Microsoft.AspNetCore.Mvc;

namespace AdsPushSampleApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PushTestController : ControllerBase
{
    private readonly IAdsPushSender _pushSender;

    public PushTestController(IAdsPushSenderFactory adsPushSenderFactory)
    {
        this._pushSender = adsPushSenderFactory.GetSender("MyApp");
    }

    [HttpPost("send/basic")]
    public async Task<IActionResult> SendBasicAsync()
    {
        return Ok();
    }
}