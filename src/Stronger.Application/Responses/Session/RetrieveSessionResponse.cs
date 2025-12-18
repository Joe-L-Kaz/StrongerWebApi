using System;
using Stronger.Domain.Common;

namespace Stronger.Application.Responses.Session;

public class RetrieveSessionResponse : SessionResponseBase
{
    public SessionData SessionData {get; set;}
    public DateOnly CompletedAt {get; set;}

    public RetrieveSessionResponse()
    {
        SessionData = new();
    }
}
