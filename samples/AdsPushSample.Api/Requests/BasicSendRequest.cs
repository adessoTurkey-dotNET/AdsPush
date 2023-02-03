using AdsPush.Abstraction;

namespace AdsPushSample.Api.Requests;

public class BasicSendRequest
{
    public AdsPushTarget Target { get; set; }
    public string PushToken { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
}