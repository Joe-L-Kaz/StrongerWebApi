using System;

namespace Stronger.Application.Responses.Ping;

public class PingResponse
{
    public String Message { get; set; }

    public PingResponse()
    {
        Message = String.Empty;
    }
}
